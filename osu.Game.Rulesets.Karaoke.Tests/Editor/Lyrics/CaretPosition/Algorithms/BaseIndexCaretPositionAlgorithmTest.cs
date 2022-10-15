// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.CaretPosition.Algorithms
{
    public abstract class BaseIndexCaretPositionAlgorithmTest<TAlgorithm, TCaret> : BaseCaretPositionAlgorithmTest<TAlgorithm, TCaret>
        where TAlgorithm : IIndexCaretPositionAlgorithm where TCaret : struct, IIndexCaretPosition
    {
        protected void TestMoveToPreviousIndex(Lyric[] lyrics, TCaret caret, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveToPreviousIndex(caret) as TCaret?;
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }

        protected void TestMoveToNextIndex(Lyric[] lyrics, TCaret caret, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveToNextIndex(caret) as TCaret?;
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }

        protected void TestMoveToFirstIndex(Lyric[] lyrics, Lyric lyric, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveToFirstIndex(lyric) as TCaret?;
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }

        protected void TestMoveToLastIndex(Lyric[] lyrics, Lyric lyric, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveToLastIndex(lyric) as TCaret?;
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }
    }
}
