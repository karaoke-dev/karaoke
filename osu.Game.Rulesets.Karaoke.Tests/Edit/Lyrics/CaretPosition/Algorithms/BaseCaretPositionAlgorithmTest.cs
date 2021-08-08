// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Lyrics.CaretPosition.Algorithms
{
    public abstract class BaseCaretPositionAlgorithmTest<T, C> where T : CaretPositionAlgorithm<C> where C : ICaretPosition
    {
        protected const int NOT_EXIST = -1;

        protected void TestPositionMovable(Lyric[] lyrics, C caret, bool movable, Action<T> invokeAlgorithm = null)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            Assert.AreEqual(algorithm.PositionMovable(caret), movable);
        }

        protected void TestMoveUp(Lyric[] lyrics, C caret, C actual, Action<T> invokeAlgorithm = null)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(algorithm.MoveUp(caret), actual);
        }

        protected void TestMoveDown(Lyric[] lyrics, C caret, C actual, Action<T> invokeAlgorithm = null)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(algorithm.MoveDown(caret), actual);
        }

        protected void TestMoveLeft(Lyric[] lyrics, C caret, C actual, Action<T> invokeAlgorithm = null)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(algorithm.MoveLeft(caret), actual);
        }

        protected void TestMoveRight(Lyric[] lyrics, C caret, C actual, Action<T> invokeAlgorithm = null)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(algorithm.MoveRight(caret), actual);
        }

        protected void TestMoveToFirst(Lyric[] lyrics, C actual, Action<T> invokeAlgorithm = null)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(algorithm.MoveToFirst(), actual);
        }

        protected void TestMoveToLast(Lyric[] lyrics, C actual, Action<T> invokeAlgorithm = null)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(algorithm.MoveToLast(), actual);
        }

        protected void TestMoveToTarget(Lyric[] lyrics, Lyric lyric, C actual, Action<T> invokeAlgorithm = null)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            Assert.IsNotNull(algorithm);

            invokeAlgorithm?.Invoke(algorithm);
            AssertEqual(algorithm.MoveToTarget(lyric), actual);
        }

        protected abstract void AssertEqual(C compare, C actual);

        protected Lyric[] GetLyricsByMethodName(string methodName)
        {
            Type thisType = GetType();
            var theMethod = thisType.GetProperty(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (theMethod == null)
                throw new MissingMethodException("Test method is not exist.");

            return theMethod.GetValue(this) as Lyric[];
        }
    }
}
