﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    /// <summary>
    /// User is typing ruby/romaji text.
    /// </summary>
    public class TypingTextTagCaretPositionAlgorithm : TextTagCaretPositionAlgorithm<TypingTextTagCaretPosition>
    {
        public TypingTextTagCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(TypingTextTagCaretPosition position)
        {
            if (!base.PositionMovable(position))
                return false;

            // check cursor in in the range
            var text = position.TextTag.Text;
            if (text == null)
                return false;

            return position.TypingCaretIndex >= 0 && position.TypingCaretIndex <= text.Length;
        }

        public override TypingTextTagCaretPosition MoveUp(TypingTextTagCaretPosition currentPosition)
        {
            // in typing mode should not have moving up.
            return null;
        }

        public override TypingTextTagCaretPosition MoveDown(TypingTextTagCaretPosition currentPosition)
        {
            // in typing mode should not have moving up.
            return null;
        }

        public override TypingTextTagCaretPosition MoveLeft(TypingTextTagCaretPosition currentPosition)
        {
            // only move cursor position in text tag.
            var newIndex = Math.Max(currentPosition.TypingCaretIndex - 1, 0);
            if (newIndex == currentPosition.TypingCaretIndex)
                return null;

            return new TypingTextTagCaretPosition(currentPosition.Lyric, currentPosition.TextTag, newIndex);
        }

        public override TypingTextTagCaretPosition MoveRight(TypingTextTagCaretPosition currentPosition)
        {
            // only move cursor position in text tag.
            var text = currentPosition.TextTag.Text;
            var newIndex = Math.Min(currentPosition.TypingCaretIndex + 1, text.Length);
            if (newIndex == currentPosition.TypingCaretIndex)
                return null;

            return new TypingTextTagCaretPosition(currentPosition.Lyric, currentPosition.TextTag, newIndex);
        }

        public override TypingTextTagCaretPosition MoveToFirst()
        {
            // not support this feature if typing.
            return null;
        }

        public override TypingTextTagCaretPosition MoveToLast()
        {
            // not support this feature if typing.
            return null;
        }

        public override TypingTextTagCaretPosition MoveToTarget(Lyric lyric)
        {
            // lazy to implement this algorithm because this algorithm haven't being used.
            return null;
        }
    }
}
