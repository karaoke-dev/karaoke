// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar
{
    public abstract class MoveCaretPositionButton : KeyActionButton
    {
        protected abstract MovingCaretAction AcceptAction { get; }

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();

        protected MoveCaretPositionButton()
        {
            Action = () =>
            {
                lyricCaretState.MoveCaret(AcceptAction);
            };

            bindableCaretPosition.BindValueChanged(e =>
            {
                if (!ValueChangedEventUtils.LyricChanged(e))
                    return;

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
