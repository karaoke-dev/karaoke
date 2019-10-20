// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Objects
{
    public class KaraokeLayout
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Lyric's aligment
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

        /// <summary>
        /// ???
        /// </summary>
        public SmartHorizon SmartHorizon { get; set; } = SmartHorizon.None;

        /// <summary>
        /// Interval between lyric texts
        /// </summary>
        public int LyricsInterval { get; set; }

        /// <summary>
        /// Interval between lyric rubies
        /// </summary>
        public int RubyInterval { get; set; }

        /// <summary>
        /// Interval between lyric romajies
        /// </summary>
        public int RomajiInterval { get; set; }

        /// <summary>
        /// Ruby position alignment
        /// </summary>
        public RubyAlignment RubyAlignment { get; set; } = RubyAlignment.Auto;

        /// <summary>
        /// Interval between lyric text and ruby
        /// </summary>
        public int RubyMargin { get; set; }

        /// <summary>
        /// (Additional) Interval between lyric text and romaji
        /// </summary>
        public int RomajiMargin { get; set; }
    }
}
