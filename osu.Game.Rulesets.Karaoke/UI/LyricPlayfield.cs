// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Judgements;
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

        public new IEnumerable<DrawableLyric> AllHitObjects => base.AllHitObjects.OfType<DrawableLyric>();

        protected WorkingBeatmap WorkingBeatmap => beatmap.Value;

        private readonly BindableBool translate = new BindableBool();
        private readonly Bindable<CultureInfo> translateLanguage = new Bindable<CultureInfo>();

        private readonly BindableBool displayRuby = new BindableBool();
        private readonly BindableBool displayRomaji = new BindableBool();

        private readonly BindableDouble preemptTime = new BindableDouble();
        private readonly Bindable<Lyric> nowLyric = new Bindable<Lyric>();
        private readonly Cached seekCache = new Cached();

        public LyricPlayfield()
        {
            // Change need to translate
            translate.BindValueChanged(x => updateLyricTranslate());
            translateLanguage.BindValueChanged(x => updateLyricTranslate());

            // Change display ruby/romaji or not
            displayRuby.BindValueChanged(x => AllHitObjects.ForEach(d => d.DisplayRuby = x.NewValue));
            displayRomaji.BindValueChanged(x => AllHitObjects.ForEach(d => d.DisplayRomaji = x.NewValue));

            // Switch to target time
            nowLyric.BindValueChanged(value =>
            {
                if (!seekCache.IsValid || value.NewValue == null)
                    return;

                var lyricStartTime = value.NewValue.LyricStartTime - preemptTime.Value;

                WorkingBeatmap.Track.Seek(lyricStartTime);
            });

            seekCache.Validate();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            NewResult += OnNewResult;
        }

        private void updateLyricTranslate()
        {
            var isTranslate = translate.Value;
            var targetLanguage = isTranslate ? translateLanguage.Value : null;

            // apply target translate language
            AllHitObjects.ForEach(x => x.DisplayTranslateLanguage = targetLanguage);
        }

        protected override void OnNewDrawableHitObject(DrawableHitObject drawableHitObject)
        {
            if (drawableHitObject is DrawableLyric drawableLyric)
            {
                // todo : not really sure should cancel binding action in here?
                drawableLyric.OnLyricStart += OnNewResult;
                drawableLyric.DisplayRuby = displayRuby.Value;
                drawableLyric.DisplayRomaji = displayRomaji.Value;
            }

            base.OnNewDrawableHitObject(drawableHitObject);
        }

        internal void OnNewResult(DrawableHitObject judgedObject, JudgementResult result)
        {
            if (!(result.Judgement is KaraokeLyricJudgement karaokeLyricJudgement))
                return;

            // Update now lyric
            var targetLyric = karaokeLyricJudgement.Time == LyricTime.Available ? judgedObject.HitObject as Lyric : null;
            seekCache.Invalidate();
            nowLyric.Value = targetLyric;
            seekCache.Validate();
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager rulesetConfig, KaraokeSessionStatics session)
        {
            // Translate
            session.BindWith(KaraokeRulesetSession.UseTranslate, translate);
            session.BindWith(KaraokeRulesetSession.PreferLanguage, translateLanguage);

            // Ruby/Romaji
            session.BindWith(KaraokeRulesetSession.DisplayRuby, displayRuby);
            session.BindWith(KaraokeRulesetSession.DisplayRomaji, displayRomaji);

            // Practice
            rulesetConfig.BindWith(KaraokeRulesetSetting.PracticePreemptTime, preemptTime);
            session.BindWith(KaraokeRulesetSession.NowLyric, nowLyric);

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
