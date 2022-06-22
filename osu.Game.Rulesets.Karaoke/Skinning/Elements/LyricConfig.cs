// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning.Elements
{
    public class LyricConfig : IKaraokeSkinElement
    {
        public static LyricConfig CreateDefault() => new()
        {
            Name = "Default",
            SmartHorizon = KaraokeTextSmartHorizon.Multi,
            LyricsInterval = 4,
            RubyInterval = 2,
            RomajiInterval = 2,
            RubyAlignment = LyricTextAlignment.EqualSpace,
            RomajiAlignment = LyricTextAlignment.EqualSpace,
            RubyMargin = 4,
            RomajiMargin = 4,
            MainTextFont = new FontUsage("Torus", 48, "Bold"),
            RubyTextFont = new FontUsage("Torus", 20, "Bold"),
            RomajiTextFont = new FontUsage("Torus", 20, "Bold")
        };

        public int ID { get; set; }

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

        public void ApplyTo(Drawable d)
        {
            if (d is not DrawableLyric drawableLyric)
                throw new InvalidDrawableTypeException(nameof(d));

            drawableLyric.ApplyToLyricPieces(l =>
            {
                // Apply text font info
                l.Font = getFont(KaraokeRulesetSetting.MainFont, MainTextFont);
                l.RubyFont = getFont(KaraokeRulesetSetting.RubyFont, RubyTextFont);
                l.RomajiFont = getFont(KaraokeRulesetSetting.RomajiFont, RomajiTextFont);

                // Layout to text
                l.KaraokeTextSmartHorizon = SmartHorizon;
                l.Spacing = new Vector2(LyricsInterval, l.Spacing.Y);

                // Ruby
                l.RubySpacing = new Vector2(RubyInterval, l.RubySpacing.Y);
                l.RubyAlignment = RubyAlignment;
                l.RubyMargin = RubyMargin;

                // Romaji
                l.RomajiSpacing = new Vector2(RomajiInterval, l.RomajiSpacing.Y);
                l.RomajiAlignment = RomajiAlignment;
                l.RomajiMargin = RomajiMargin;
            });

            // Apply translate font.
            drawableLyric.ApplyToTranslateText(text =>
            {
                text.Font = getFont(KaraokeRulesetSetting.TranslateFont);
            });

            FontUsage getFont(KaraokeRulesetSetting setting, FontUsage? skinFont = null)
            {
                var config = drawableLyric.Dependencies.Get<KaraokeRulesetConfigManager>();
                var font = config?.Get<FontUsage>(setting) ?? FontUsage.Default;

                bool forceUseDefault = forceUseDefaultFont();
                if (forceUseDefault || skinFont == null)
                    return font;

                return font.With(size: skinFont.Value.Size);

                bool forceUseDefaultFont()
                {
                    switch (setting)
                    {
                        case KaraokeRulesetSetting.MainFont:
                        case KaraokeRulesetSetting.RubyFont:
                        case KaraokeRulesetSetting.RomajiFont:
                            return config?.Get<bool>(KaraokeRulesetSetting.ForceUseDefaultFont) ?? false;

                        case KaraokeRulesetSetting.TranslateFont:
                            return config?.Get<bool>(KaraokeRulesetSetting.ForceUseDefaultTranslateFont) ?? false;

                        default:
                            throw new InvalidOperationException(nameof(setting));
                    }
                }
            }
        }
    }
}
