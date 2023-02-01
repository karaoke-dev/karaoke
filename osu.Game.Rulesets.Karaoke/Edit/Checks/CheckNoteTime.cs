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
        new IssueTemplateNoteInvalidReferenceTimeTagTime(this),
        new IssueTemplateNoteDurationTooShort(this),
        new IssueTemplateNoteDurationTooLong(this)
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
            yield return new IssueTemplateNoteInvalidReferenceTimeTagTime(this).Create(note);

            yield break;
        }

        // note's duration should be in the range.
        switch (note.Duration)
        {
            case < MIN_DURATION:
                yield return new IssueTemplateNoteDurationTooShort(this).Create(note);

                break;

            case > MAX_DURATION:
                yield return new IssueTemplateNoteDurationTooLong(this).Create(note);

                break;
        }

        // todo: check for offset time's range.
    }

    public class IssueTemplateNoteInvalidReferenceTimeTagTime : IssueTemplate
    {
        public IssueTemplateNoteInvalidReferenceTimeTagTime(ICheck check)
            : base(check, IssueType.Problem, "Note must have text.")
        {
        }

        public Issue Create(Note note)
            => new NoteIssue(note, this);
    }

    public class IssueTemplateNoteDurationTooShort : IssueTemplate
    {
        public IssueTemplateNoteDurationTooShort(ICheck check)
            : base(check, IssueType.Problem, "Note's duration too short.")
        {
        }

        public Issue Create(Note note)
            => new NoteIssue(note, this);
    }

    public class IssueTemplateNoteDurationTooLong : IssueTemplate
    {
        public IssueTemplateNoteDurationTooLong(ICheck check)
            : base(check, IssueType.Problem, "Note's duration too long.")
        {
        }

        public Issue Create(Note note)
            => new NoteIssue(note, this);
    }
}
