// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

/// <summary>
/// Compare to <see cref="InteractLyricLayer"/>, this layer can do more things:
/// 1. Change the hover caret and index.
/// 2. Change the caret and index.
/// 3. Change the drag caret and index.
/// 4. Do some special action like cut the lyric by double click.
/// </summary>
public partial class EditLyricLayer : UIEventLayer
{
    [Resolved]
    private IPreviewLyricPositionProvider previewLyricPositionProvider { get; set; } = null!;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    [Resolved]
    private ILyricsChangeHandler lyricsChangeHandler { get; set; } = null!;

    public EditLyricLayer(Lyric lyric)
        : base(lyric)
    {
    }

    #region Hover

    protected override bool OnMouseMove(MouseMoveEvent e)
    {
        if (!lyricCaretState.CaretEnabled)
            return false;

        // should not trigger the hover caret event if dragging other lyric.
        if (e.HasAnyButtonPressed)
            return false;

        object? caretIndex = getCaretIndexByPosition(e);

        if (caretIndex != null)
        {
            lyricCaretState.MoveHoverCaretToTargetPosition(Lyric, caretIndex);
        }
        else if (lyricCaretState.CaretPosition is not IIndexCaretPosition)
        {
            // still need to handle the case with non-index caret position.
            lyricCaretState.MoveHoverCaretToTargetPosition(Lyric);
        }

        return base.OnMouseMove(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        base.OnHoverLost(e);

        if (!lyricCaretState.CaretEnabled)
            return;

        // lost hover caret and time-tag caret
        lyricCaretState.ClearHoverCaretPosition();
    }

    #endregion

    #region Click

    protected override bool OnClick(ClickEvent e)
    {
        return lyricCaretState.ConfirmHoverCaretPosition();
    }

    #endregion

    #region Drag

    protected override bool OnDragStart(DragStartEvent e)
    {
        // should not handle the drag event if the caret algorithm is able to handle it.
        if (!lyricCaretState.CaretDraggable)
            return false;

        // confirm the hover caret position before drag start.
        return lyricCaretState.StartDragging();
    }

    protected override void OnDrag(DragEvent e)
    {
        object? caretIndex = getCaretIndexByPosition(e);

        if (caretIndex != null)
        {
            lyricCaretState.MoveDraggingCaretIndex(caretIndex);
        }

        base.OnDrag(e);
    }

    protected override void OnDragEnd(DragEndEvent e)
    {
        lyricCaretState.EndDragging();

        base.OnDragEnd(e);
    }

    #endregion

    private object? getCaretIndexByPosition(UIEvent mouseEvent)
    {
        var algorithm = lyricCaretState.CaretPositionAlgorithm;
        float xPosition = ToLocalSpace(mouseEvent.ScreenSpaceMousePosition).X;
        return algorithm switch
        {
            CuttingCaretPositionAlgorithm => previewLyricPositionProvider.GetCharIndicatorByPosition(xPosition),
            TypingCaretPositionAlgorithm => previewLyricPositionProvider.GetCharIndicatorByPosition(xPosition),
            NavigateCaretPositionAlgorithm => null,
            CreateRubyTagCaretPositionAlgorithm => previewLyricPositionProvider.GetCharIndexByPosition(xPosition),
            CreateRemoveTimeTagCaretPositionAlgorithm => previewLyricPositionProvider.GetCharIndexByPosition(xPosition),
            _ => null,
        };
    }

    #region Double click

    protected override bool OnDoubleClick(DoubleClickEvent e)
    {
        var position = lyricCaretState.CaretPosition;

        switch (position)
        {
            case CuttingCaretPosition cuttingCaretPosition:
                lyricsChangeHandler.Split(cuttingCaretPosition.CharGap);
                return true;

            default:
                return false;
        }
    }

    #endregion
}
