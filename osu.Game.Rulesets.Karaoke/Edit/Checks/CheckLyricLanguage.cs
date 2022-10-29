// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckLyricLanguage : CheckHitObjectProperty<Lyric>
    {
        protected override string Description => "Lyric with invalid language.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateLyricNotFillLanguage(this),
        };

        protected override IEnumerable<Issue> Check(Lyric lyric)
        {
            if (lyric.Language == null)
                yield return new IssueTemplateLyricNotFillLanguage(this).Create(lyric);
        }

        public class IssueTemplateLyricNotFillLanguage : IssueTemplate
        {
            public IssueTemplateLyricNotFillLanguage(ICheck check)
                : base(check, IssueType.Problem, "Lyric must have assign language.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }
    }
}
