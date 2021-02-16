// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditorStateManager
    {
        public void MoveCursorToTargetPosition(Lyric lyric, TextIndex index)
        {
            if (!Lyrics.Contains(lyric))
                return;

            movePositionTo(new CursorPosition(lyric, index));
        }

        public void MoveHoverCursorToTargetPosition(Lyric lyric, TextIndex index)
        {
            if (!Lyrics.Contains(lyric))
                return;

            moveHoverPositionTo(new CursorPosition(lyric, index));
        }

        public void ClearHoverCursorPosition()
        {
            BindableHoverCursorPosition.Value = new CursorPosition();
        }

        private bool moveCursor(MovingCursorAction action)
        {
            var currentPosition = BindableCursorPosition.Value;
            CursorPosition position;

            switch (action)
            {
                case MovingCursorAction.Up:
                    position = getPreviousLyricCursorPosition(currentPosition);
                    break;

                case MovingCursorAction.Down:
                    position = getNextLyricCursorPosition(currentPosition);
                    break;

                case MovingCursorAction.Left:
                    position = getPreviousCursorPosition(currentPosition);
                    break;

                case MovingCursorAction.Right:
                    position = getNextCursorPosition(currentPosition);
                    break;

                case MovingCursorAction.First:
                    position = getFirstCursorPosition();
                    break;

                case MovingCursorAction.Last:
                    position = getLastCursorPosition();
                    break;

                default:
                    throw new InvalidOperationException(nameof(action));
            }

            if (position.Lyric == null)
                return false;

            movePositionTo(position);
            return true;
        }

        private CursorPosition getPreviousLyricCursorPosition(CursorPosition position)
        {
            var lyric = Lyrics.GetPrevious(position.Lyric);
            if (lyric == null)
                return new CursorPosition();

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Min(position.Index.Index, lyricTextLength - 1);
            var state = position.Index.State;

            return new CursorPosition(lyric, new TextIndex(index, state));
        }

        private CursorPosition getNextLyricCursorPosition(CursorPosition position)
        {
            var lyric = Lyrics.GetNext(position.Lyric);
            if (lyric == null)
                return new CursorPosition();

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Min(position.Index.Index, lyricTextLength - 1);
            var state = position.Index.State;

            return new CursorPosition(lyric, new TextIndex(index, state));
        }

        private CursorPosition getPreviousCursorPosition(CursorPosition position)
        {
            // get previous cursor and make a check is need to change line.
            var previousIndex = getPreviousIndex(Mode, position.Index);

            if (previousIndex.Index < 0)
            {
                getNextLyricCursorPosition(new CursorPosition(position.Lyric, new TextIndex(int.MaxValue)));
            }

            return new CursorPosition(position.Lyric, previousIndex);

            static TextIndex getPreviousIndex(Mode mode, TextIndex currentIndex)
            {
                switch (mode)
                {
                    case Mode.EditMode:
                    case Mode.TypingMode:
                        return new TextIndex(currentIndex.Index - 1, currentIndex.State);

                    case Mode.TimeTagEditMode:
                        var nextIndex = TextIndexUtils.ToLyricIndex(currentIndex) - 1;
                        var nextState = TextIndexUtils.ReverseState(currentIndex.State);
                        return new TextIndex(nextIndex, nextState);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }
        }

        private CursorPosition getNextCursorPosition(CursorPosition position)
        {
            var textLength = position.Lyric?.Text?.Length ?? 0;

            // get next cursor and make a check is need to change line.
            var nextIndex = getNextIndex(Mode, position.Index);

            if (nextIndex.Index >= textLength)
            {
                getNextLyricCursorPosition(new CursorPosition(position.Lyric, new TextIndex(int.MinValue)));
            }

            return new CursorPosition(position.Lyric, nextIndex);

            static TextIndex getNextIndex(Mode mode, TextIndex currentIndex)
            {
                switch (mode)
                {
                    case Mode.EditMode:
                    case Mode.TypingMode:
                        return new TextIndex(currentIndex.Index + 1, currentIndex.State);

                    case Mode.TimeTagEditMode:
                        var nextIndex = TextIndexUtils.ToLyricIndex(currentIndex);
                        var nextState = TextIndexUtils.ReverseState(currentIndex.State);
                        return new TextIndex(nextIndex, nextState);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }
        }

        private CursorPosition getFirstCursorPosition()
        {
            var lyric = Lyrics.FirstOrDefault();
            var index = new TextIndex();
            return new CursorPosition(lyric, index);
        }

        private CursorPosition getLastCursorPosition()
        {
            var lyric = Lyrics.LastOrDefault();
            var textLength = lyric?.Text.Length ?? 0;
            var index = new TextIndex(textLength - 1, TextIndex.IndexState.End);
            return new CursorPosition(lyric, index);
        }

        private void movePositionTo(CursorPosition position)
        {
            if (position.Lyric == null)
                return;

            BindableHoverCursorPosition.Value = new CursorPosition();
            BindableCursorPosition.Value = position;
        }

        private void moveHoverPositionTo(CursorPosition position)
        {
            if (position.Lyric == null)
                return;

            BindableHoverCursorPosition.Value = position;
        }
    }
}
