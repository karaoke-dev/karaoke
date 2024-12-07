// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Stages.Drawables;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.UI;

public partial class LyricPlayfield : Playfield
{
    [Resolved]
    private IStageHitObjectRunner? stageRunner { get; set; }

    private readonly Bindable<Lyric[]> singingLyrics = new();

    protected override void OnNewDrawableHitObject(DrawableHitObject drawableHitObject)
    {
        if (drawableHitObject is DrawableLyric drawableLyric)
        {
            drawableLyric.OnLyricStart += onLyricStart;
            drawableLyric.OnLyricEnd += onLyricEnd;
        }

        base.OnNewDrawableHitObject(drawableHitObject);
    }

    private void onLyricStart(DrawableLyric drawableLyric)
    {
        var lyrics = singingLyrics.Value ?? Array.Empty<Lyric>();
        var lyric = drawableLyric.HitObject;

        if (lyrics.Contains(lyric))
            return;

        singingLyrics.Value = lyrics.Concat(new[] { lyric }).ToArray();
    }

    private void onLyricEnd(DrawableLyric drawableLyric)
    {
        var lyrics = singingLyrics.Value ?? Array.Empty<Lyric>();
        var lyric = drawableLyric.HitObject;

        if (!lyrics.Contains(lyric))
            return;

        singingLyrics.Value = lyrics.Where(x => x != lyric).ToArray();
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeSessionStatics session)
    {
        // Practice
        session.BindWith(KaraokeRulesetSession.SingingLyrics, singingLyrics);

        RegisterPool<Lyric, DrawableLyric>(50);
    }

    protected override HitObjectLifetimeEntry CreateLifetimeEntry(HitObject hitObject) => new LyricHitObjectLifetimeEntry(hitObject, stageRunner);

    private class LyricHitObjectLifetimeEntry : HitObjectLifetimeEntry
    {
        private readonly IStageHitObjectRunner? stageRunner;

        public LyricHitObjectLifetimeEntry(HitObject hitObject, IStageHitObjectRunner? runner)
            : base(hitObject)
        {
            stageRunner = runner;
            if (stageRunner == null)
                return;

            // Manually set to reduce the number of future alive objects to a bare minimum.
            updateLifetime();

            stageRunner.OnCommandUpdated += updateLifetime;
        }

        private void updateLifetime()
        {
            if (stageRunner == null)
                throw new InvalidOperationException();

            // follow the same event as SetInitialLifetime() in the base class.
            LifetimeStart = HitObject.StartTime - InitialLifetimeOffset;
            LifetimeEnd = lyric.EndTime + stageRunner.GetEndTimeOffset(lyric);
        }

        private Lyric lyric => (Lyric)HitObject;

        protected override double InitialLifetimeOffset
        {
            get
            {
                if (stageRunner == null)
                    return base.InitialLifetimeOffset;

                return stageRunner.GetStartTimeOffset(lyric) + stageRunner.GetPreemptTime(HitObject);
            }
        }
    }
}
