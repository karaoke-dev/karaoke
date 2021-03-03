// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    /// <summary>
    /// User is typing ruby/romaji text.
    /// </summary>
    public class TypingTextTagCaretPositionAlgorithm : TextTagCaretPositionAlgorithm<TypingTextTagCaretPosition>
    {
        public TypingTextTagCaretPositionAlgorithm(Lyric[] lyrics, EditArea editArea)
            : base(lyrics, editArea)
        {

        }

        public override bool PositionMovable(TypingTextTagCaretPosition position)
        {
            throw new System.NotImplementedException();
        }

        public override TypingTextTagCaretPosition MoveUp(TypingTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override TypingTextTagCaretPosition MoveDown(TypingTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override TypingTextTagCaretPosition MoveLeft(TypingTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override TypingTextTagCaretPosition MoveRight(TypingTextTagCaretPosition currentPosition)
        {
            throw new System.NotImplementedException();
        }

        public override TypingTextTagCaretPosition MoveToFirst()
        {
            throw new System.NotImplementedException();
        }

        public override TypingTextTagCaretPosition MoveToLast()
        {
            throw new System.NotImplementedException();
        }
    }
}
