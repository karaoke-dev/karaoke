// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Game.Beatmaps;
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
        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        protected WorkingBeatmap WorkingBeatmap => beatmap.Value;

        private readonly BindableDouble preemptTime = new();
        private readonly Bindable<Lyric[]> nowLyrics = new();
        private readonly Cached seekCache = new();

        public LyricPlayfield()
        {
            // Switch to target time
            nowLyrics.BindValueChanged(value =>
            {
                if (!seekCache.IsValid || value.NewValue == null)
                    return;

                double lyricStartTime = value.NewValue.Select(x => x.LyricStartTime).Min() - preemptTime.Value;

                WorkingBeatmap.Track.Seek(lyricStartTime);
            });

            seekCache.Validate();
        }

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
            var lyrics = nowLyrics.Value ?? Array.Empty<Lyric>();
            var lyric = drawableLyric.HitObject;

            if (lyrics.Contains(lyric))
                return;

            seekCache.Invalidate();

            nowLyrics.Value = lyrics.Concat(new[] { lyric }).ToArray();

            seekCache.Validate();
        }

        private void onLyricEnd(DrawableLyric drawableLyric)
        {
            var lyrics = nowLyrics.Value ?? Array.Empty<Lyric>();
            var lyric = drawableLyric.HitObject;

            if (!lyrics.Contains(lyric))
                return;

            seekCache.Invalidate();

            nowLyrics.Value = lyrics.Where(x => x != lyric).ToArray();

            seekCache.Validate();
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager rulesetConfig, KaraokeSessionStatics session)
        {
            // Practice
            rulesetConfig.BindWith(KaraokeRulesetSetting.PracticePreemptTime, preemptTime);
            session.BindWith(KaraokeRulesetSession.NowLyrics, nowLyrics);

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
