// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Metadatas
{
    public class LyricLayout
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Group
        /// </summary>
        public int Group { get; set; }

        /// <summary>
        /// Lyric alignment
        /// </summary>
        public Anchor Alignment { get; set; } = Anchor.Centre;

        /// <summary>
        /// Horizontal margin
        /// </summary>
        public int HorizontalMargin { get; set; }

        /// <summary>
        /// Vertical margin
        /// </summary>
        public int VerticalMargin { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public bool Continuous { get; set; }
    }
}
