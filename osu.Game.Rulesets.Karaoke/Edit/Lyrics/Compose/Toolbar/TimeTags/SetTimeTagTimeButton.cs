// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar.TimeTags
{
    public class SetTimeTagTimeButton : KeyActionButton
    {
        protected override KaraokeEditAction EditAction => KaraokeEditAction.SetTime;

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        [Resolved, AllowNull]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        [Resolved, AllowNull]
        private EditorClock editorClock { get; set; }

        public SetTimeTagTimeButton()
        {
            SetIcon(FontAwesome.Solid.Stopwatch);

            Action = () =>
            {
                if (lyricCaretState.BindableCaretPosition.Value is not TimeTagCaretPosition timeTagCaretPosition)
                    throw new InvalidOperationException();

                var timeTag = timeTagCaretPosition.TimeTag;
                double currentTime = editorClock.CurrentTime;
                lyricTimeTagsChangeHandler.SetTimeTagTime(timeTag, currentTime);
            };
        }
    }
}
