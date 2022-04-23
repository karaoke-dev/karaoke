// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class LyricPlayfield : Playfield
    {
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

        protected override HitObjectLifetimeEntry CreateLifetimeEntry(HitObject hitObject) => new LyricHitObjectLifetimeEntry(hitObject);

        private class LyricHitObjectLifetimeEntry : HitObjectLifetimeEntry
        {
            public LyricHitObjectLifetimeEntry(HitObject hitObject)
                : base(hitObject)
            {
                // Manually set to reduce the number of future alive objects to a bare minimum.
                LifetimeEnd = Lyric.EndTime;
                LifetimeStart = HitObject.StartTime - Lyric.TimePreempt;
            }

            protected Lyric Lyric => (Lyric)HitObject;

            protected override double InitialLifetimeOffset => Lyric.TimePreempt;
        }
    }
}
