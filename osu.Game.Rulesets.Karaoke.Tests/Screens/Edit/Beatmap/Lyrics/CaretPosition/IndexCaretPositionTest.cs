// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.CaretPosition;

[TestFixture(typeof(CreateRubyTagCaretPosition))]
[TestFixture(typeof(CuttingCaretPosition))]
[TestFixture(typeof(RecordingTimeTagCaretPosition))]
[TestFixture(typeof(CreateRemoveTimeTagCaretPosition))]
[TestFixture(typeof(TypingCaretPosition))]
public class IndexCaretPositionTest<TIndexCaretPosition> where TIndexCaretPosition : IIndexCaretPosition
{
    [Test]
    public void TestCompareWithLargerIndex()
    {
        var lyric = new Lyric();

        var caretPosition = createSmallerCaretPosition(lyric);
        var comparedCaretPosition = createBiggerCaretPosition(lyric);

        Assert.IsTrue(caretPosition < comparedCaretPosition);
        Assert.IsTrue(caretPosition <= comparedCaretPosition);
        Assert.IsFalse(caretPosition >= comparedCaretPosition);
        Assert.IsFalse(caretPosition > comparedCaretPosition);
    }

    [Test]
    public void TestCompareEqualIndex()
    {
        var lyric = new Lyric();

        var caretPosition = createSmallerCaretPosition(lyric);
        var comparedCaretPosition = createSmallerCaretPosition(lyric);

        Assert.IsFalse(caretPosition < comparedCaretPosition);
        Assert.IsTrue(caretPosition <= comparedCaretPosition);
        Assert.IsTrue(caretPosition >= comparedCaretPosition);
        Assert.IsFalse(caretPosition > comparedCaretPosition);
    }

    [Test]
    public void TestCompareWithSmallerIndex()
    {
        var lyric = new Lyric();

        var caretPosition = createBiggerCaretPosition(lyric);
        var comparedCaretPosition = createSmallerCaretPosition(lyric);

        Assert.IsFalse(caretPosition < comparedCaretPosition);
        Assert.IsFalse(caretPosition <= comparedCaretPosition);
        Assert.IsTrue(caretPosition >= comparedCaretPosition);
        Assert.IsTrue(caretPosition > comparedCaretPosition);
    }

    [Test]
    public void TestCompareWithDifferentLyric()
    {
        var lyric1 = new Lyric();
        var lyric2 = new Lyric();

        var caretPosition = createBiggerCaretPosition(lyric1);
        var comparedCaretPosition = createSmallerCaretPosition(lyric2);

        Assert.Throws<InvalidOperationException>(() =>
        {
            int _ = caretPosition.CompareTo(comparedCaretPosition);
        });
    }

    [Test]
    public void TestCompareDifferentType()
    {
        var lyric = new Lyric();

        var caretPosition = createBiggerCaretPosition(lyric);
        var comparedCaretPosition = new FakeCaretPosition(lyric);

        Assert.Throws<InvalidOperationException>(() =>
        {
            int _ = caretPosition.CompareTo(comparedCaretPosition);
        });
    }

    private static IIndexCaretPosition createSmallerCaretPosition(Lyric lyric) =>
        typeof(TIndexCaretPosition) switch
        {
            Type t when t == typeof(CreateRubyTagCaretPosition) => new CreateRubyTagCaretPosition(lyric, 0),
            Type t when t == typeof(CuttingCaretPosition) => new CuttingCaretPosition(lyric, 0),
            Type t when t == typeof(RecordingTimeTagCaretPosition) => new RecordingTimeTagCaretPosition(lyric, new TimeTag(new TextIndex())),
            Type t when t == typeof(CreateRemoveTimeTagCaretPosition) => new CreateRemoveTimeTagCaretPosition(lyric, 0),
            Type t when t == typeof(TypingCaretPosition) => new TypingCaretPosition(lyric, 0),
            _ => throw new NotSupportedException(),
        };

    private static IIndexCaretPosition createBiggerCaretPosition(Lyric lyric) =>
        typeof(TIndexCaretPosition) switch
        {
            Type t when t == typeof(CreateRubyTagCaretPosition) => new CreateRubyTagCaretPosition(lyric, 1),
            Type t when t == typeof(CuttingCaretPosition) => new CuttingCaretPosition(lyric, 1),
            Type t when t == typeof(RecordingTimeTagCaretPosition) => new RecordingTimeTagCaretPosition(lyric, new TimeTag(new TextIndex(1))),
            Type t when t == typeof(CreateRemoveTimeTagCaretPosition) => new CreateRemoveTimeTagCaretPosition(lyric, 1),
            Type t when t == typeof(TypingCaretPosition) => new TypingCaretPosition(lyric, 1),
            _ => throw new NotSupportedException(),
        };

    private struct FakeCaretPosition : IIndexCaretPosition
    {
        public FakeCaretPosition(Lyric lyric)
        {
            Lyric = lyric;
        }

        public Lyric Lyric { get; }

        public int CompareTo(IIndexCaretPosition? other)
        {
            throw new NotImplementedException();
        }
    }
}
