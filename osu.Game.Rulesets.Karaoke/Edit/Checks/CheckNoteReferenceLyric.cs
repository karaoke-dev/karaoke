// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckNoteReferenceLyric : CheckHitObjectProperty<Note>
    {
        protected override string Description => "Note with invalid reference lyric.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateInvalidReferenceLyric(this),
        };

        protected override IEnumerable<Issue> Check(Note note)
        {
            yield break;
        }

        protected override IEnumerable<Issue> CheckAllHitObject(IReadOnlyList<HitObject> hitObjects)
        {
            var lyrics = hitObjects.OfType<Lyric>().ToArray();
            var notes = hitObjects.OfType<Note>().ToArray();

            foreach (var note in notes)
            {
                if (note.ReferenceLyric == null)
                    yield return new IssueTemplateNullReferenceLyric(this).Create(note);

                if (note.ReferenceLyric != null && !lyrics.Contains(note.ReferenceLyric))
                    yield return new IssueTemplateInvalidReferenceLyric(this).Create(note);
            }
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
    }
}
