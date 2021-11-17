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
                Assert.AreEqual(firstLayout.Name, "下-1");
                Assert.AreEqual(firstLayout.Alignment, Anchor.BottomRight);
                Assert.AreEqual(firstLayout.HorizontalMargin, 30);
                Assert.AreEqual(firstLayout.VerticalMargin, 45);
                Assert.AreEqual(firstLayout.Continuous, false);

                // Testing style
                var firstFont = skin.Styles.FirstOrDefault();
                Assert.IsNotNull(firstFont);
                Assert.AreEqual(firstFont.Name, "標準配色");

                // Because some property has been converted into shader, so should test shader property.
                var shaders = firstFont.LeftLyricTextShaders;
                Assert.NotNull(shaders);

                // Test outline shader.
                var outlineShader = shaders.FirstOrDefault() as OutlineShader;
                Assert.NotNull(outlineShader);
                Assert.AreEqual(outlineShader.OutlineColour, new Color4(255, 255, 255, 255));
                Assert.AreEqual(outlineShader.Radius, 10);

                // Test shader convert result.
                var shadowShader = shaders.LastOrDefault() as ShadowShader;
                Assert.NotNull(shadowShader);
                Assert.AreEqual(shadowShader.ShadowOffset, new Vector2(3));

                // test lyric config
                var defaultLyricConfig = skin.DefaultLyricConfig;
                Assert.NotNull(defaultLyricConfig);
                Assert.AreEqual(defaultLyricConfig.SmartHorizon, KaraokeTextSmartHorizon.Multi);
                Assert.AreEqual(defaultLyricConfig.LyricsInterval, 4);
                Assert.AreEqual(defaultLyricConfig.RubyInterval, 2);
                Assert.AreEqual(defaultLyricConfig.RubyAlignment, LyricTextAlignment.Auto);
                Assert.AreEqual(defaultLyricConfig.RubyMargin, 4);

                // Test main text font
                var mainTextFontInfo = defaultLyricConfig.MainTextFont;
                Assert.AreEqual(mainTextFontInfo.Family, "游明朝 Demibold");
                Assert.AreEqual(mainTextFontInfo.Weight, "Bold");
                Assert.AreEqual(mainTextFontInfo.Size, 40);
                Assert.AreEqual(mainTextFontInfo.FixedWidth, false);
            }
        }
    }
}
