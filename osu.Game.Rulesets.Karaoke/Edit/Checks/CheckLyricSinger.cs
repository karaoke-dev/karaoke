// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckLyricSinger : CheckHitObjectProperty<Lyric>
    {
        protected override string Description => "Lyric with invalid singer.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateLyricNoSinger(this),
        };

        protected override IEnumerable<Issue> Check(Lyric lyric)
        {
            if (!lyric.Singers.Any())
                yield return new IssueTemplateLyricNoSinger(this).Create(lyric);
        }

        public class IssueTemplateLyricNoSinger : IssueTemplate
        {
            public IssueTemplateLyricNoSinger(ICheck check)
                : base(check, IssueType.Problem, "Lyric must have at least one singer.")
            {
            }

            public Issue Create(Lyric lyric)
                => new(lyric, this);
        }
    }
}
