// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Objects.Types;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class Title : KaraokeHitObject, IHasEndTime
    {
        public string Name { get; set; }

        public int KaraokeLayoutIndex { get; set; }

        public double Duration { get; set; }

        public double EndTime { get; set; }

        public int LineInterval { get; set; }

        public bool ShowRuby { get; set; }

        public List<TitlePart> TitleParts { get; set; }
    }
}
