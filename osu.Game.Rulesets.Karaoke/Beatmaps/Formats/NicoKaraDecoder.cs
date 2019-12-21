// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using NicoKaraParser;
using NicoKaraParser.Model;
using NicoKaraParser.Model.Font.Font;
using NicoKaraParser.Model.Font.Shadow;
using NicoKaraParser.Model.Layout;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osuTK;
using osuTK.Graphics;
using BrushInfo = NicoKaraParser.Model.Font.Brush.BrushInfo;
using FontInfo = NicoKaraParser.Model.Font.Font.FontInfo;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class NicoKaraDecoder : Decoder<KaroakeSkin>
    {
        public static void Register()
        {
            AddDecoder<KaroakeSkin>("<?xml version=", m => new NicoKaraDecoder());
        }

        protected override void ParseStreamInto(LineBufferedReader stream, KaroakeSkin output)
        {
            Project nicoKaraProject;

            using (TextReader sr = new StringReader(stream.ReadToEnd()))
            {
                nicoKaraProject = new Parser().Deserialize(sr);
            }

            // Clean-up layour
            output.DefinedLayouts = new List<Skinning.Components.KaraokeLayout>();

            foreach (var nicoKaraLayour in nicoKaraProject.KaraokeLayouts)
            {
                Enum.TryParse(nicoKaraLayour.SmartHorizon.ToString(), out KaraokeTextSmartHorizon smartHorizon);
                Enum.TryParse(nicoKaraLayour.RubyAlignment.ToString(), out LyricTextAlignment rubyAlignment);

                output.DefinedLayouts.Add(new Skinning.Components.KaraokeLayout
                {
                    Name = nicoKaraLayour.Name,
                    Alignment = convertAnchor(nicoKaraLayour.HorizontalAlignment, nicoKaraLayour.VerticalAlignment),
                    HorizontalMargin = nicoKaraLayour.HorizontalMargin,
                    VerticalMargin = nicoKaraLayour.VerticalMargin,
                    Continuous = nicoKaraLayour.Continuous,
                    SmartHorizon = smartHorizon,
                    LyricsInterval = nicoKaraLayour.LyricsInterval,
                    RubyInterval = nicoKaraLayour.RubyInterval,
                    RubyAlignment = rubyAlignment,
                    RubyMargin = nicoKaraLayour.RubyMargin
                });
            }

            // Clean-up style
            output.DefinedFonts = new List<KaraokeFont>();

            foreach (var nicoKaraFont in nicoKaraProject.KaraokeFonts)
            {
                output.DefinedFonts.Add(new KaraokeFont
                {
                    Name = nicoKaraFont.Name,
                    UseShadow = nicoKaraFont.UseShadow,
                    ShadowOffset = convertShadowSlide(nicoKaraFont.ShadowSlide),
                    FrontTextBrushInfo = new KaraokeFont.TextBrushInfo
                    {
                        TextBrush = convertBrushInfo(nicoKaraFont.BrushInfos[0]),
                        BorderBrush = convertBrushInfo(nicoKaraFont.BrushInfos[1]),
                        ShadowBrush = convertBrushInfo(nicoKaraFont.BrushInfos[2]),
                    },
                    BackTextBrushInfo = new KaraokeFont.TextBrushInfo
                    {
                        TextBrush = convertBrushInfo(nicoKaraFont.BrushInfos[3]),
                        BorderBrush = convertBrushInfo(nicoKaraFont.BrushInfos[4]),
                        ShadowBrush = convertBrushInfo(nicoKaraFont.BrushInfos[5]),
                    },
                    LyricTextFontInfo = new KaraokeFont.TextFontInfo
                    {
                        LyricTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[0]),
                        NakaTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[1]),
                        EnTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[2]),
                    },
                    RubyTextFontInfo = new KaraokeFont.TextFontInfo
                    {
                        LyricTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[3]),
                        NakaTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[4]),
                        EnTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[5]),
                    },
                    RomajiTextFontInfo = new KaraokeFont.TextFontInfo
                    {
                        // Just copied from ruby setting
                        LyricTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[3]),
                        NakaTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[4]),
                        EnTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[5]),
                    }
                });
            }

            Vector2 convertShadowSlide(ShadowSlide side)
            {
                return new Vector2(side.X, side.Y);
            }

            Anchor convertAnchor(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
            {
                Enum.TryParse((1 << ((int)horizontalAlignment + 3)).ToString(), out Anchor horizontalAnchor);
                Enum.TryParse((1 << (int)verticalAlignment).ToString(), out Anchor verticalAnchor);

                return horizontalAnchor | verticalAnchor;
            }

            Skinning.Components.FontInfo convertFontInfo(FontInfo info)
            {
                return new Skinning.Components.FontInfo
                {
                    FontName = info.FontName,
                    Bold = info.FontStyle == FontStyle.Bold,
                    CharSize = info.CharSize,
                    EdgeSize = info.EdgeSize
                };
            }

            Skinning.Components.BrushInfo convertBrushInfo(BrushInfo info)
            {
                Enum.TryParse(info.Type.ToString(), out BrushType type);

                // Convert BrushGradient
                List<Skinning.Components.BrushInfo.BrushGradient> brushGradient = new List<Skinning.Components.BrushInfo.BrushGradient>();

                for (int i = 0; i < info.GradientPositions.Count; i++)
                {
                    brushGradient.Add(new Skinning.Components.BrushInfo.BrushGradient
                    {
                        XPosition = info.GradientPositions[i],
                        Color = convertColor(info.GradientColors[i])
                    });
                }

                return new Skinning.Components.BrushInfo
                {
                    Type = type,
                    SolidColor = convertColor(info.SolidColor),
                    BrushGradients = brushGradient
                };

                Color4 convertColor(System.Drawing.Color color)
                {
                    return new Color4(color.R, color.G, color.B, color.A);
                }
            }
        }
    }
}
