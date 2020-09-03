// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class LyricPlayfield : Playfield
    {
        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public IBeatmap Beatmap => beatmap.Value.Beatmap;

        public new IEnumerable<DrawableLyricLine> AllHitObjects => base.AllHitObjects.OfType<DrawableLyricLine>();

        protected WorkingBeatmap WorkingBeatmap => beatmap.Value;

        private readonly BindableBool translate = new BindableBool();
        private readonly Bindable<string> translateLanguage = new Bindable<string>();

        private readonly BindableBool displayRuby = new BindableBool();
        private readonly BindableBool displayRomaji = new BindableBool();

        private readonly BindableDouble preemptTime = new BindableDouble();
        private readonly Bindable<LyricLine> nowLyric = new Bindable<LyricLine>();
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

        private void updateLyricTranslate()
        {
            var isTranslate = translate.Value;
            var targetLanguage = translateLanguage.Value;

            var lyric = Beatmap.HitObjects.OfType<LyricLine>().ToList();
            var translateDictionary = Beatmap.GetProperty();

            // Clear exist translate
            lyric.ForEach(x => x.TranslateText = null);

            // If contain target language
            if (isTranslate && targetLanguage != null
                            && Beatmap.AvailableTranslates().Contains(targetLanguage))
            {
                var translate = Beatmap.GetTranslate(targetLanguage);

                // Apply translate
                for (int i = 0; i < Math.Min(lyric.Count, translate.Count); i++)
                {
                    lyric[i].TranslateText = translate[i];
                }
            }
        }

        public override void Add(DrawableHitObject h)
        {
            if (h is DrawableLyricLine drawableLyric)
            {
                drawableLyric.OnLyricStart += OnNewResult;
                drawableLyric.DisplayRuby = displayRuby.Value;
                drawableLyric.DisplayRomaji = displayRomaji.Value;
            }

            h.OnNewResult += OnNewResult;
            base.Add(h);
        }

        public override bool Remove(DrawableHitObject h)
        {
            if (h is DrawableLyricLine drawableLyric)
                drawableLyric.OnLyricStart -= OnNewResult;

            h.OnNewResult -= OnNewResult;
            return base.Remove(h);
        }

        internal void OnNewResult(DrawableHitObject judgedObject, JudgementResult result)
        {
            if (!(result.Judgement is KaraokeLyricJudgement karaokeLyricJudgement))
                return;

            // Update now lyric
            var targetLyric = karaokeLyricJudgement.Time == LyricTime.Available ? judgedObject.HitObject as LyricLine : null;
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
        }
    }
}
