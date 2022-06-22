// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckInvalidPropertyNotes : ICheck
    {
        public CheckMetadata Metadata => new(CheckCategory.HitObjects, "Notes with invalid property.");

        public IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateInvalidParentLyric(this),
        };

        public IEnumerable<Issue> Run(BeatmapVerifierContext context)
        {
            var lyrics = context.Beatmap.HitObjects.OfType<Lyric>().ToArray();

            foreach (var note in context.Beatmap.HitObjects.OfType<Note>())
            {
                if (note.ParentLyric == null || !lyrics.Contains(note.ParentLyric))
                    yield return new IssueTemplateInvalidParentLyric(this).Create(note);
            }
        }

        public class IssueTemplateInvalidParentLyric : IssueTemplate
        {
            public IssueTemplateInvalidParentLyric(ICheck check)
                : base(check, IssueType.Problem, "Note must have its parent lyric. If see this issue, please contact to ruleset developer.")
            {
            }

            public Issue Create(Note note)
                => new(note, this);
        }
    }
}
