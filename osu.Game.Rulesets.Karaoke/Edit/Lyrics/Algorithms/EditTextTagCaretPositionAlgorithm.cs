// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    /// <summary>
    /// User select to edit
    /// Preparing to double-click to edit text or delete.
    /// </summary>
    public class EditTextTagCaretPositionAlgorithm : TextTagCaretPositionAlgorithm<EditTextTagCaretPosition>
    {
        public EditTextTagCaretPositionAlgorithm(Lyric[] lyrics, EditArea editArea)
            : base(lyrics, editArea)
        {

        }

        public override EditTextTagCaretPosition MoveUp(EditTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override EditTextTagCaretPosition MoveDown(EditTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override EditTextTagCaretPosition MoveLeft(EditTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override EditTextTagCaretPosition MoveRight(EditTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override EditTextTagCaretPosition MoveToFirst()
        {
            throw new System.NotImplementedException();
        }

        public override EditTextTagCaretPosition MoveToLast()
        {
            throw new System.NotImplementedException();
        }
    }
}
