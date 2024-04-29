// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;

[Cached(typeof(IEditableLyricState))]
public partial class EditableLyric : InteractableLyric, IEditableLyricState
{
    [Resolved]
    private ILyricsChangeHandler lyricsChangeHandler { get; set; } = null!;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    public EditableLyric(Lyric lyric)
        : base(lyric)
    {
        CornerRadius = 5;
        Padding = new MarginPadding { Bottom = 10 };
    }

    protected override IEnumerable<BaseLayer> CreateLayers(Lyric lyric)
    {
        return new BaseLayer[]
        {
            new TimeTagLayer(lyric),
            new CaretLayer(lyric),
            new BlueprintLayer(lyric),
        };
    }

    public void TriggerDisallowEditEffect()
    {
        InternalChildren.OfType<BaseLayer>().ForEach(x => x.TriggerDisallowEditEffect(BindableMode.Value));
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

    private object? getCaretIndexByPosition(UIEvent mouseEvent)
    {
        var algorithm = lyricCaretState.CaretPositionAlgorithm;
        float xPosition = ToLocalSpace(mouseEvent.ScreenSpaceMousePosition).X;
        return algorithm switch
        {
            CuttingCaretPositionAlgorithm => KaraokeSpriteText.GetCharIndicatorByPosition(xPosition),
            TypingCaretPositionAlgorithm => KaraokeSpriteText.GetCharIndicatorByPosition(xPosition),
            NavigateCaretPositionAlgorithm => null,
            CreateRubyTagCaretPositionAlgorithm => KaraokeSpriteText.GetCharIndexByPosition(xPosition),
            CreateRemoveTimeTagCaretPositionAlgorithm => KaraokeSpriteText.GetCharIndexByPosition(xPosition),
            TimeTagCaretPositionAlgorithm => KaraokeSpriteText.GetTimeTagByPosition(xPosition),
            _ => null,
        };
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
}
