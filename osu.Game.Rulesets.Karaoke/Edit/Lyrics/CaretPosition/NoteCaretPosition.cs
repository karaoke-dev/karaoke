// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition
{
    public struct NoteCaretPosition : IIndexCaretPosition
    {
        public NoteCaretPosition(Lyric lyric, Note? note, CaretGenerateType generateType = CaretGenerateType.Action)
        {
            Lyric = lyric;
            Note = note;
            GenerateType = generateType;
        }

        public Lyric Lyric { get; }

        public Note? Note { get; }

        public CaretGenerateType GenerateType { get; }
    }
}
