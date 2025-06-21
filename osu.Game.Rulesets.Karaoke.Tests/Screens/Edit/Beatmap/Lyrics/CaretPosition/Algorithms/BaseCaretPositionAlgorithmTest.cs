// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.CaretPosition.Algorithms;

public abstract class BaseCaretPositionAlgorithmTest<TAlgorithm, TCaret> where TAlgorithm : ICaretPositionAlgorithm where TCaret : struct, ICaretPosition
{
    protected static void TestPositionMovable(Lyric[] lyrics, TCaret caret, bool expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = CreateAlgorithm(lyrics);

        invokeAlgorithm?.Invoke(algorithm);

        // because we made the position movable into the protected, so use another way to test it.
        var method = algorithm.GetType().GetMethod("PositionMovable", BindingFlags.Instance | BindingFlags.NonPublic);
        object? result = method?.Invoke(algorithm, new object[] { caret });
        if (result is not bool actual)
            throw new InvalidCastException();

        Assert.That(expected, Is.EqualTo(actual));
    }

    protected static void TestMoveToPreviousLyric(Lyric[] lyrics, TCaret caret, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = CreateAlgorithm(lyrics);

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToPreviousLyric(caret) as TCaret?;
        AssertEqual(expected, actual);
    }

    protected static void TestMoveToNextLyric(Lyric[] lyrics, TCaret caret, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = CreateAlgorithm(lyrics);

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToNextLyric(caret) as TCaret?;
        AssertEqual(expected, actual);
    }

    protected static void TestMoveToFirstLyric(Lyric[] lyrics, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = CreateAlgorithm(lyrics);

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToFirstLyric() as TCaret?;
        AssertEqual(expected, actual);
    }

    protected static void TestMoveToLastLyric(Lyric[] lyrics, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = CreateAlgorithm(lyrics);

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToLastLyric() as TCaret?;
        AssertEqual(expected, actual);
    }

    protected static void TestMoveToTargetLyric(Lyric[] lyrics, Lyric lyric, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = CreateAlgorithm(lyrics);

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToTargetLyric(lyric) as TCaret?;
        AssertEqual(expected, actual);
    }

    protected static TAlgorithm CreateAlgorithm(Lyric[] lyrics)
        => ActivatorUtils.CreateInstance<TAlgorithm>(new object[] { lyrics });

    protected static void AssertEqual(TCaret? expected, TCaret? actual)
    {
        if (expected == null || actual == null)
        {
            Assert.That(expected, Is.Null);
            Assert.That(actual, Is.Null);
        }
        else
        {
            Assert.That(actual.Value, Is.EqualTo(expected.Value));
        }
    }

    protected Lyric[] GetLyricsByMethodName(string methodName)
    {
        var thisType = GetType();
        var theMethod = getMethod(thisType, methodName);
        if (theMethod == null)
            throw new MissingMethodException("Test method is not exist.");

        return (Lyric[])theMethod.GetValue(this)!;
    }

    private static PropertyInfo? getMethod(Type type, string methodName)
    {
        Type? targetType = type;

        while (targetType != null)
        {
            var theMethod = targetType.GetProperty(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (theMethod != null)
                return theMethod;

            targetType = targetType.BaseType;
        }

        return null;
    }
}
