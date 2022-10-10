// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar
{
    public abstract class MoveCaretPositionButton : KeyActionButton
    {
        protected abstract MovingCaretAction AcceptAction { get; }

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        protected MoveCaretPositionButton()
        {
            Action = () =>
            {
                lyricCaretState.MoveCaret(AcceptAction);
            };
        }
    }
}
