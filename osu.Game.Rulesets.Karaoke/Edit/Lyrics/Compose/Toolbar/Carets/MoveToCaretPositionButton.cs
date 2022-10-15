// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar.Carets
{
    public abstract class MoveToCaretPositionButton : KeyActionButton
    {
        protected abstract MovingCaretAction AcceptAction { get; }

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();

        protected MoveToCaretPositionButton()
        {
            Action = () =>
            {
                lyricCaretState.MoveCaret(AcceptAction);
            };

            bindableCaretPosition.BindValueChanged(e =>
            {
                bool movable = lyricCaretState.GetCaretPositionByAction(AcceptAction) != null;
                SetState(movable);
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
        }
    }
}
