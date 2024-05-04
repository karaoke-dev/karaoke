// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class CreateTimeTagActionReceiver : Component, IKeyBindingHandler<KaraokeEditAction>
{
    [Resolved]
    private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    public bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e)
    {
        var action = e.Action;
        var caretPosition = lyricCaretState.CaretPosition;

        return caretPosition switch
        {
            CreateRemoveTimeTagCaretPosition timeTagIndexCaretPosition => processCreateTimeTagAction(timeTagIndexCaretPosition, action),
            TimeTagCaretPosition timeTagCaretPosition => processModifyTimeTagAction(timeTagCaretPosition, action),
            _ => throw new NotSupportedException(nameof(caretPosition)),
        };
    }

    private bool processCreateTimeTagAction(CreateRemoveTimeTagCaretPosition createRemoveTimeTagCaretPosition, KaraokeEditAction action)
    {
        int index = createRemoveTimeTagCaretPosition.CharIndex;

        switch (action)
        {
            case KaraokeEditAction.CreateStartTimeTag:
                lyricTimeTagsChangeHandler.AddByPosition(new TextIndex(index));
                return true;

            case KaraokeEditAction.CreateEndTimeTag:
                lyricTimeTagsChangeHandler.AddByPosition(new TextIndex(index, TextIndex.IndexState.End));
                return true;

            case KaraokeEditAction.RemoveStartTimeTag:
                lyricTimeTagsChangeHandler.RemoveByPosition(new TextIndex(index));
                return true;

            case KaraokeEditAction.RemoveEndTimeTag:
                lyricTimeTagsChangeHandler.RemoveByPosition(new TextIndex(index, TextIndex.IndexState.End));
                return true;

            default:
                return false;
        }
    }

    private bool processModifyTimeTagAction(TimeTagCaretPosition timeTagCaretPosition, KaraokeEditAction action)
    {
        // todo: modify time-tag might have it's own key.
        switch (action)
        {
            case KaraokeEditAction.CreateStartTimeTag:
            case KaraokeEditAction.CreateEndTimeTag:
                var index = timeTagCaretPosition.TimeTag.Index;
                lyricTimeTagsChangeHandler.AddByPosition(index);
                return true;

            case KaraokeEditAction.RemoveStartTimeTag:
            case KaraokeEditAction.RemoveEndTimeTag:
                var timeTag = timeTagCaretPosition.TimeTag;
                bool movable = lyricCaretState.MoveCaret(MovingCaretAction.PreviousIndex);
                lyricTimeTagsChangeHandler.Remove(timeTag);

                if (!movable)
                {
                    // Should make sure that hover to the first time-tag again if first time-tag has been removed.
                    lyricCaretState.MoveCaret(MovingCaretAction.FirstLyric);
                }

                return true;

            default:
                var tuple = getTupleByAction(action);
                if (tuple == null)
                    return false;

                var newTimeTag = lyricTimeTagsChangeHandler.Shifting(timeTagCaretPosition.TimeTag, tuple.Item1, tuple.Item2);
                lyricCaretState.MoveCaretToTargetPosition(timeTagCaretPosition.Lyric, newTimeTag);
                return true;
        }

        static Tuple<ShiftingDirection, ShiftingType>? getTupleByAction(KaraokeEditAction action) =>
            action switch
            {
                KaraokeEditAction.ShiftTheTimeTagLeft => new Tuple<ShiftingDirection, ShiftingType>(ShiftingDirection.Left, ShiftingType.Index),
                KaraokeEditAction.ShiftTheTimeTagRight => new Tuple<ShiftingDirection, ShiftingType>(ShiftingDirection.Right, ShiftingType.Index),
                KaraokeEditAction.ShiftTheTimeTagStateLeft => new Tuple<ShiftingDirection, ShiftingType>(ShiftingDirection.Left, ShiftingType.State),
                KaraokeEditAction.ShiftTheTimeTagStateRight => new Tuple<ShiftingDirection, ShiftingType>(ShiftingDirection.Right, ShiftingType.State),
                _ => null,
            };
    }

    public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
    {
    }
}
