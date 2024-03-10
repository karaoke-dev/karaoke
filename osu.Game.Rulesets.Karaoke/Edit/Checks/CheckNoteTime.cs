// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckNoteTime : CheckHitObjectProperty<Note>
{
    public const double MIN_DURATION = 100;

    public const double MAX_DURATION = 10000;

    protected override string Description => "Note with invalid timing in the note.";

    public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
    {
        new IssueTemplateInvalidReferenceTimeTagTime(this),
        new IssueTemplateDurationTooShort(this),
        new IssueTemplateDurationTooLong(this),
    };

    protected override IEnumerable<Issue> Check(Note note)
    {
        // because lyric's start and end time is assigned by the reference lyric, so should skip the check if note does not contains the reference lyric.
        var referenceLyric = note.ReferenceLyric;
        if (referenceLyric == null)
            yield break;

        //  should make sure that reference time-tag has time.
        // if contains no time, will reported in the CheckNoteReferenceLyric.
        double? startTime = note.StartReferenceTimeTag?.Time;
        double? endTime = note.EndReferenceTimeTag?.Time;
        if (startTime == null || endTime == null)
            yield break;

        // should have alert if reference time-tag's time is invalid.
        if (endTime.Value < startTime.Value)
        {
            yield return new IssueTemplateInvalidReferenceTimeTagTime(this).Create(note);

            yield break;
        }

        // note's duration should be in the range.
        switch (note.Duration)
        {
            case < MIN_DURATION:
                yield return new IssueTemplateDurationTooShort(this).Create(note);

                break;

            case > MAX_DURATION:
                yield return new IssueTemplateDurationTooLong(this).Create(note);

                break;
        }

        // todo: check for offset time's range.
    }

    public class IssueTemplateInvalidReferenceTimeTagTime : IssueTemplate
    {
        public IssueTemplateInvalidReferenceTimeTagTime(ICheck check)
            : base(check, IssueType.Problem, "Note must have text.")
        {
        }

        public Issue Create(Note note)
            => new NoteIssue(note, this);
    }

    public class IssueTemplateDurationTooShort : IssueTemplate
    {
        public IssueTemplateDurationTooShort(ICheck check)
            : base(check, IssueType.Problem, "Note's duration too short.")
        {
        }

        public Issue Create(Note note)
            => new NoteIssue(note, this);
    }

    public class IssueTemplateDurationTooLong : IssueTemplate
    {
        public IssueTemplateDurationTooLong(ICheck check)
            : base(check, IssueType.Problem, "Note's duration too long.")
        {
        }

        public Issue Create(Note note)
            => new NoteIssue(note, this);
    }
}
