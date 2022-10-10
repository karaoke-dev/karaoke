// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.TimeTags
{
    public class CreateTimeTagActionReceiver : Component, IKeyBindingHandler<KaraokeEditAction>
    {
        [Resolved]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        public bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e)
        {
            var action = e.Action;
            var caretPosition = lyricCaretState.BindableCaretPosition.Value;

            return caretPosition switch
            {
                TimeTagIndexCaretPosition timeTagIndexCaretPosition => processCreateTimeTagAction(timeTagIndexCaretPosition, action),
                TimeTagCaretPosition timeTagCaretPosition => processModifyTimeTagAction(timeTagCaretPosition, action),
                _ => throw new NotSupportedException(nameof(caretPosition))
            };
        }

        private bool processCreateTimeTagAction(TimeTagIndexCaretPosition timeTagIndexCaretPosition, KaraokeEditAction action)
        {
            var index = timeTagIndexCaretPosition.Index;

            switch (action)
            {
                case KaraokeEditAction.CreateTimeTag:
                    lyricTimeTagsChangeHandler.AddByPosition(index);
                    return true;

                case KaraokeEditAction.RemoveTimeTag:
                    lyricTimeTagsChangeHandler.RemoveByPosition(index);
                    return true;

                default:
                    return false;
            }
        }

        private bool processModifyTimeTagAction(TimeTagCaretPosition timeTagCaretPosition, KaraokeEditAction action)
        {
            switch (action)
            {
                case KaraokeEditAction.CreateTimeTag:
                    var index = timeTagCaretPosition.TimeTag.Index;
                    lyricTimeTagsChangeHandler.AddByPosition(index);
                    return true;

                case KaraokeEditAction.RemoveTimeTag:
                    var timeTag = timeTagCaretPosition.TimeTag;
                    bool movable = lyricCaretState.MoveCaret(MovingCaretAction.Left);
                    lyricTimeTagsChangeHandler.Remove(timeTag);

                    if (!movable)
                    {
                        // Should make sure that hover to the first time-tag again if first time-tag has been removed.
                        lyricCaretState.MoveCaret(MovingCaretAction.First);
                    }

                    return true;

                default:
                    var tuple = getTupleByAction(action);
                    if (tuple == null)
                        return false;

                    var newTimeTag = lyricTimeTagsChangeHandler.Shifting(timeTagCaretPosition.TimeTag, tuple.Item1, tuple.Item2);
                    lyricCaretState.MoveCaretToTargetPosition(new TimeTagCaretPosition(timeTagCaretPosition.Lyric, newTimeTag));
                    return true;
            }

            static Tuple<ShiftingDirection, ShiftingType> getTupleByAction(KaraokeEditAction action) =>
                action switch
                {
                    KaraokeEditAction.ShiftTheTimeTagLeft => new Tuple<ShiftingDirection, ShiftingType>(ShiftingDirection.Left, ShiftingType.Index),
                    KaraokeEditAction.ShiftTheTimeTagRight => new Tuple<ShiftingDirection, ShiftingType>(ShiftingDirection.Right, ShiftingType.Index),
                    KaraokeEditAction.ShiftTheTimeTagStateLeft => new Tuple<ShiftingDirection, ShiftingType>(ShiftingDirection.Left, ShiftingType.State),
                    KaraokeEditAction.ShiftTheTimeTagStateRight => new Tuple<ShiftingDirection, ShiftingType>(ShiftingDirection.Right, ShiftingType.State),
                    _ => null
                };
        }

        public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
        {
        }
    }
}
