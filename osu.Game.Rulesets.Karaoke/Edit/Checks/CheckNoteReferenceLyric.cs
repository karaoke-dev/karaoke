// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckNoteReferenceLyric : CheckHitObjectReferenceProperty<Note, Lyric>
    {
        protected override string Description => "Note with invalid reference lyric.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateNoteNullReferenceLyric(this),
            new IssueTemplateNoteInvalidReferenceLyric(this),
            new IssueTemplateNoteReferenceLyricHasLessThanTwoTimeTag(this),
            new IssueTemplateNoteMissingStartReferenceTimeTag(this),
            new IssueTemplateNoteStartReferenceTimeTagMissingTime(this),
            new IssueTemplateNoteMissingEndReferenceTimeTag(this),
            new IssueTemplateNoteEndReferenceTimeTagMissingTime(this),
        };

        protected override IEnumerable<Issue> CheckReferenceProperty(Note note, IEnumerable<Lyric> allAvailableReferencedHitObjects)
        {
            if (note.ReferenceLyric == null)
            {
                yield return new IssueTemplateNoteNullReferenceLyric(this).Create(note);

                yield break;
            }

            if (note.ReferenceLyric != null && !allAvailableReferencedHitObjects.Contains(note.ReferenceLyric))
                yield return new IssueTemplateNoteInvalidReferenceLyric(this).Create(note);

            if (note.ReferenceLyric?.TimeTags.Count < 2)
            {
                yield return new IssueTemplateNoteReferenceLyricHasLessThanTwoTimeTag(this).Create(note);

                yield break;
            }

            var startTimeTag = note.StartReferenceTimeTag;
            var endTimeTag = note.EndReferenceTimeTag;

            if (startTimeTag == null)
                yield return new IssueTemplateNoteMissingStartReferenceTimeTag(this).Create(note);

            if (startTimeTag != null && startTimeTag.Time == null)
                yield return new IssueTemplateNoteStartReferenceTimeTagMissingTime(this).Create(note);

            if (endTimeTag == null)
                yield return new IssueTemplateNoteMissingEndReferenceTimeTag(this).Create(note);

            if (endTimeTag != null && endTimeTag.Time == null)
                yield return new IssueTemplateNoteEndReferenceTimeTagMissingTime(this).Create(note);
        }

        public class IssueTemplateNoteNullReferenceLyric : IssueTemplate
        {
            public IssueTemplateNoteNullReferenceLyric(ICheck check)
                : base(check, IssueType.Error, "Note must have its parent lyric.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateNoteInvalidReferenceLyric : IssueTemplate
        {
            public IssueTemplateNoteInvalidReferenceLyric(ICheck check)
                : base(check, IssueType.Error, "Note's reference lyric must in the beatmap.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateNoteReferenceLyricHasLessThanTwoTimeTag : IssueTemplate
        {
            public IssueTemplateNoteReferenceLyricHasLessThanTwoTimeTag(ICheck check)
                : base(check, IssueType.Problem, "Note's reference lyric must have at least two time-tags.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateNoteMissingStartReferenceTimeTag : IssueTemplate
        {
            public IssueTemplateNoteMissingStartReferenceTimeTag(ICheck check)
                : base(check, IssueType.Problem, "Note's start reference time-tag is missing.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateNoteStartReferenceTimeTagMissingTime : IssueTemplate
        {
            public IssueTemplateNoteStartReferenceTimeTagMissingTime(ICheck check)
                : base(check, IssueType.Problem, "Note's start reference time-tag is found but missing time.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateNoteMissingEndReferenceTimeTag : IssueTemplate
        {
            public IssueTemplateNoteMissingEndReferenceTimeTag(ICheck check)
                : base(check, IssueType.Problem, "Note's end reference time-tag is missing.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateNoteEndReferenceTimeTagMissingTime : IssueTemplate
        {
            public IssueTemplateNoteEndReferenceTimeTagMissingTime(ICheck check)
                : base(check, IssueType.Problem, "Note's end reference time-tag is found but missing time.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }
    }
}
