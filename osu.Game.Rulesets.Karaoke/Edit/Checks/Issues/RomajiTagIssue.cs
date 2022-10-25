// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks.Issues
{
    public class RomajiTagIssue : Issue
    {
        public readonly RomajiTag RomajiTag;

        public RomajiTagIssue(HitObject lyric, IssueTemplate template, RomajiTag romajiTag, params object[] args)
            : base(lyric, template, args)
        {
            RomajiTag = romajiTag;
        }
    }
}
