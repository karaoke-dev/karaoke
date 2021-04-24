// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public class EditNoteCaretPositionAlgorithm : CaretPositionAlgorithm<EditNoteCaretPosition>
    {
        public EditNoteCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(EditNoteCaretPosition position)
        {
            return true;
        }

        public override EditNoteCaretPosition MoveUp(EditNoteCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPrevious(currentPosition.Lyric);
            if (lyric == null)
                return null;

            return new EditNoteCaretPosition(lyric);
        }

        public override EditNoteCaretPosition MoveDown(EditNoteCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNext(currentPosition.Lyric);
            if (lyric == null)
                return null;

            return new EditNoteCaretPosition(lyric);
        }

        public override EditNoteCaretPosition MoveLeft(EditNoteCaretPosition currentPosition)
        {
            return null;
        }

        public override EditNoteCaretPosition MoveRight(EditNoteCaretPosition currentPosition)
        {
            return null;
        }

        public override EditNoteCaretPosition MoveToFirst()
        {
            var lyric = Lyrics.FirstOrDefault();
            if (lyric == null)
                return null;

            return new EditNoteCaretPosition(lyric);
        }

        public override EditNoteCaretPosition MoveToLast()
        {
            var lyric = Lyrics.LastOrDefault();
            if (lyric == null)
                return null;

            return new EditNoteCaretPosition(lyric);
        }
    }
}
