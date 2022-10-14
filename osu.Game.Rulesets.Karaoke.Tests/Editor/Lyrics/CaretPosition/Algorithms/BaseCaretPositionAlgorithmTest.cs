// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.CaretPosition.Algorithms
{
    public abstract class BaseCaretPositionAlgorithmTest<TAlgorithm, TCaret> where TAlgorithm : ICaretPositionAlgorithm where TCaret : struct, ICaretPosition
    {
        protected void TestPositionMovable(Lyric[] lyrics, TCaret caret, bool expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            bool actual = algorithm.PositionMovable(caret);
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
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }

        protected void TestMoveToNextLyric(Lyric[] lyrics, TCaret caret, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveToNextLyric(caret) as TCaret?;
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }

        protected void TestMoveToFirstLyric(Lyric[] lyrics, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveToFirstLyric() as TCaret?;
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }

        protected void TestMoveToLastLyric(Lyric[] lyrics, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveToLastLyric() as TCaret?;
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }

        protected void TestMoveToTargetLyric(Lyric[] lyrics, Lyric lyric, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveToTargetLyric(lyric) as TCaret?;
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.TargetLyric, actual);
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

        protected void CheckCaretGenerateType(CaretGenerateType expected, TCaret? actual)
        {
            if (actual == null)
                return;

            Assert.AreEqual(expected, actual.Value.GenerateType);
        }

        protected abstract void AssertEqual(TCaret expected, TCaret actual);

        protected Lyric[] GetLyricsByMethodName(string methodName)
        {
            var thisType = GetType();
            var theMethod = thisType.GetProperty(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (theMethod == null)
                throw new MissingMethodException("Test method is not exist.");

            return (Lyric[])theMethod.GetValue(this)!;
        }
    }
}
