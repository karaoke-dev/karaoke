// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks.Components
{
    public class TimeTagIssue : Issue
    {
        public readonly Dictionary<TimeTagInvalid, TimeTag[]> InvalidTimeTags;

        public TimeTagIssue(Lyric layic, IssueTemplate template, Dictionary<TimeTagInvalid, TimeTag[]> invalidTimeTags, params object[] args)
            : base(layic, template, args)
        {
            InvalidTimeTags = invalidTimeTags;
        }
    }
}
