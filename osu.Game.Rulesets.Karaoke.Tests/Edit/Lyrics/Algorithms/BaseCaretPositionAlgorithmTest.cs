// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Lyrics.Algorithms
{
    public abstract class BaseCaretPositionAlgorithmTest<T, C> where T : CaretPositionAlgorithm<C> where C : ICaretPosition
    {
        protected const int NOT_EXIST = -1;

        protected void TestPositionMovable(Lyric[] lyrics, C caret, bool movable)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            Assert.AreEqual(algorithm.PositionMovable(caret), movable);
        }

        protected void TestMoveUp(Lyric[] lyrics, C caret, C actual)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            AssertEqual(algorithm.MoveUp(caret), actual);
        }

        protected void TestMoveDown(Lyric[] lyrics, C caret, C actual)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            AssertEqual(algorithm.MoveDown(caret), actual);
        }

        protected void TestMoveLeft(Lyric[] lyrics, C caret, C actual)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            AssertEqual(algorithm.MoveLeft(caret), actual);
        }

        protected void TestMoveRight(Lyric[] lyrics, C caret, C actual)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            AssertEqual(algorithm.MoveRight(caret), actual);
        }

        protected void TestMoveToFirst(Lyric[] lyrics, C actual)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            AssertEqual(algorithm.MoveToFirst(), actual);
        }

        protected void TestMoveToLast(Lyric[] lyrics, C actual)
        {
            var algorithm = (T)Activator.CreateInstance(typeof(T), new object[] { lyrics });
            AssertEqual(algorithm.MoveToLast(), actual);
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
