// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckNoteText : CheckHitObjectProperty<Note>
    {
        protected override string Description => "Note with invalid text.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateNoteNoText(this),
        };

        protected override IEnumerable<Issue> Check(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Text))
                yield return new IssueTemplateNoteNoText(this).Create(note);

            if (note.RubyText != null && string.IsNullOrWhiteSpace(note.RubyText))
                yield return new IssueTemplateNoteEmptyRubyText(this).Create(note);
        }

        public class IssueTemplateNoteNoText : IssueTemplate
        {
            public IssueTemplateNoteNoText(ICheck check)
                : base(check, IssueType.Problem, "Note must have text.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateNoteEmptyRubyText : IssueTemplate
        {
            public IssueTemplateNoteEmptyRubyText(ICheck check)
                : base(check, IssueType.Error, "Note's ruby text should be null or has the value.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }
    }
}
