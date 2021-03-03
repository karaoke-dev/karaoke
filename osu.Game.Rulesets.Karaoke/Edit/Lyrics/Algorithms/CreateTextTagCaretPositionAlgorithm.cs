// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    /// <summary>
    /// User hover cursor or dragging to create default ruby/romaji text.
    /// </summary>
    public class CreateTextTagCaretPositionAlgorithm : TextTagCaretPositionAlgorithm<CreateTextTagCaretPosition>
    {
        public CreateTextTagCaretPositionAlgorithm(Lyric[] lyrics, EditArea editArea)
            : base(lyrics, editArea)
        {

        }

        public override bool PositionMovable(CreateTextTagCaretPosition position)
        {
            throw new System.NotImplementedException();
        }

        public override CreateTextTagCaretPosition MoveUp(CreateTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override CreateTextTagCaretPosition MoveDown(CreateTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override CreateTextTagCaretPosition MoveLeft(CreateTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override CreateTextTagCaretPosition MoveRight(CreateTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override CreateTextTagCaretPosition MoveToFirst()
        {
            throw new System.NotImplementedException();
        }

        public override CreateTextTagCaretPosition MoveToLast()
        {
            throw new System.NotImplementedException();
        }
    }
}
