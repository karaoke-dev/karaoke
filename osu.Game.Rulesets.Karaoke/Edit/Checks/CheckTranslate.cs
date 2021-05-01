// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckTranslate : ICheck
    {
        public CheckMetadata Metadata => new CheckMetadata(CheckCategory.Metadata, "Unfinished translate language.");

        public IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateMissingTranslate(this),
            new IssueTemplateMissingPartialTranslate(this),
        };

        public IEnumerable<Issue> Run(IBeatmap playableBeatmap, IWorkingBeatmap workingBeatmap)
        {
            var languages = availableTranslateInBeatmap(playableBeatmap);
            if (languages == null || languages.Length == 0)
                yield break;

            var lyric = playableBeatmap.HitObjects.OfType<Lyric>().ToList();
            if (lyric.Count == 0)
                yield break;

            foreach (var language in languages)
            {
                var notTranslateLyrics = lyric.Where(x =>
                {
                    if (!x.Translates.ContainsKey(language))
                        return true;

                    if (string.IsNullOrWhiteSpace(x.Translates[language]))
                        return true;

                    return false;
                });

                if (notTranslateLyrics.Count() == lyric.Count)
                {
                    yield return new IssueTemplateMissingTranslate(this).Create(language);
                }
                else if (notTranslateLyrics.Any())
                {
                    yield return new IssueTemplateMissingPartialTranslate(this).Create(language);
                }
            }
        }

        private CultureInfo[] availableTranslateInBeatmap(IBeatmap beatmap)
        {
            if (beatmap is EditorBeatmap editorBeatmap)
            {
                if (editorBeatmap.PlayableBeatmap is KaraokeBeatmap karaokeBeatmap)
                {
                    return karaokeBeatmap.AvailableTranslates;
                }
            }

            return null;
        }

        public class IssueTemplateMissingTranslate : IssueTemplate
        {
            public IssueTemplateMissingTranslate(ICheck check)
                : base(check, IssueType.Problem, "This language does not have any translate in lyric.")
            {
            }

            public Issue Create(CultureInfo cultureInfo)
                => new Issue(this, cultureInfo);
        }

        public class IssueTemplateMissingPartialTranslate : IssueTemplate
        {
            public IssueTemplateMissingPartialTranslate(ICheck check)
                : base(check, IssueType.Problem, "This language does missing translate in some lyric.")
            {
            }

            public Issue Create(CultureInfo cultureInfo)
                => new Issue(this, cultureInfo);
        }
    }
}
