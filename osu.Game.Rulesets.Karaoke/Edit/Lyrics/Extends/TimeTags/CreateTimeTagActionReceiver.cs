// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
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

            if (caretPosition is TimeTagIndexCaretPosition timeTagIndexCaretPosition)
                return processCreateTimeTagAction(timeTagIndexCaretPosition, action);

            if (caretPosition is TimeTagCaretPosition timeTagCaretPosition)
                return processModifyTimeTagAction(timeTagCaretPosition, action);

            throw new NotSupportedException(nameof(caretPosition));
        }

        private bool processCreateTimeTagAction(TimeTagIndexCaretPosition timeTagIndexCaretPosition, KaraokeEditAction action)
        {
            var index = timeTagIndexCaretPosition.Index;

            switch (action)
            {
                case KaraokeEditAction.Create:
                    lyricTimeTagsChangeHandler.AddByPosition(index);
                    return true;

                case KaraokeEditAction.Remove:
                    lyricTimeTagsChangeHandler.RemoveByPosition(index);
                    return true;

                default:
                    return false;
            }
        }

        private bool processModifyTimeTagAction(TimeTagCaretPosition timeTagCaretPosition, KaraokeEditAction action)
        {
            // todo: implement the action.
            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
        {
        }
    }
}
