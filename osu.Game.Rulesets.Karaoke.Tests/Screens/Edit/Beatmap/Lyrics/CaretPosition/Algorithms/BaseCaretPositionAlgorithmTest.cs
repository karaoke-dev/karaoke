// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.CaretPosition.Algorithms;

public abstract class BaseCaretPositionAlgorithmTest<TAlgorithm, TCaret> where TAlgorithm : ICaretPositionAlgorithm where TCaret : struct, ICaretPosition
{
    protected void TestPositionMovable(Lyric[] lyrics, TCaret caret, bool expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
        if (algorithm == null)
            throw new ArgumentNullException();

        invokeAlgorithm?.Invoke(algorithm);

        // because we made the position movable into the protected, so use another way to test it.
        var method = algorithm.GetType().GetMethod("PositionMovable", BindingFlags.Instance | BindingFlags.NonPublic);
        object? result = method?.Invoke(algorithm, new object[] { caret });
        if (result is not bool actual)
            throw new InvalidCastException();

        Assert.AreEqual(expected, actual);
    }

    protected void TestMoveToPreviousLyric(Lyric[] lyrics, TCaret caret, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
        if (algorithm == null)
            throw new ArgumentNullException();

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToPreviousLyric(caret) as TCaret?;
        AssertEqual(expected, actual);
    }

    protected void TestMoveToNextLyric(Lyric[] lyrics, TCaret caret, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
        if (algorithm == null)
            throw new ArgumentNullException();

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToNextLyric(caret) as TCaret?;
        AssertEqual(expected, actual);
    }

    protected void TestMoveToFirstLyric(Lyric[] lyrics, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
        if (algorithm == null)
            throw new ArgumentNullException();

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToFirstLyric() as TCaret?;
        AssertEqual(expected, actual);
    }

    protected void TestMoveToLastLyric(Lyric[] lyrics, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
        if (algorithm == null)
            throw new ArgumentNullException();

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToLastLyric() as TCaret?;
        AssertEqual(expected, actual);
    }

    protected void TestMoveToTargetLyric(Lyric[] lyrics, Lyric lyric, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
    {
        var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
        if (algorithm == null)
            throw new ArgumentNullException();

        invokeAlgorithm?.Invoke(algorithm);

        var actual = algorithm.MoveToTargetLyric(lyric) as TCaret?;
        AssertEqual(expected, actual);
    }

    protected void AssertEqual(TCaret? expected, TCaret? actual)
    {
        if (expected == null || actual == null)
        {
            Assert.IsNull(expected);
            Assert.IsNull(actual);
        }
        else
        {
            AssertEqual(expected.Value, actual.Value);
        }
    }

    protected abstract void AssertEqual(TCaret expected, TCaret actual);

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
        var theMethod = type.GetProperty(methodName, BindingFlags.NonPublic | BindingFlags.Static);

        if (theMethod != null)
            return theMethod;

        var baseType = type.BaseType;
        if (baseType == null)
            return null;

        return getMethod(baseType, methodName);
    }
}
