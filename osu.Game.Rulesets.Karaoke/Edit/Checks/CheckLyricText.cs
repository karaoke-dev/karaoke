// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckLyricText : CheckHitObjectProperty<Lyric>
    {
        protected override string Description => "Lyric with invalid text.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateNoText(this),
        };

        protected override IEnumerable<Issue> Check(Lyric lyric)
        {
            if (string.IsNullOrWhiteSpace(lyric.Text))
                yield return new IssueTemplateNoText(this).Create(lyric);
        }

        public class IssueTemplateNoText : IssueTemplate
        {
            public IssueTemplateNoText(ICheck check)
                : base(check, IssueType.Problem, "Lyric must have text.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }
    }
}
