// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class NicoKaraDecoderTest
    {
        public NicoKaraDecoderTest()
        {
            // a trick to get osu! to register karaoke beatmaps
            NicoKaraDecoder.Register();
        }

        [Test]
        public void TestDecodeNicoKara()
        {
            using (var resStream = TestResources.OpenNicoKaraResource("default"))
            using (var stream = new LineBufferedReader(resStream))
            {
                var decoder = Decoder.GetDecoder<KaraokeSkin>(stream);
                var skin = decoder.Decode(stream);

                // Testing layout
                var firstLayout = skin.Layouts.FirstOrDefault();
                Assert.IsNotNull(firstLayout);
                Assert.AreEqual(firstLayout.Name, "下-1");
                Assert.AreEqual(firstLayout.Alignment, Anchor.BottomRight);
                Assert.AreEqual(firstLayout.HorizontalMargin, 30);
                Assert.AreEqual(firstLayout.VerticalMargin, 45);
                Assert.AreEqual(firstLayout.Continuous, false);
                Assert.AreEqual(firstLayout.SmartHorizon, KaraokeTextSmartHorizon.Multi);
                Assert.AreEqual(firstLayout.LyricsInterval, 4);
                Assert.AreEqual(firstLayout.RubyInterval, 2);
                Assert.AreEqual(firstLayout.RubyAlignment, LyricTextAlignment.Auto);
                Assert.AreEqual(firstLayout.RubyMargin, 4);

                // Testing style
                var firstFont = skin.Fonts.FirstOrDefault();
                Assert.IsNotNull(firstFont);
                Assert.AreEqual(firstFont.Name, "標準配色");

                // Test back text brush
                var backTextBrushInfo = firstFont.BackTextBrushInfo.TextBrush;
                Assert.AreEqual(backTextBrushInfo.BrushGradients.Count, 3);
                Assert.AreEqual(backTextBrushInfo.SolidColor, new Color4(255, 255, 255, 255));
                Assert.AreEqual(backTextBrushInfo.Type, BrushType.Solid);

                // Test font info
                var lyricTextFontInfo = firstFont.LyricTextFontInfo.LyricTextFontInfo;
                Assert.AreEqual(lyricTextFontInfo.FontName, "游明朝 Demibold");
                Assert.AreEqual(lyricTextFontInfo.Bold, true);
                Assert.AreEqual(lyricTextFontInfo.CharSize, 40);
                Assert.AreEqual(lyricTextFontInfo.EdgeSize, 10);
            }
        }
    }
}
