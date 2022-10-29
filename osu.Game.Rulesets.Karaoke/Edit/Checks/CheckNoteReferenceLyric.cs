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
            new IssueTemplateNullReferenceLyric(this),
            new IssueTemplateInvalidReferenceLyric(this),
            new IssueTemplateReferenceLyricHasLessThanTwoTimeTag(this),
            new IssueTemplateMissingStartReferenceTimeTag(this),
            new IssueTemplateStartReferenceTimeTagMissingTime(this),
            new IssueTemplateMissingEndReferenceTimeTag(this),
            new IssueTemplateEndReferenceTimeTagMissingTime(this),
        };

        protected override IEnumerable<Issue> CheckReferenceProperty(Note note, IEnumerable<Lyric> allAvailableReferencedHitObjects)
        {
            if (note.ReferenceLyric == null)
            {
                yield return new IssueTemplateNullReferenceLyric(this).Create(note);

                yield break;
            }

            if (note.ReferenceLyric != null && !allAvailableReferencedHitObjects.Contains(note.ReferenceLyric))
                yield return new IssueTemplateInvalidReferenceLyric(this).Create(note);

            if (note.ReferenceLyric?.TimeTags.Count < 2)
            {
                yield return new IssueTemplateReferenceLyricHasLessThanTwoTimeTag(this).Create(note);

                yield break;
            }

            var startTimeTag = note.StartReferenceTimeTag;
            var endTimeTag = note.EndReferenceTimeTag;

            if (startTimeTag == null)
                yield return new IssueTemplateMissingStartReferenceTimeTag(this).Create(note);

            if (startTimeTag != null && startTimeTag.Time == null)
                yield return new IssueTemplateStartReferenceTimeTagMissingTime(this).Create(note);

            if (endTimeTag == null)
                yield return new IssueTemplateMissingEndReferenceTimeTag(this).Create(note);

            if (endTimeTag != null && endTimeTag.Time == null)
                yield return new IssueTemplateEndReferenceTimeTagMissingTime(this).Create(note);
        }

        public class IssueTemplateNullReferenceLyric : IssueTemplate
        {
            public IssueTemplateNullReferenceLyric(ICheck check)
                : base(check, IssueType.Problem, "Note must have its parent lyric.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateInvalidReferenceLyric : IssueTemplate
        {
            public IssueTemplateInvalidReferenceLyric(ICheck check)
                : base(check, IssueType.Problem, "Note's reference lyric must in the beatmap.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateReferenceLyricHasLessThanTwoTimeTag : IssueTemplate
        {
            public IssueTemplateReferenceLyricHasLessThanTwoTimeTag(ICheck check)
                : base(check, IssueType.Problem, "Note's reference lyric must have at least two time-tags.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateMissingStartReferenceTimeTag : IssueTemplate
        {
            public IssueTemplateMissingStartReferenceTimeTag(ICheck check)
                : base(check, IssueType.Problem, "Note's start reference time-tag is missing.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateStartReferenceTimeTagMissingTime : IssueTemplate
        {
            public IssueTemplateStartReferenceTimeTagMissingTime(ICheck check)
                : base(check, IssueType.Problem, "Note's start reference time-tag is found but missing time.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateMissingEndReferenceTimeTag : IssueTemplate
        {
            public IssueTemplateMissingEndReferenceTimeTag(ICheck check)
                : base(check, IssueType.Problem, "Note's end reference time-tag is missing.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }

        public class IssueTemplateEndReferenceTimeTagMissingTime : IssueTemplate
        {
            public IssueTemplateEndReferenceTimeTagMissingTime(ICheck check)
                : base(check, IssueType.Problem, "Note's end reference time-tag is found but missing time.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }
    }
}
