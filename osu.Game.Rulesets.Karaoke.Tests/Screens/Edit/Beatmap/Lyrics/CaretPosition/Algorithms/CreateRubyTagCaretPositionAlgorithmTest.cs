// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.CaretPosition.Algorithms;

[TestFixture]
public class CreateRubyTagCaretPositionAlgorithmTest : BaseCharIndexCaretPositionAlgorithmTest<CreateRubyTagCaretPositionAlgorithm, CreateRubyTagCaretPosition>
{
    #region Lyric index

    [TestCase(nameof(singleLyric), 0, 0, 3, 3)]
    [TestCase(nameof(singleLyric), 0, 3, 0, 0)] // should be OK if the release char index is smaller than char index.
    [TestCase(nameof(singleLyric), 0, 0, 0, 0)] // should be OK if the release char index is same as char index.
    [TestCase(nameof(singleLyric), 0, 3, 3, 3)]
    [TestCase(nameof(singleLyric), 0, 0, -1, null)] // should be invalid if the release char index is out of range.
    [TestCase(nameof(singleLyric), 0, 0, 4, null)]
    [TestCase(nameof(singleLyric), 0, -1, 0, null)] // should be invalid if the start char index is out of range.
    [TestCase(nameof(singleLyric), 0, -1, 3, null)]
    public void TestAdjustEndIndex(string sourceName, int lyricIndex, int charIndex, int releaseCharIndex, int? expectedReleaseCharIndex)
    {
        var lyrics = GetLyricsByMethodName(sourceName);
        var lyric = lyrics[lyricIndex];

        var caretPosition = CreateCaret(lyric, charIndex);
        var expected = createExpectedCaretPosition(lyric, charIndex, expectedReleaseCharIndex);

        // Check move to target position.
        TestAdjustEndIndex(lyrics, caretPosition, releaseCharIndex, expected);

        static CreateRubyTagCaretPosition? createExpectedCaretPosition(Lyric lyric, int index, int? releaseIndex)
        {
            if (releaseIndex == null)
                return null;

            return new CreateRubyTagCaretPosition(lyric, index, releaseIndex.Value);
        }
    }

    #endregion

    protected override CreateRubyTagCaretPosition CreateCaret(Lyric lyric, int index)
        => new(lyric, index, index);

    protected override void AssertEqual(CreateRubyTagCaretPosition expected, CreateRubyTagCaretPosition actual)
    {
        Assert.AreEqual(expected.Lyric, actual.Lyric);
        Assert.AreEqual(expected.CharIndex, actual.CharIndex);
        Assert.AreEqual(expected.ReleaseCharIndex, actual.ReleaseCharIndex);
    }

    #region source

    private static Lyric[] singleLyric => new[]
    {
        new Lyric
        {
            Text = "カラオケ"
        }
    };

    #endregion
}
