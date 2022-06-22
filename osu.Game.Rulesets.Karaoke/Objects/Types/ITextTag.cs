// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

namespace osu.Game.Rulesets.Karaoke.Objects.Types
{
    public interface ITextTag : IHasText
    {
        /// <summary>
        /// Start index.
        /// Notice that this index means gap index between characters.
        /// </summary>
        int StartIndex { get; set; }

        /// <summary>
        /// End index
        /// Notice this this index means gap index between characters.
        /// </summary>
        int EndIndex { get; set; }
    }
}
