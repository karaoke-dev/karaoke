// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class NavigateCaretPositionAlgorithm : CaretPositionAlgorithm<NavigateCaretPosition>
    {
        public NavigateCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(NavigateCaretPosition position)
        {
            return true;
        }

        public override NavigateCaretPosition MoveUp(NavigateCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPrevious(currentPosition.Lyric);
            if (lyric == null)
                return null;

            return new NavigateCaretPosition(lyric);
        }

        public override NavigateCaretPosition MoveDown(NavigateCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNext(currentPosition.Lyric);
            if (lyric == null)
                return null;

            return new NavigateCaretPosition(lyric);
        }

        public override NavigateCaretPosition MoveLeft(NavigateCaretPosition currentPosition)
        {
            return null;
        }

        public override NavigateCaretPosition MoveRight(NavigateCaretPosition currentPosition)
        {
            return null;
        }

        public override NavigateCaretPosition MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault();
            if (lyric == null)
                return null;

            return new NavigateCaretPosition(lyric);
        }

        public override NavigateCaretPosition MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault();
            if (lyric == null)
                return null;

            return new NavigateCaretPosition(lyric);
        }

        public override NavigateCaretPosition MoveToTarget(Lyric lyric)
        {
            return new NavigateCaretPosition(lyric);
        }
    }
}
