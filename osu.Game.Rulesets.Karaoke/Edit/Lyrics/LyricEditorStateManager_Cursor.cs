// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditorStateManager
    {
        public Bindable<CursorPosition> BindableHoverCursorPosition { get; } = new Bindable<CursorPosition>();

        public Bindable<CursorPosition> BindableCursorPosition { get; } = new Bindable<CursorPosition>();

        public void MoveCursorToTargetPosition(Lyric lyric, TextIndex index)
        {
            movePositionTo(new CursorPosition(lyric, index));
        }

        public void MoveHoverCursorToTargetPosition(Lyric lyric, TextIndex index)
        {
            moveHoverPositionTo(new CursorPosition(lyric, index));
        }

        public void ClearHoverCursorPosition()
        {
            BindableHoverCursorPosition.Value = new CursorPosition();
        }

        private bool moveCursor(CursorAction action)
        {
            var currentPosition = BindableCursorPosition.Value;

            CursorPosition position = new CursorPosition();

            switch (action)
            {
                case CursorAction.MoveUp:
                    position = getPreviousLyricCursorPosition(currentPosition);
                    break;

                case CursorAction.MoveDown:
                    position = getNextLyricCursorPosition(currentPosition);
                    break;

                case CursorAction.MoveLeft:
                    position = getPreviousCursorPosition(currentPosition);
                    break;

                case CursorAction.MoveRight:
                    position = getNextCursorPosition(currentPosition);
                    break;

                case CursorAction.First:
                    position = getFirstCursorPosition();
                    break;

                case CursorAction.Last:
                    position = getLastCursorPosition();
                    break;
            }

            if (position.Lyric == null)
                return false;

            movePositionTo(position);
            return true;
        }

        private CursorPosition getPreviousLyricCursorPosition(CursorPosition position)
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            var lyric = lyrics.GetPrevious(position.Lyric);
            if (lyric == null)
                return new CursorPosition();

            var lyricTextLength = lyric.Text?.Length ?? 0;
            var index = Math.Min(position.Index.Index, lyricTextLength - 1);
            var state = position.Index.State;

            return new CursorPosition(lyric, new TextIndex(index, state));
        }

        private CursorPosition getNextLyricCursorPosition(CursorPosition position)
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            var lyric = lyrics.GetNext(position.Lyric);
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
            var previousTimeTag = getPreviousTag(Mode, position.Index);

            if (previousTimeTag.Index < 0)
            {
                getNextLyricCursorPosition(new CursorPosition(position.Lyric, new TextIndex(int.MaxValue)));
            }

            return new CursorPosition(position.Lyric, previousTimeTag);

            static TextIndex getPreviousTag(Mode mode, TextIndex currentTimeTag)
            {
                switch (mode)
                {
                    case Mode.EditMode:
                    case Mode.TypingMode:
                        return new TextIndex(currentTimeTag.Index - 1, currentTimeTag.State);

                    case Mode.TimeTagEditMode:
                        var nextIndex = TextIndexUtils.ToLyricIndex(currentTimeTag) - 1;
                        var nextState = TextIndexUtils.ReverseState(currentTimeTag.State);
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
            var nextTimeTag = getNextTag(Mode, position.Index);

            if (nextTimeTag.Index >= textLength)
            {
                getNextLyricCursorPosition(new CursorPosition(position.Lyric, new TextIndex(int.MinValue)));
            }

            return new CursorPosition(position.Lyric, nextTimeTag);

            static TextIndex getNextTag(Mode mode, TextIndex currentTimeTag)
            {
                switch (mode)
                {
                    case Mode.EditMode:
                    case Mode.TypingMode:
                        return new TextIndex(currentTimeTag.Index + 1, currentTimeTag.State);

                    case Mode.TimeTagEditMode:
                        var nextIndex = TextIndexUtils.ToLyricIndex(currentTimeTag);
                        var nextState = TextIndexUtils.ReverseState(currentTimeTag.State);
                        return new TextIndex(nextIndex, nextState);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }
        }

        private CursorPosition getFirstCursorPosition()
        {
            var lyric = beatmap.HitObjects.OfType<Lyric>().FirstOrDefault();
            var index = new TextIndex();
            return new CursorPosition(lyric, index);
        }

        private CursorPosition getLastCursorPosition()
        {
            var lyric = beatmap.HitObjects.OfType<Lyric>().LastOrDefault();
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
