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

    protected override bool OnMouseMove(MouseMoveEvent e)
    {
        if (!lyricCaretState.CaretEnabled)
            return false;

        // should not trigger the hover caret event if dragging other lyric.
        if (e.HasAnyButtonPressed)
            return false;

        float xPosition = ToLocalSpace(e.ScreenSpaceMousePosition).X;
        object? caretIndex = getCaretIndexByPosition(xPosition);

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

    protected override bool OnDragStart(DragStartEvent e)
    {
        // should not handle the drag event if the caret algorithm is able to handle it.
        if (!lyricCaretState.CaretDraggable)
            return false;

        // confirm the hover caret position before drag start.
        return lyricCaretState.ConfirmHoverCaretPosition();
    }

    protected override void OnDrag(DragEvent e)
    {
        if (!lyricCaretState.CaretEnabled)
            return;

        float xPosition = ToLocalSpace(e.ScreenSpaceMousePosition).X;
        object? caretIndex = getCaretIndexByPosition(xPosition);

        if (caretIndex != null)
        {
            lyricCaretState.MoveDraggingCaretIndex(caretIndex);
        }

        base.OnDrag(e);
    }

    private object? getCaretIndexByPosition(float position) =>
        lyricCaretState.CaretPosition switch
        {
            CuttingCaretPosition => KaraokeSpriteText.GetCharIndicatorByPosition(position),
            TypingCaretPosition => KaraokeSpriteText.GetCharIndicatorByPosition(position),
            NavigateCaretPosition => null,
            CreateRubyTagCaretPosition => KaraokeSpriteText.GetCharIndexByPosition(position),
            TimeTagIndexCaretPosition => KaraokeSpriteText.GetCharIndexByPosition(position),
            TimeTagCaretPosition => KaraokeSpriteText.GetTimeTagByPosition(position),
            _ => null,
        };

    protected override void OnHoverLost(HoverLostEvent e)
    {
        base.OnHoverLost(e);

        if (!lyricCaretState.CaretEnabled)
            return;

        // lost hover caret and time-tag caret
        lyricCaretState.ClearHoverCaretPosition();
    }

    protected override bool OnClick(ClickEvent e)
    {
        return lyricCaretState.ConfirmHoverCaretPosition();
    }

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
