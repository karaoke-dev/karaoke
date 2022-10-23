// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckLyricSinger : ICheck
    {
        public CheckMetadata Metadata => new(CheckCategory.HitObjects, "Lyrics with invalid property.");

        public IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateNoSinger(this),
        };

        public IEnumerable<Issue> Run(BeatmapVerifierContext context)
        {
            foreach (var lyric in context.Beatmap.HitObjects.OfType<Lyric>())
            {
                if (!lyric.Singers.Any())
                    yield return new IssueTemplateNoSinger(this).Create(lyric);
            }
        }

        public class IssueTemplateNoSinger : IssueTemplate
        {
            public IssueTemplateNoSinger(ICheck check)
                : base(check, IssueType.Problem, "Lyric must have at least one singer.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }
    }
}
