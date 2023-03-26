// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public class CheckBeatmapNoteInfo : CheckBeatmapProperty<NoteInfo, Note>
{
    public const int MIN_COLUMNS = 9;

    public const int MAX_COLUMNS = 12;

    protected override string Description => "Check invalid note info and the note that out of range.";

    public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
    {
        new IssueTemplateColumnNotEnough(this),
        new IssueTemplateColumnExceed(this),
        new IssueTemplateNoteToneTooLow(this),
        new IssueTemplateNoteToneTooHigh(this),
    };

    protected override NoteInfo GetPropertyFromBeatmap(KaraokeBeatmap karaokeBeatmap)
        => karaokeBeatmap.NoteInfo;

    protected override IEnumerable<Issue> CheckProperty(NoteInfo property)
    {
        switch (property.Columns)
        {
            case < MIN_COLUMNS:
                yield return new IssueTemplateColumnNotEnough(this).Create();

                break;

            case > MAX_COLUMNS:
                yield return new IssueTemplateColumnExceed(this).Create();

                break;
        }
    }

    protected override IEnumerable<Issue> CheckHitObject(NoteInfo property, Note hitObject)
    {
        if (hitObject.Tone < property.MinTone)
            yield return new IssueTemplateNoteToneTooLow(this).Create(hitObject);

        if (hitObject.Tone > property.MaxTone)
            yield return new IssueTemplateNoteToneTooHigh(this).Create(hitObject);
    }

    public class IssueTemplateColumnNotEnough : IssueTemplate
    {
        public IssueTemplateColumnNotEnough(ICheck check)
            : base(check, IssueType.Warning, "Column is not enough.")
        {
        }

        public Issue Create() => new(this);
    }

    public class IssueTemplateColumnExceed : IssueTemplate
    {
        public IssueTemplateColumnExceed(ICheck check)
            : base(check, IssueType.Warning, "Column exceed.")
        {
        }

        public Issue Create() => new(this);
    }

    public class IssueTemplateNoteToneTooLow : IssueTemplate
    {
        public IssueTemplateNoteToneTooLow(ICheck check)
            : base(check, IssueType.Warning, "Note's tone is too low.")
        {
        }

        public Issue Create(Note note) => new NoteIssue(note, this);
    }

    public class IssueTemplateNoteToneTooHigh : IssueTemplate
    {
        public IssueTemplateNoteToneTooHigh(ICheck check)
            : base(check, IssueType.Warning, "Note's tone is too high.")
        {
        }

        public Issue Create(Note note) => new NoteIssue(note, this);
    }
}
