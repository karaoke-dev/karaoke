// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NicoKaraParser;
using NicoKaraParser.Model;
using NicoKaraParser.Model.Font;
using NicoKaraParser.Model.Font.Brush;
using NicoKaraParser.Model.Font.Font;
using NicoKaraParser.Model.Font.Shadow;
using NicoKaraParser.Model.Layout;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;
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
                output.Layouts.Add(new LyricLayout
                {
                    Name = karaokeLayout.Name,
                    Alignment = convertAnchor(karaokeLayout.HorizontalAlignment, karaokeLayout.VerticalAlignment),
                    HorizontalMargin = karaokeLayout.HorizontalMargin,
                    VerticalMargin = karaokeLayout.VerticalMargin,
                    Continuous = karaokeLayout.Continuous,
                });
            }

            // Clean-up style
            output.Styles = new List<LyricStyle>();

            foreach (var nicoKaraFont in nicoKaraProject.KaraokeFonts)
            {
                output.Styles.Add(new LyricStyle
                {
                    Name = nicoKaraFont.Name,
                    LeftLyricTextShaders = createShaders(nicoKaraFont, ApplyShaderPart.Left),
                    RightLyricTextShaders = createShaders(nicoKaraFont, ApplyShaderPart.Right),
                });
            }

            // assign default lyric config
            var firstLayout = nicoKaraProject.KaraokeLayouts?.FirstOrDefault();
            var firstFont = nicoKaraProject.KaraokeFonts?.FirstOrDefault();

            if (firstLayout == null || firstFont == null)
                return;

            Enum.TryParse(firstLayout.SmartHorizon.ToString(), out KaraokeTextSmartHorizon smartHorizon);
            Enum.TryParse(firstLayout.RubyAlignment.ToString(), out LyricTextAlignment rubyAlignment);

            output.DefaultLyricConfig = new LyricConfig
            {
                SmartHorizon = smartHorizon,
                LyricsInterval = firstLayout.LyricsInterval,
                RubyInterval = firstLayout.RubyInterval,
                RubyAlignment = rubyAlignment,
                RubyMargin = firstLayout.RubyMargin,
                MainTextFont = convertFontInfo(firstFont.FontInfos[0]),
                RubyTextFont = convertFontInfo(firstFont.FontInfos[3]),
                RomajiTextFont = convertFontInfo(firstFont.FontInfos[3]),
            };
        }

        private static Anchor convertAnchor(HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            Enum.TryParse((1 << ((int)horizontalAlignment + 3)).ToString(), out Anchor horizontalAnchor);
            Enum.TryParse((1 << (int)verticalAlignment).ToString(), out Anchor verticalAnchor);

            return horizontalAnchor | verticalAnchor;
        }

        private static List<ICustomizedShader> createShaders(KaraokeFont font, ApplyShaderPart part)
        {
            var fontInfo = font.FontInfos[0];
            var brushInfos = getBrusnInfos(font, part);
            var fontBrushInfo = brushInfos[0];
            var borderBrushInfo = brushInfos[1];
            var shaderBrushInfo = brushInfos[2];

            var shaders = new List<ICustomizedShader>();

            // todo: implement change font color.
            shaders.Add(createOutlineShader(borderBrushInfo, fontInfo));

            var hasShadow = font.UseShadow;

            if (hasShadow)
            {
                shaders.Add(createShadowShader(shaderBrushInfo, font.ShadowSlide));
            }

            return shaders;
        }

        private static BrushInfo[] getBrusnInfos(KaraokeFont font, ApplyShaderPart part) =>
            part switch
            {
                ApplyShaderPart.Left => font.BrushInfos.GetRange(0, 3).ToArray(),
                ApplyShaderPart.Right => font.BrushInfos.GetRange(3, 3).ToArray(),
                _ => throw new ArgumentOutOfRangeException(nameof(part))
            };

        private static OutlineShader createOutlineShader(BrushInfo info, FontInfo fontInfo)
        {
            var color = convertColor(info);
            var radius = convertEdgeSize(fontInfo);

            return new OutlineShader
            {
                OutlineColour = color,
                Radius = (int)radius,
            };

            static float convertEdgeSize(FontInfo info) => info.EdgeSize;
        }

        private static ShadowShader createShadowShader(BrushInfo info, ShadowSlide slide)
        {
            var color = convertColor(info);
            var shadowOffset = convertShadowSlide(slide);

            return new ShadowShader
            {
                ShadowColour = color,
                ShadowOffset = shadowOffset,
            };

            static Vector2 convertShadowSlide(ShadowSlide side) => new(side.X, side.Y);
        }

        private static Color4 convertColor(BrushInfo info)
        {
            // todo: we only support pure colour conversion.
            return convertSolidColor(info.SolidColor);
            /*
            Enum.TryParse(info.Type.ToString(), out BrushType type);

            // Convert BrushGradient
            var brushGradient = info.GradientPositions.Select((t, i) => new BrushInfo.BrushGradient { XPosition = t, Color = convertColor(info.GradientColors[i]) }).ToList();

            return new BrushInfo
            {
                Type = type,
                SolidColor = convertColor(info.SolidColor),
                BrushGradients = brushGradient
            };
            */

            static Color4 convertSolidColor(Color color) => new(color.R, color.G, color.B, color.A);
        }

        private static FontUsage convertFontInfo(FontInfo info)
        {
            var family = info.FontName;
            var size = Math.Max(info.CharSize, 8);
            var weight = info.FontStyle == FontStyle.Regular ? "Regular" : "Bold";
            return new FontUsage(family, size, weight);
        }

        private enum ApplyShaderPart
        {
            Left,

            Right
        }
    }
}
