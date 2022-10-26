// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public abstract class CheckLyricTextTag<TTextTag> : CheckHitObjectProperty<Lyric> where TTextTag : ITextTag
    {
        protected override IEnumerable<Issue> Check(Lyric lyric)
        {
            string text = lyric.Text;
            var textTags = GetTextTag(lyric);

            var outOfRangeTags = TextTagsUtils.FindOutOfRange(textTags, text);
            var overlappingTags = TextTagsUtils.FindOverlapping(textTags, Sorting);
            var emptyTextTags = TextTagsUtils.FindEmptyText(textTags);

            foreach (var textTag in outOfRangeTags)
            {
                yield return GetOutOfRangeIssue(lyric, textTag);
            }

            foreach (var textTag in overlappingTags)
            {
                yield return GetOverlappingIssue(lyric, textTag);
            }

            foreach (var textTag in emptyTextTags)
            {
                yield return GetEmptyTextIssue(lyric, textTag);
            }
        }

        protected abstract IList<TTextTag> GetTextTag(Lyric lyric);

        protected abstract TextTagsUtils.Sorting Sorting { get; }

        protected abstract Issue GetOutOfRangeIssue(Lyric lyric, TTextTag textTag);

        protected abstract Issue GetOverlappingIssue(Lyric lyric, TTextTag textTag);

        protected abstract Issue GetEmptyTextIssue(Lyric lyric, TTextTag textTag);

        public abstract class TextTagIssueTemplate : IssueTemplate
        {
            protected TextTagIssueTemplate(ICheck check, IssueType type, string unformattedMessage)
                : base(check, type, unformattedMessage)
            {
            }
        }
    }
}
