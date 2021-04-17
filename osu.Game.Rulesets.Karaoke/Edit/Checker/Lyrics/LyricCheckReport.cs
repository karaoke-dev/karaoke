﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics
{
    public class LyricCheckReport
    {
        public LyricCheckReport(Lyric lyric)
        {
            CheckObject = lyric;
        }

        public Lyric CheckObject { get; }

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
}
