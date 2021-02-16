// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics
{
    public class LyricCheckReport
    {
        public TimeInvalid[] TimeInvalid { get; set; }

        public Dictionary<TimeTagInvalid, TimeTag[]> InvalidTimeTags { get; set; }

        public Dictionary<RubyTagInvalid, RubyTag[]> InvalidRubyTags { get; set; }

        public Dictionary<RomajiTagInvalid, RomajiTag[]> InvalidRomajiTags { get; set; }

        public bool IsValid
        {
            get
            {
                if (TimeInvalid?.Length > 0)
                    return false;

                if (InvalidTimeTags?.Count > 0)
                    return false;

                if (InvalidRubyTags?.Count > 0)
                    return false;

                if (InvalidRomajiTags?.Count > 0)
                    return false;

                return true;
            }
        }
    }

    public enum TimeInvalid
    {
        Overlapping,

        StartTimeInvalid,

        EndTimeInvalid
    }

    public enum TimeTagInvalid
    {
        OutOfRange,

        Overlapping,
    }

    public enum RubyTagInvalid
    {
        OutOfRange,

        Overlapping
    }

    public enum RomajiTagInvalid
    {
        OutOfRange,

        Overlapping
    }
}
