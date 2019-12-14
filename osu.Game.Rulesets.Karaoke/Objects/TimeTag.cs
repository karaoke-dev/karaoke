// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class TimeTag
    {
        /// <summary>
        /// Time
        /// </summary>
        public double StartTime;

        /// <summary>
        /// Chected
        /// </summary>
        public bool Check;

        /// <summary>
        /// keyUp
        /// </summary>
        public bool KeyUp;

        public override string ToString() => $@"Time={StartTime},Check={Check},KeyUp={KeyUp}";
    }
}
