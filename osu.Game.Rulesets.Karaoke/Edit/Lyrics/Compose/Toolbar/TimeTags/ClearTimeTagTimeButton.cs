// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar.TimeTags
{
    public class ClearTimeTagTimeButton : KeyActionButton
    {
        protected override KaraokeEditAction EditAction => KaraokeEditAction.ClearTime;

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        [Resolved, AllowNull]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        public ClearTimeTagTimeButton()
        {
            SetIcon(FontAwesome.Solid.Eraser);

            Action = () =>
            {
                if (lyricCaretState.BindableCaretPosition.Value is not TimeTagCaretPosition timeTagCaretPosition)
                    throw new InvalidOperationException();

                var timeTag = timeTagCaretPosition.TimeTag;
                lyricTimeTagsChangeHandler.ClearTimeTagTime(timeTag);
            };
        }
    }
}
