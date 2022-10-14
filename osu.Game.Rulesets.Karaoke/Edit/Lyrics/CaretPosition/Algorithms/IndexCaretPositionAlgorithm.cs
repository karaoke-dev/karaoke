// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public abstract class IndexCaretPositionAlgorithm<TCaretPosition> : CaretPositionAlgorithm<TCaretPosition>, IIndexCaretPositionAlgorithm
        where TCaretPosition : struct, IIndexCaretPosition
    {
        protected IndexCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }
    }
}
