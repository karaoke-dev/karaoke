// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public abstract class CaretPositionAlgorithm<T> : ICaretPositionAlgorithm where T : ICaretPosition
    {
        // Lyrics is not lock and can be accessible.
        protected readonly Lyric[] Lyrics;

        protected CaretPositionAlgorithm(Lyric[] lyrics)
        {
            Lyrics = LyricsUtils.FindUnlockLyrics(OrderUtils.Sorted(lyrics));
        }

        public abstract bool PositionMovable(T position);

        public abstract T MoveUp(T currentPosition);

        public abstract T MoveDown(T currentPosition);

        public abstract T MoveLeft(T currentPosition);

        public abstract T MoveRight(T currentPosition);

        public abstract T MoveToFirst();

        public abstract T MoveToLast();

        public abstract T MoveToTarget(Lyric lyric);
    }
}
