// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckBeatmapAvailableTranslates : ICheck
    {
        public CheckMetadata Metadata => new(CheckCategory.HitObjects, "Lyrics with invalid translations.");

        public IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateMissingTranslate(this),
            new IssueTemplateMissingPartialTranslate(this),
        };

        public IEnumerable<Issue> Run(BeatmapVerifierContext context)
        {
            var languages = availableTranslateInBeatmap(context.Beatmap);

            var lyrics = context.Beatmap.HitObjects.OfType<Lyric>().ToList();
            if (lyrics.Count == 0)
                yield break;

            // check if some translate is missing or empty.
            foreach (var language in languages)
            {
                var notTranslateLyrics = lyrics.Where(x => !x.Translates.ContainsKey(language) || string.IsNullOrWhiteSpace(x.Translates[language])).ToArray();

                if (notTranslateLyrics.Length == lyrics.Count)
                {
                    yield return new IssueTemplateMissingTranslate(this).Create(notTranslateLyrics, language);
                }
                else if (notTranslateLyrics.Any())
                {
                    yield return new IssueTemplateMissingPartialTranslate(this).Create(notTranslateLyrics, language);
                }
            }

            // should check is lyric contains translate that is not listed in beatmap.
            // if got this issue, then it's a bug.
            var allTranslateLanguageInLyric = lyrics.SelectMany(x => x.Translates.Keys).Distinct();
            var languageNotListInBeatmap = allTranslateLanguageInLyric.Except(languages);

            foreach (var language in languageNotListInBeatmap)
            {
                var notTranslateLyrics = lyrics.Where(x => !x.Translates.ContainsKey(language));

                yield return new IssueTemplateContainsNotListedLanguage(this).Create(notTranslateLyrics, language);
            }
        }

        private IList<CultureInfo> availableTranslateInBeatmap(IBeatmap beatmap)
        {
            if (beatmap is not EditorBeatmap editorBeatmap)
                return Array.Empty<CultureInfo>();

            if (editorBeatmap.PlayableBeatmap is not KaraokeBeatmap karaokeBeatmap)
                return Array.Empty<CultureInfo>();

            return karaokeBeatmap.AvailableTranslates;
        }

        public class IssueTemplateMissingTranslate : IssueTemplate
        {
            public IssueTemplateMissingTranslate(ICheck check)
                : base(check, IssueType.Problem, "There are no lyric translations for this language.")
            {
            }

            public Issue Create(IEnumerable<HitObject> hitObjects, CultureInfo cultureInfo)
                => new(hitObjects, this, cultureInfo);
        }

        public class IssueTemplateMissingPartialTranslate : IssueTemplate
        {
            public IssueTemplateMissingPartialTranslate(ICheck check)
                : base(check, IssueType.Problem, "Some lyrics in this language are missing translations.")
            {
            }

            public Issue Create(IEnumerable<HitObject> hitObjects, CultureInfo cultureInfo)
                => new(hitObjects, this, cultureInfo);
        }

        public class IssueTemplateContainsNotListedLanguage : IssueTemplate
        {
            public IssueTemplateContainsNotListedLanguage(ICheck check)
                : base(check, IssueType.Problem, "Seems some translate language is not listed, plz contact developer to fix that bug.")
            {
            }

            public Issue Create(IEnumerable<HitObject> hitObjects, CultureInfo cultureInfo)
                => new(hitObjects, this, cultureInfo);
        }
    }
}
