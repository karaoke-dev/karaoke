// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckBeatmapAvailableTranslates : CheckBeatmapProperty<IList<CultureInfo>, Lyric>
    {
        protected override string Description => "Lyrics with invalid translations.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateMissingTranslate(this),
            new IssueTemplateMissingPartialTranslate(this),
            new IssueTemplateContainsNotListedLanguage(this)
        };

        protected override IList<CultureInfo> GetPropertyFromBeatmap(KaraokeBeatmap karaokeBeatmap)
            => karaokeBeatmap.AvailableTranslates;

        protected override IEnumerable<Issue> CheckProperty(IList<CultureInfo> property)
        {
            // todo: maybe check duplicated languages?
            yield break;
        }

        protected override IEnumerable<Issue> CheckHitObjects(IList<CultureInfo> property, IReadOnlyList<Lyric> hitObject)
        {
            if (hitObject.Count == 0)
                yield break;

            // check if some translate is missing or empty.
            foreach (var language in property)
            {
                var notTranslateLyrics = hitObject.Where(x => !x.Translates.ContainsKey(language) || string.IsNullOrWhiteSpace(x.Translates[language])).ToArray();

                if (notTranslateLyrics.Length == hitObject.Count)
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
            var allTranslateLanguageInLyric = hitObject.SelectMany(x => x.Translates.Keys).Distinct();
            var languageNotListInBeatmap = allTranslateLanguageInLyric.Except(property);

            foreach (var language in languageNotListInBeatmap)
            {
                var notTranslateLyrics = hitObject.Where(x => !x.Translates.ContainsKey(language));

                yield return new IssueTemplateContainsNotListedLanguage(this).Create(notTranslateLyrics, language);
            }
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
