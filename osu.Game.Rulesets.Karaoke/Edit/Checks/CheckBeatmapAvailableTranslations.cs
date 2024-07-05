// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckBeatmapAvailableTranslations : CheckBeatmapProperty<IList<CultureInfo>, Lyric>
{
    protected override string Description => "Beatmap with invalid localization info.";

    public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
    {
        new IssueTemplateMissingTranslation(this),
        new IssueTemplateMissingPartialTranslation(this),
        new IssueTemplateTranslationNotInListedLanguage(this),
    };

    protected override IList<CultureInfo> GetPropertyFromBeatmap(KaraokeBeatmap karaokeBeatmap)
        => karaokeBeatmap.AvailableTranslationLanguages;

    protected override IEnumerable<Issue> CheckProperty(IList<CultureInfo> property)
    {
        // todo: maybe check duplicated languages?
        yield break;
    }

    protected override IEnumerable<Issue> CheckHitObjects(IList<CultureInfo> property, IReadOnlyList<Lyric> hitObject)
    {
        if (hitObject.Count == 0)
            yield break;

        // check if some translations is missing or empty.
        foreach (var language in property)
        {
            var missingTranslationLyrics = hitObject.Where(x => !x.Translations.ContainsKey(language) || string.IsNullOrWhiteSpace(x.Translations[language])).ToArray();

            if (missingTranslationLyrics.Length == hitObject.Count)
            {
                yield return new IssueTemplateMissingTranslation(this).Create(missingTranslationLyrics, language);
            }
            else if (missingTranslationLyrics.Any())
            {
                yield return new IssueTemplateMissingPartialTranslation(this).Create(missingTranslationLyrics, language);
            }
        }

        // should check is lyric contains translation that is not listed in beatmap.
        // if got this issue, then it's a bug.
        var allTranslationLanguageInLyric = hitObject.SelectMany(x => x.Translations.Keys).Distinct();
        var languageNotListInBeatmap = allTranslationLanguageInLyric.Except(property);

        foreach (var language in languageNotListInBeatmap)
        {
            var notContainsTranslationLyrics = hitObject.Where(x => !x.Translations.ContainsKey(language));

            yield return new IssueTemplateTranslationNotInListedLanguage(this).Create(notContainsTranslationLyrics, language);
        }
    }

    public class IssueTemplateMissingTranslation : IssueTemplate
    {
        public IssueTemplateMissingTranslation(ICheck check)
            : base(check, IssueType.Problem, "There are no lyric translations for this language.")
        {
        }

        public Issue Create(IEnumerable<HitObject> hitObjects, CultureInfo cultureInfo)
            => new(hitObjects, this, cultureInfo);
    }

    public class IssueTemplateMissingPartialTranslation : IssueTemplate
    {
        public IssueTemplateMissingPartialTranslation(ICheck check)
            : base(check, IssueType.Problem, "Some lyrics in this language are missing translations.")
        {
        }

        public Issue Create(IEnumerable<HitObject> hitObjects, CultureInfo cultureInfo)
            => new(hitObjects, this, cultureInfo);
    }

    public class IssueTemplateTranslationNotInListedLanguage : IssueTemplate
    {
        public IssueTemplateTranslationNotInListedLanguage(ICheck check)
            : base(check, IssueType.Problem, "Seems some translation language is not listed in the beatmap, please contact developer to fix that bug.")
        {
        }

        public Issue Create(IEnumerable<HitObject> hitObjects, CultureInfo cultureInfo)
            => new(hitObjects, this, cultureInfo);
    }
}
