// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar.Carets;

public abstract partial class MoveToCaretPositionButton : ToolbarEditActionButton
{
    protected abstract MovingCaretAction AcceptAction { get; }

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

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
