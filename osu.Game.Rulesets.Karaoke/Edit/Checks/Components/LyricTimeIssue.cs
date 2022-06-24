// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks.Components
{
    public class LyricTimeIssue : Issue
    {
        public readonly TimeInvalid[] InvalidLyricTime;

        public LyricTimeIssue(HitObject lyric, IssueTemplate template, TimeInvalid[] invalidLyricTime, params object[] args)
            : base(lyric, template, args)
        {
            InvalidLyricTime = invalidLyricTime;
        }
    }

    public enum TimeInvalid
    {
        Overlapping,

        StartTimeInvalid,

        EndTimeInvalid
    }
}
