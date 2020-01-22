// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public struct RubyTag : ITag
    {
        /// <summary>
        /// If kanji Matched, then apply ruby
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Start index
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// End index
        /// </summary>
        public int EndIndex { get; set; }
    }
}
