﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public class TypingTextTagCaretPosition : ITextTagCaretPosition
    {
        public TypingTextTagCaretPosition(Lyric lyric, ITextTag textTag, int typingCaretIndex)
        {
            Lyric = lyric;
            TextTag = textTag;
            TypingCaretIndex = typingCaretIndex;
        }

        public Lyric Lyric { get; }

        public ITextTag TextTag { get; }

        public int TypingCaretIndex { get; }
    }
}
