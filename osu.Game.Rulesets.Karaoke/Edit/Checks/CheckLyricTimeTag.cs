// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckLyricTimeTag : CheckHitObjectProperty<Lyric>
    {
        protected override string Description => "Lyric with invalid time-tag.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateLyricTimeTagOutOfRange(this),
            new IssueTemplateLyricTimeTagOverlapping(this),
            new IssueTemplateLyricTimeTagEmptyTime(this),
        };

        public override IEnumerable<Issue> Check(Lyric lyric)
        {
            // todo: maybe config?
            const GroupCheck group_check = GroupCheck.Asc;
            const SelfCheck self_check = SelfCheck.BasedOnStart;

            var outOfRangeTags = TimeTagsUtils.FindOutOfRange(lyric.TimeTags, lyric.Text);
            var overlappingTimeTags = TimeTagsUtils.FindOverlapping(lyric.TimeTags, group_check, self_check).ToArray();
            var noTimeTimeTags = TimeTagsUtils.FindNoneTime(lyric.TimeTags);

            foreach (var textTag in outOfRangeTags)
            {
                yield return new IssueTemplateLyricTimeTagOutOfRange(this).Create(lyric, textTag);
            }

            foreach (var textTag in overlappingTimeTags)
            {
                yield return new IssueTemplateLyricTimeTagOverlapping(this).Create(lyric, textTag);
            }

            foreach (var textTag in noTimeTimeTags)
            {
                yield return new IssueTemplateLyricTimeTagEmptyTime(this).Create(lyric, textTag);
            }
        }

        public abstract class TimeTagIssueTemplate : IssueTemplate
        {
            protected TimeTagIssueTemplate(ICheck check, IssueType type, string unformattedMessage)
                : base(check, type, unformattedMessage)
            {
            }

            public Issue Create(Lyric lyric, TimeTag timeTag) => new TimeTagIssue(lyric, this, timeTag, timeTag);
        }

        public class IssueTemplateLyricTimeTagOutOfRange : TimeTagIssueTemplate
        {
            public IssueTemplateLyricTimeTagOutOfRange(ICheck check)
                : base(check, IssueType.Problem, "Time-tag index is out of range.")
            {
            }
        }

        public class IssueTemplateLyricTimeTagOverlapping : TimeTagIssueTemplate
        {
            public IssueTemplateLyricTimeTagOverlapping(ICheck check)
                : base(check, IssueType.Problem, "Time-tag index is overlapping to another time-tag.")
            {
            }
        }

        public class IssueTemplateLyricTimeTagEmptyTime : TimeTagIssueTemplate
        {
            public IssueTemplateLyricTimeTagEmptyTime(ICheck check)
                : base(check, IssueType.Problem, "Time-tag has no time.")
            {
            }
        }
    }
}
