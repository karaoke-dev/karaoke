// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts
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

        /// <summary>
        /// ???
        /// todo: should be moved into config.
        /// </summary>
        public KaraokeTextSmartHorizon SmartHorizon { get; set; } = KaraokeTextSmartHorizon.None;

        /// <summary>
        /// Interval between lyric texts
        /// todo: should be moved into config.
        /// </summary>
        public int LyricsInterval { get; set; }

        /// <summary>
        /// Interval between lyric rubies
        /// todo: should be moved into config.
        /// </summary>
        public int RubyInterval { get; set; }

        /// <summary>
        /// Interval between lyric romaji
        /// todo: should be moved into config.
        /// </summary>
        public int RomajiInterval { get; set; }

        /// <summary>
        /// Ruby position alignment
        /// todo: should be moved into config.
        /// </summary>
        public LyricTextAlignment RubyAlignment { get; set; } = LyricTextAlignment.Auto;

        /// <summary>
        /// Ruby position alignment
        /// todo: should be moved into config.
        /// </summary>
        public LyricTextAlignment RomajiAlignment { get; set; } = LyricTextAlignment.Auto;

        /// <summary>
        /// Interval between lyric text and ruby
        /// todo: should be moved into config.
        /// </summary>
        public int RubyMargin { get; set; }

        /// <summary>
        /// (Additional) Interval between lyric text and romaji
        /// todo: should be moved into config.
        /// </summary>
        public int RomajiMargin { get; set; }
    }
}
