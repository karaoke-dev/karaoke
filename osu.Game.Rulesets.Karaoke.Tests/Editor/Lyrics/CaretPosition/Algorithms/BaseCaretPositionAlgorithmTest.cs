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
    public abstract class BaseCaretPositionAlgorithmTest<TAlgorithm, TCaret> where TAlgorithm : CaretPositionAlgorithm<TCaret> where TCaret : class, ICaretPosition
    {
        protected const int NOT_EXIST = -1;

        protected void TestPositionMovable(Lyric[] lyrics, TCaret caret, bool movable, Action<TAlgorithm> invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            Assert.AreEqual(movable, algorithm.PositionMovable(caret));
        }

        protected void TestMoveUp(Lyric[] lyrics, TCaret caret, TCaret expected, Action<TAlgorithm> invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(expected, algorithm.MoveUp(caret));
        }

        protected void TestMoveDown(Lyric[] lyrics, TCaret caret, TCaret expected, Action<TAlgorithm> invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(expected, algorithm.MoveDown(caret));
        }

        protected void TestMoveLeft(Lyric[] lyrics, TCaret caret, TCaret expected, Action<TAlgorithm> invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(expected, algorithm.MoveLeft(caret));
        }

        protected void TestMoveRight(Lyric[] lyrics, TCaret caret, TCaret expected, Action<TAlgorithm> invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(expected, algorithm.MoveRight(caret));
        }

        protected void TestMoveToFirst(Lyric[] lyrics, TCaret expected, Action<TAlgorithm> invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(expected, algorithm.MoveToFirst());
        }

        protected void TestMoveToLast(Lyric[] lyrics, TCaret expected, Action<TAlgorithm> invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(expected, algorithm.MoveToLast());
        }

        protected void TestMoveToTarget(Lyric[] lyrics, Lyric lyric, TCaret expected, Action<TAlgorithm> invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(expected, algorithm.MoveToTarget(lyric));
        }

        protected abstract void AssertEqual(TCaret expected, TCaret actual);

        protected Lyric[] GetLyricsByMethodName(string methodName)
        {
            var thisType = GetType();
            var theMethod = thisType.GetProperty(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (theMethod == null)
                throw new MissingMethodException("Test method is not exist.");

            return theMethod.GetValue(this) as Lyric[];
        }
    }
}
