// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public class NoteCaretPositionAlgorithm : IndexCaretPositionAlgorithm<NoteCaretPosition>
    {
        public NoteCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected override void Validate(NoteCaretPosition input)
        {
            var noteInCaret = input.Note;
            var lyricInCaret = input.Lyric;

            Debug.Assert(noteInCaret == null || noteInCaret.ReferenceLyric == lyricInCaret);
        }

        protected override bool PositionMovable(NoteCaretPosition position)
        {
            return true;
        }

        protected override NoteCaretPosition? MoveToPreviousLyric(NoteCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPrevious(currentPosition.Lyric);
            if (lyric == null)
                return null;

            // todo: get the first note.
            return new NoteCaretPosition(lyric, null);
        }

        protected override NoteCaretPosition? MoveToNextLyric(NoteCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNext(currentPosition.Lyric);
            if (lyric == null)
                return null;

            // todo: get the first note.
            return new NoteCaretPosition(lyric, null);
        }

        protected override NoteCaretPosition? MoveToFirstLyric()
        {
            var lyric = Lyrics.FirstOrDefault();
            if (lyric == null)
                return null;

            // todo: get the first note.
            return new NoteCaretPosition(lyric, null);
        }

        protected override NoteCaretPosition? MoveToLastLyric()
        {
            var lyric = Lyrics.LastOrDefault();
            if (lyric == null)
                return null;

            // todo: get the first note.
            return new NoteCaretPosition(lyric, null);
        }

        protected override NoteCaretPosition? MoveToTargetLyric(Lyric lyric)
        {
            // todo: get the first note.
            return new NoteCaretPosition(lyric, null, CaretGenerateType.TargetLyric);
        }

        protected override NoteCaretPosition? MoveToPreviousIndex(NoteCaretPosition currentPosition)
        {
            return null;
        }

        protected override NoteCaretPosition? MoveToNextIndex(NoteCaretPosition currentPosition)
        {
            return null;
        }

        protected override NoteCaretPosition? MoveToFirstIndex(Lyric lyric)
        {
            return null;
        }

        protected override NoteCaretPosition? MoveToLastIndex(Lyric lyric)
        {
            return null;
        }
    }
}
