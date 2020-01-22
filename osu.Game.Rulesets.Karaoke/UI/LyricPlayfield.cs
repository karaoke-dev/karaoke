// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class LyricPlayfield : Playfield
    {
        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public IBeatmap Beatmap => beatmap.Value.Beatmap;

        private readonly BindableBool translate = new BindableBool();
        private readonly Bindable<string> translateLanguage = new Bindable<string>();

        public LyricPlayfield()
        {
            // Change need to translate
            translate.BindValueChanged(x => updateLyricTranslate());
            translateLanguage.BindValueChanged(x => updateLyricTranslate());
        }

        private void updateLyricTranslate()
        {
            var isTranslate = translate.Value;
            var translateLanguage = this.translateLanguage.Value;

            var lyric = Beatmap.HitObjects.OfType<LyricLine>().ToList();
            var translateDictionary = Beatmap.HitObjects.OfType<TranslateDictionary>().FirstOrDefault();

            // Clear exist translate
            lyric.ForEach(x => x.TranslateText = null);

            // If contain target language
            if (isTranslate && translateLanguage != null
                            && translateDictionary != null && translateDictionary.Translates.ContainsKey(translateLanguage))
            {
                var language = translateDictionary.Translates[translateLanguage];

                // Apply translate
                for (int i = 0; i < Math.Min(lyric.Count, language.Count); i++)
                {
                    lyric[i].TranslateText = language[i];
                }
            }
        }

        [BackgroundDependencyLoader]
        private void load(KaroakeSessionStatics session)
        {
            // Translate
            session.BindWith(KaraokeRulesetSession.UseTranslate, translate);
            session.BindWith(KaraokeRulesetSession.PreferLanguage, translateLanguage);
        }
    }
}
