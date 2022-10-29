// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckLyricTranslate : CheckHitObjectProperty<Lyric>
    {
        protected override string Description => "Lyric with invalid translations.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateLyricTranslationNoText(this),
        };

        protected override IEnumerable<Issue> Check(Lyric lyric)
        {
            var translates = lyric.Translates;

            foreach ((var language, string translate) in translates)
            {
                if (string.IsNullOrWhiteSpace(translate))
                    yield return new IssueTemplateLyricTranslationNoText(this).Create(lyric, language);
            }
        }

        public class IssueTemplateLyricTranslationNoText : IssueTemplate
        {
            public IssueTemplateLyricTranslationNoText(ICheck check)
                : base(check, IssueType.Problem, "Translation in the lyric should not by empty or white-space only.")
            {
            }

            public Issue Create(Lyric lyric, CultureInfo language)
                => new(lyric, this, language);
        }
    }
}
