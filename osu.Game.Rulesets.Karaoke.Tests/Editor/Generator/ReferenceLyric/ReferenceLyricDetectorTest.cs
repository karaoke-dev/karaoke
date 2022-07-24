// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.ReferenceLyric;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.ReferenceLyric
{
    [TestFixture]
    public class ReferenceLyricDetectorTest : BaseDetectorTest<ReferenceLyricDetector, Lyric, ReferenceLyricDetectorConfig>
    {
        [TestCase("karaoke", "karaoke", true)]
        [TestCase("karaoke", "karaoke -", false)] // should be able to detect only if two lyric text are the same.
        [TestCase("- karaoke", "karaoke", false)] // should be able to detect only if two lyric text are the same.
        public void TestCanDetect(string lyricText, string detectedLyricText, bool canDetect)
        {
            var detectedLyric = new Lyric
            {
                Text = detectedLyricText
            };

            var lyrics = new[]
            {
                new Lyric
                {
                    Text = lyricText,
                },
                detectedLyric
            };
            var config = GeneratorConfig();
            CheckCanDetect(lyrics, detectedLyric, canDetect, config);
        }

        [TestCase("karaoke", "karaoke", true)]
        [TestCase("karaoke", "カラオケ", false)]
        [TestCase("karaoke", "karaoke -", true)]
        [TestCase("karaoke", "- karaoke", true)]
        [TestCase("karaoke", "- karaoke -", true)]
        [TestCase("karaoke", "karaokeカラオケ", false)]
        [TestCase("karaoke -", "karaoke", true)]
        [TestCase("- karaoke", "karaoke", true)]
        [TestCase("- karaoke -", "karaoke", true)]
        [TestCase("カラオケkaraoke", "karaoke", false)]
        [TestCase("- karaoke", "karaoke -", false)] // it's not supported now.
        public void TestCanDetectWithIgnorePrefixAndPostfixSymbol(string lyricText, string detectedLyricText, bool canDetect)
        {
            var detectedLyric = new Lyric
            {
                Text = detectedLyricText
            };

            var lyrics = new[]
            {
                new Lyric
                {
                    Text = lyricText,
                },
                detectedLyric
            };
            var config = GeneratorConfig("IgnorePrefixAndPostfixSymbol");
            CheckCanDetect(lyrics, detectedLyric, canDetect, config);
        }

        protected override void AssertEqual(Lyric expected, Lyric actual)
        {
            throw new System.NotImplementedException();
        }

        #region Utility function

        protected static void CheckCanDetect(IEnumerable<Lyric> lyrics, Lyric lyric, bool canDetect, ReferenceLyricDetectorConfig config)
        {
            var detector = new ReferenceLyricDetector(lyrics, config);

            CheckCanDetect(lyric, canDetect, detector);
        }

        #endregion
    }
}
