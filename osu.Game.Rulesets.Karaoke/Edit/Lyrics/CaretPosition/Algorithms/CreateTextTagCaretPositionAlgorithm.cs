// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    /// <summary>
    /// User hover cursor or dragging to create default ruby/romaji text.
    /// </summary>
    public class CreateTextTagCaretPositionAlgorithm : TextTagCaretPositionAlgorithm<CreateTextTagCaretPosition>
    {
        public CreateTextTagCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(CreateTextTagCaretPosition position)
        {
            // only check type is ok then let it pass.
            if (!IsTextTagTypeValid(position))
                return false;

            // need to check is start and end index in the range
            var text = position.Lyric.Text;
            if (string.IsNullOrEmpty(text))
                return false;

            return position.TextTag.StartIndex >= 0 && position.TextTag.EndIndex <= text.Length;
        }

        public override CreateTextTagCaretPosition MoveUp(CreateTextTagCaretPosition currentPosition)
        {
            // It's tricky to drag create area and moving by keyboard at the same time.
            return null;
        }

        public override CreateTextTagCaretPosition MoveDown(CreateTextTagCaretPosition currentPosition)
        {
            // It's tricky to drag create area and moving by keyboard at the same time.
            return null;
        }

        public override CreateTextTagCaretPosition MoveLeft(CreateTextTagCaretPosition currentPosition)
        {
            // It's tricky to drag create area and moving by keyboard at the same time.
            return null;
        }

        public override CreateTextTagCaretPosition MoveRight(CreateTextTagCaretPosition currentPosition)
        {
            // It's tricky to drag create area and moving by keyboard at the same time.
            return null;
        }

        public override CreateTextTagCaretPosition MoveToFirst()
        {
            // Of course it's not have move to first feature.
            return null;
        }

        public override CreateTextTagCaretPosition MoveToLast()
        {
            // Of course it's not have move to last feature.
            return null;
        }

        public override CreateTextTagCaretPosition MoveToTarget(Lyric lyric)
        {
            // lazy to implement this algorithm because this algorithm haven't being used.
            return null;
        }
    }
}
