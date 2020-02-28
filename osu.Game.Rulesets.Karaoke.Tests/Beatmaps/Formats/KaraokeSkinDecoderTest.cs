// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Formats
{
    [TestFixture]
    public class KaraokeSkinDecoderTest
    {
        [Test]
        public void TestDecodeKaraokeSkin()
        {
            using (var resStream = TestResources.OpenSkinResource("default"))
            using (var stream = new LineBufferedReader(resStream))
            {
                var decoder = new KaraokeSkinDecoder();
                var skin = decoder.Decode(stream);

                // Checking font decode result
                var firstDecodedFont = skin.Fonts.FirstOrDefault();
                Assert.IsNotNull(firstDecodedFont);
                Assert.AreEqual(firstDecodedFont.Name, "標準配色");

                // Test back text brush
                var backTextBrushInfo = firstDecodedFont.BackTextBrushInfo.TextBrush;
                Assert.AreEqual(backTextBrushInfo.BrushGradients.Count, 3);
                Assert.AreEqual(backTextBrushInfo.SolidColor, new Color4(255, 255, 255, 255));
                Assert.AreEqual(backTextBrushInfo.Type, BrushType.Solid);

                // Test font info
                var lyricTextFontInfo = firstDecodedFont.LyricTextFontInfo.LyricTextFontInfo;
                Assert.AreEqual(lyricTextFontInfo.FontName, "游明朝 Demibold");
                Assert.AreEqual(lyricTextFontInfo.Bold, true);
                Assert.AreEqual(lyricTextFontInfo.CharSize, 40);
                Assert.AreEqual(lyricTextFontInfo.EdgeSize, 10);

                // Checking layout decode result
                var firstDecodedLayout = skin.Layouts.FirstOrDefault();
                Assert.AreEqual(firstDecodedLayout.Name, "下-1");
                Assert.AreEqual(firstDecodedLayout.Alignment, Anchor.BottomRight);
                Assert.AreEqual(firstDecodedLayout.HorizontalMargin, 30);
                Assert.AreEqual(firstDecodedLayout.VerticalMargin, 45);
                Assert.AreEqual(firstDecodedLayout.Continuous, false);
                Assert.AreEqual(firstDecodedLayout.SmartHorizon, KaraokeTextSmartHorizon.Multi);
                Assert.AreEqual(firstDecodedLayout.LyricsInterval, 4);
                Assert.AreEqual(firstDecodedLayout.RubyInterval, 2);
                Assert.AreEqual(firstDecodedLayout.RubyAlignment, LyricTextAlignment.Auto);
                Assert.AreEqual(firstDecodedLayout.RomajiAlignment, LyricTextAlignment.Auto);
                Assert.AreEqual(firstDecodedLayout.RubyMargin, 4);
                Assert.AreEqual(firstDecodedLayout.RomajiMargin, 0);

                // Checking note decode result
                var firstDecodedNoteSkin = skin.NoteSkins.FirstOrDefault();
                Assert.AreEqual(firstDecodedNoteSkin.Name, "Note-1");
                Assert.AreEqual(firstDecodedNoteSkin.NoteColor, new Color4(128, 128, 128, 255));
                Assert.AreEqual(firstDecodedNoteSkin.BlinkColor, new Color4(128, 128, 128, 255));
                Assert.AreEqual(firstDecodedNoteSkin.TextColor, new Color4(128, 128, 128, 255));
                Assert.AreEqual(firstDecodedNoteSkin.BoldText, true);
            }
        }
    }
}
