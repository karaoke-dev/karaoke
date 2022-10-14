// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.CaretPosition.Algorithms
{
    public abstract class BaseIndexCaretPositionAlgorithmTest<TAlgorithm, TCaret> : BaseCaretPositionAlgorithmTest<TAlgorithm, TCaret>
        where TAlgorithm : IndexCaretPositionAlgorithm<TCaret> where TCaret : struct, IIndexCaretPosition
    {
        protected void TestMoveLeft(Lyric[] lyrics, TCaret caret, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveLeft(caret);
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }

        protected void TestMoveRight(Lyric[] lyrics, TCaret caret, TCaret? expected, Action<TAlgorithm>? invokeAlgorithm = null)
        {
            var algorithm = (TAlgorithm?)Activator.CreateInstance(typeof(TAlgorithm), new object[] { lyrics });
            if (algorithm == null)
                throw new ArgumentNullException();

            invokeAlgorithm?.Invoke(algorithm);

            var actual = algorithm.MoveRight(caret);
            AssertEqual(expected, actual);
            CheckCaretGenerateType(CaretGenerateType.Action, actual);
        }
    }
}
