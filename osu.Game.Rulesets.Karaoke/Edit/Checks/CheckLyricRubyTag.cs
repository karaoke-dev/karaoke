// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class CheckLyricRubyTag : CheckLyricTextTag<RubyTag>
    {
        protected override string Description => "Lyric with invalid ruby tag.";

        public override IEnumerable<IssueTemplate> PossibleTemplates => new IssueTemplate[]
        {
            new IssueTemplateLyricRubyOutOfRange(this),
            new IssueTemplateLyricRubyOverlapping(this),
            new IssueTemplateLyricRubyEmptyText(this),
        };

        protected override IList<RubyTag> GetTextTag(Lyric lyric)
            => lyric.RubyTags;

        protected override TextTagsUtils.Sorting Sorting => TextTagsUtils.Sorting.Asc;

        protected override Issue GetOutOfRangeIssue(Lyric lyric, RubyTag textTag)
            => new IssueTemplateLyricRubyOutOfRange(this).Create(lyric, textTag);

        protected override Issue GetOverlappingIssue(Lyric lyric, RubyTag textTag)
            => new IssueTemplateLyricRubyOverlapping(this).Create(lyric, textTag);

        protected override Issue GetEmptyTextIssue(Lyric lyric, RubyTag textTag)
            => new IssueTemplateLyricRubyEmptyText(this).Create(lyric, textTag);

        public abstract class RubyTagIssueTemplate : TextTagIssueTemplate
        {
            protected RubyTagIssueTemplate(ICheck check, IssueType type, string unformattedMessage)
                : base(check, type, unformattedMessage)
            {
            }

            public Issue Create(Lyric lyric, RubyTag textTag) => new RubyTagIssue(lyric, this, textTag, textTag);
        }

        public class IssueTemplateLyricRubyOutOfRange : RubyTagIssueTemplate
        {
            public IssueTemplateLyricRubyOutOfRange(ICheck check)
                : base(check, IssueType.Problem, "Ruby tag index is out of range.")
            {
            }
        }

        public class IssueTemplateLyricRubyOverlapping : RubyTagIssueTemplate
        {
            public IssueTemplateLyricRubyOverlapping(ICheck check)
                : base(check, IssueType.Problem, "Ruby tag index is overlapping to another ruby.")
            {
            }
        }

        public class IssueTemplateLyricRubyEmptyText : RubyTagIssueTemplate
        {
            public IssueTemplateLyricRubyEmptyText(ICheck check)
                : base(check, IssueType.Problem, "Ruby tag has no text.")
            {
            }
        }
    }
}
