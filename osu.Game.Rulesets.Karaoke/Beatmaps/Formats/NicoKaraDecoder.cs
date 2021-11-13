// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NicoKaraParser;
using NicoKaraParser.Model;
using NicoKaraParser.Model.Font.Font;
using NicoKaraParser.Model.Font.Shadow;
using NicoKaraParser.Model.Layout;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Layouts;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class NicoKaraDecoder : Decoder<NicoKaraSkin>
    {
        public static void Register()
        {
            AddDecoder<NicoKaraSkin>("<?xml version=", _ => new NicoKaraDecoder());
        }

        protected override void ParseStreamInto(LineBufferedReader stream, NicoKaraSkin output)
        {
            Project nicoKaraProject;

            using (TextReader sr = new StringReader(stream.ReadToEnd()))
            {
                nicoKaraProject = new Parser().Deserialize(sr);
            }

            // Clean-up layout
            output.Layouts = new List<LyricLayout>();

            foreach (var karaokeLayout in nicoKaraProject.KaraokeLayouts)
            {
                Enum.TryParse(karaokeLayout.SmartHorizon.ToString(), out KaraokeTextSmartHorizon smartHorizon);
                Enum.TryParse(karaokeLayout.RubyAlignment.ToString(), out LyricTextAlignment rubyAlignment);

                output.Layouts.Add(new LyricLayout
                {
                    Name = karaokeLayout.Name,
                    Alignment = convertAnchor(karaokeLayout.HorizontalAlignment, karaokeLayout.VerticalAlignment),
                    HorizontalMargin = karaokeLayout.HorizontalMargin,
                    VerticalMargin = karaokeLayout.VerticalMargin,
                    Continuous = karaokeLayout.Continuous,
                    SmartHorizon = smartHorizon,
                    LyricsInterval = karaokeLayout.LyricsInterval,
                    RubyInterval = karaokeLayout.RubyInterval,
                    RubyAlignment = rubyAlignment,
                    RubyMargin = karaokeLayout.RubyMargin
                });
            }

            // Clean-up style
            output.Fonts = new List<LyricFont>();

            foreach (var nicoKaraFont in nicoKaraProject.KaraokeFonts)
            {
                output.Fonts.Add(new LyricFont
                {
                    Name = nicoKaraFont.Name,
                    UseShadow = nicoKaraFont.UseShadow,
                    ShadowOffset = convertShadowSlide(nicoKaraFont.ShadowSlide),
                    FrontTextBrushInfo = new LyricFont.TextBrushInfo
                    {
                        TextBrush = convertBrushInfo(nicoKaraFont.BrushInfos[0]),
                        BorderBrush = convertBrushInfo(nicoKaraFont.BrushInfos[1]),
                        ShadowBrush = convertBrushInfo(nicoKaraFont.BrushInfos[2]),
                    },
                    BackTextBrushInfo = new LyricFont.TextBrushInfo
                    {
                        TextBrush = convertBrushInfo(nicoKaraFont.BrushInfos[3]),
                        BorderBrush = convertBrushInfo(nicoKaraFont.BrushInfos[4]),
                        ShadowBrush = convertBrushInfo(nicoKaraFont.BrushInfos[5]),
                    },
                    LyricTextFontInfo = new LyricFont.TextFontInfo
                    {
                        LyricTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[0]),
                        NakaTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[1]),
                        EnTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[2]),
                        EdgeSize = convertEdgeSize(nicoKaraFont.FontInfos[0]),
                    },
                    RubyTextFontInfo = new LyricFont.TextFontInfo
                    {
                        LyricTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[3]),
                        NakaTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[4]),
                        EnTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[5]),
                        EdgeSize = convertEdgeSize(nicoKaraFont.FontInfos[3]),
                    },
                    RomajiTextFontInfo = new LyricFont.TextFontInfo
                    {
                        // Just copied from ruby setting
                        LyricTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[3]),
                        NakaTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[4]),
                        EnTextFontInfo = convertFontInfo(nicoKaraFont.FontInfos[5]),
                        EdgeSize = convertEdgeSize(nicoKaraFont.FontInfos[3]),
                    }
                });
            }

            static Vector2 convertShadowSlide(ShadowSlide side) => new(side.X, side.Y);

            static Anchor convertAnchor(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
            {
                Enum.TryParse((1 << ((int)horizontalAlignment + 3)).ToString(), out Anchor horizontalAnchor);
                Enum.TryParse((1 << (int)verticalAlignment).ToString(), out Anchor verticalAnchor);

                return horizontalAnchor | verticalAnchor;
            }

            static BrushInfo convertBrushInfo(NicoKaraParser.Model.Font.Brush.BrushInfo info)
            {
                Enum.TryParse(info.Type.ToString(), out BrushType type);

                // Convert BrushGradient
                var brushGradient = info.GradientPositions.Select((t, i) => new BrushInfo.BrushGradient { XPosition = t, Color = convertColor(info.GradientColors[i]) }).ToList();

                return new BrushInfo
                {
                    Type = type,
                    SolidColor = convertColor(info.SolidColor),
                    BrushGradients = brushGradient
                };

                static Color4 convertColor(Color color) => new(color.R, color.G, color.B, color.A);
            }

            static FontUsage convertFontInfo(FontInfo info)
            {
                var family = info.FontName;
                var size = Math.Max(info.CharSize, 8);
                var weight = info.FontStyle == FontStyle.Regular ? "Regular" : "Bold";
                return new FontUsage(family, size, weight);
            }

            static float convertEdgeSize(FontInfo info)
                => info.EdgeSize;
        }
    }
}
