// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Skinning.Metadatas
{
    public class LyricConfig
    {
        public string Name { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public KaraokeTextSmartHorizon SmartHorizon { get; set; } = KaraokeTextSmartHorizon.None;

        /// <summary>
        /// Interval between lyric texts
        /// </summary>
        public int LyricsInterval { get; set; }

        /// <summary>
        /// Interval between lyric rubies
        /// </summary>
        public int RubyInterval { get; set; }

        /// <summary>
        /// Interval between lyric romaji
        /// </summary>
        public int RomajiInterval { get; set; }

        /// <summary>
        /// Ruby position alignment
        /// </summary>
        public LyricTextAlignment RubyAlignment { get; set; } = LyricTextAlignment.Auto;

        /// <summary>
        /// Ruby position alignment
        /// </summary>
        public LyricTextAlignment RomajiAlignment { get; set; } = LyricTextAlignment.Auto;

        /// <summary>
        /// Interval between lyric text and ruby
        /// </summary>
        public int RubyMargin { get; set; }

        /// <summary>
        /// (Additional) Interval between lyric text and romaji
        /// </summary>
        public int RomajiMargin { get; set; }

        /// <summary>
        /// Main text font
        /// </summary>
        public FontUsage MainTextFont { get; set; } = new("Torus", 48, "Bold");

        /// <summary>
        /// Ruby text font
        /// </summary>
        public FontUsage RubyTextFont { get; set; } = new("Torus", 20, "Bold");

        /// <summary>
        /// Romaji text font
        /// </summary>
        public FontUsage RomajiTextFont { get; set; } = new("Torus", 20, "Bold");
    }
}
