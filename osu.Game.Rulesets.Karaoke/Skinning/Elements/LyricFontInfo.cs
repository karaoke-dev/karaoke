// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Skinning.Elements;

public class LyricFontInfo : IKaraokeSkinElement
{
    public static LyricFontInfo CreateDefault() => new()
    {
        Name = "Default",
        SmartHorizon = KaraokeTextSmartHorizon.Multi,
        LyricsInterval = 4,
        RubyInterval = 2,
        RomanisationInterval = 2,
        RubyAlignment = LyricTextAlignment.EqualSpace,
        RomanisationAlignment = LyricTextAlignment.EqualSpace,
        RubyMargin = 4,
        RomanisationMargin = 4,
        MainTextFont = new FontUsage("Torus", 48, "Bold"),
        RubyTextFont = new FontUsage("Torus", 20, "Bold"),
        RomanisationTextFont = new FontUsage("Torus", 20, "Bold"),
    };

    public int ID { get; set; }

    public string Name { get; set; } = string.Empty;

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
    /// Interval between lyric romanisation
    /// </summary>
    public int RomanisationInterval { get; set; }

    /// <summary>
    /// Ruby position alignment
    /// </summary>
    public LyricTextAlignment RubyAlignment { get; set; } = LyricTextAlignment.Auto;

    /// <summary>
    /// Ruby position alignment
    /// </summary>
    public LyricTextAlignment RomanisationAlignment { get; set; } = LyricTextAlignment.Auto;

    /// <summary>
    /// Interval between lyric text and ruby
    /// </summary>
    public int RubyMargin { get; set; }

    /// <summary>
    /// (Additional) Interval between lyric text and romanisation.
    /// </summary>
    public int RomanisationMargin { get; set; }

    /// <summary>
    /// Main text font
    /// </summary>
    public FontUsage MainTextFont { get; set; } = new("Torus", 48, "Bold");

    /// <summary>
    /// Ruby text font
    /// </summary>
    public FontUsage RubyTextFont { get; set; } = new("Torus", 20, "Bold");

    /// <summary>
    /// Romanisation text font
    /// </summary>
    public FontUsage RomanisationTextFont { get; set; } = new("Torus", 20, "Bold");

    public void ApplyTo(Drawable d)
    {
        if (d is not DrawableLyric drawableLyric)
            throw new InvalidDrawableTypeException(nameof(d));

        drawableLyric.ApplyToLyricPieces(l =>
        {
            // Apply text font info
            l.Font = getFont(KaraokeRulesetSetting.MainFont, MainTextFont);
            l.TopTextFont = getFont(KaraokeRulesetSetting.RubyFont, RubyTextFont);
            l.BottomTextFont = getFont(KaraokeRulesetSetting.RomanisationFont, RomanisationTextFont);

            // Layout to text
            l.KaraokeTextSmartHorizon = SmartHorizon;
            l.Spacing = new Vector2(LyricsInterval, l.Spacing.Y);

            // Top text
            l.TopTextSpacing = new Vector2(RubyInterval, l.TopTextSpacing.Y);
            l.TopTextAlignment = RubyAlignment;
            l.TopTextMargin = RubyMargin;

            // Bottom text
            l.BottomTextSpacing = new Vector2(RomanisationInterval, l.BottomTextSpacing.Y);
            l.BottomTextAlignment = RomanisationAlignment;
            l.BottomTextMargin = RomanisationMargin;
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
                    case KaraokeRulesetSetting.RomanisationFont:
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
