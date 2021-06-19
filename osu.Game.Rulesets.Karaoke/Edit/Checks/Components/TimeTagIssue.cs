// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks.Components
{
    public class TimeTagIssue : Issue
    {
        public readonly Dictionary<TimeTagInvalid, TimeTag[]> InvalidTimeTags;

        public readonly bool MissingStartTimeTag;

        public readonly bool MissingEndTimeTag;

        public TimeTagIssue(HitObject lyric, IssueTemplate template, Dictionary<TimeTagInvalid, TimeTag[]> invalidTimeTags, bool missingStartTimeTag, bool missingEndTimeTag, params object[] args)
            : base(lyric, template, args)
        {
            InvalidTimeTags = invalidTimeTags;
            MissingStartTimeTag = missingStartTimeTag;
            MissingEndTimeTag = missingEndTimeTag;
        }
    }

    public enum TimeTagInvalid
    {
        OutOfRange,

        Overlapping,

        EmptyTime,
    }
}
