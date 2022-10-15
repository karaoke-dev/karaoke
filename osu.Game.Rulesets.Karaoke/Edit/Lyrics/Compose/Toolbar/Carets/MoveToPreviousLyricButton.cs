// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar.Carets
{
    public class MoveToPreviousLyricButton : MoveCaretPositionButton
    {
        protected override KaraokeEditAction EditAction => KaraokeEditAction.MoveUp;

        protected override MovingCaretAction AcceptAction => MovingCaretAction.PreviousLyric;

        public MoveToPreviousLyricButton()
        {
            SetIcon(FontAwesome.Solid.ArrowUp);
        }
    }
}
