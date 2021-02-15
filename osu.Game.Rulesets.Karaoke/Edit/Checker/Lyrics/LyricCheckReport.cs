// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics
{
    public struct LyricCheckReport
    {
        public bool TimeInvalid { get; set; }

        public TimeTag[] InvalidTimeTag { get; set; }

        public RubyTag[] InvalidRubyTag { get; set; }

        public RomajiTag[] InvalidRomajiTag { get; set; }

        public bool IsValid
        {
            get
            {
                if (TimeInvalid)
                    return false;

                if (InvalidTimeTag?.Length > 0)
                    return false;

                if (InvalidRubyTag?.Length > 0)
                    return false;

                if (InvalidRomajiTag?.Length > 0)
                    return false;

                return true;
            }
        }
    }
}
