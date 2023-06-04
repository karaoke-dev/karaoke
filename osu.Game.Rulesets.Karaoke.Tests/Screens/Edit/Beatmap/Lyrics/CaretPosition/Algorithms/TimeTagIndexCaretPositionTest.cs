// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.CaretPosition.Algorithms;

[TestFixture]
public class TimeTagIndexCaretPositionTest : BaseCharIndexCaretPositionAlgorithmTest<TimeTagIndexCaretPositionAlgorithm, TimeTagIndexCaretPosition>
{
    protected override TimeTagIndexCaretPosition CreateCaret(Lyric lyric, int index)
        => new(lyric, index);

    protected override void AssertEqual(TimeTagIndexCaretPosition expected, TimeTagIndexCaretPosition actual)
    {
        Assert.AreEqual(expected.Lyric, actual.Lyric);
        Assert.AreEqual(expected.CharIndex, actual.CharIndex);
    }
}
