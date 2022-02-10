// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class NicoKaraDecoderTest
    {
        public NicoKaraDecoderTest()
        {
            // It's a tricky to let osu! to read karaoke testing beatmap
            NicoKaraDecoder.Register();
        }

        [Test]
        public void TestDecodeNicoKara()
        {
            using (var resStream = TestResources.OpenNicoKaraResource("default"))
            using (var stream = new LineBufferedReader(resStream))
            {
                var decoder = Decoder.GetDecoder<NicoKaraSkin>(stream);
                var skin = decoder.Decode(stream);

                // Testing layout
                var firstLayout = skin.Layouts.FirstOrDefault();
                Assert.IsNotNull(firstLayout);
                Assert.AreEqual("下-1", firstLayout.Name);
                Assert.AreEqual(Anchor.BottomRight, firstLayout.Alignment);
                Assert.AreEqual(30, firstLayout.HorizontalMargin);
                Assert.AreEqual(45, firstLayout.VerticalMargin);

                // Testing style
                var firstFont = skin.LyricStyles.FirstOrDefault();
                Assert.IsNotNull(firstFont);
                Assert.AreEqual("標準配色", firstFont.Name);

                // Because some property has been converted into shader, so should test shader property.
                var shaders = firstFont.LeftLyricTextShaders;
                Assert.NotNull(shaders);

                // Test outline shader.
                var outlineShader = shaders.FirstOrDefault() as OutlineShader;
                Assert.NotNull(outlineShader);
                Assert.AreEqual(new Color4(255, 255, 255, 255), outlineShader.OutlineColour);
                Assert.AreEqual(10, outlineShader.Radius);

                // Test shader convert result.
                var shadowShader = shaders.LastOrDefault() as ShadowShader;
                Assert.NotNull(shadowShader);
                Assert.AreEqual(new Vector2(3), shadowShader.ShadowOffset);

                // test lyric config
                var defaultLyricConfig = skin.DefaultLyricConfig;
                Assert.NotNull(defaultLyricConfig);
                Assert.AreEqual(KaraokeTextSmartHorizon.Multi, defaultLyricConfig.SmartHorizon);
                Assert.AreEqual(4, defaultLyricConfig.LyricsInterval);
                Assert.AreEqual(2, defaultLyricConfig.RubyInterval);
                Assert.AreEqual(LyricTextAlignment.Auto, defaultLyricConfig.RubyAlignment);
                Assert.AreEqual(4, defaultLyricConfig.RubyMargin);

                // Test main text font
                var mainTextFontInfo = defaultLyricConfig.MainTextFont;
                Assert.AreEqual("游明朝 Demibold", mainTextFontInfo.Family);
                Assert.AreEqual("Bold", mainTextFontInfo.Weight);
                Assert.AreEqual(40, mainTextFontInfo.Size);
                Assert.AreEqual(false, mainTextFontInfo.FixedWidth);
            }
        }
    }
}
