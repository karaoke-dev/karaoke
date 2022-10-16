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
    public class RemoveTimeTagButton : KeyActionButton
    {
        protected override KaraokeEditAction EditAction => KaraokeEditAction.RemoveTimeTag;

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        [Resolved, AllowNull]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        public RemoveTimeTagButton()
        {
            SetIcon(FontAwesome.Solid.Eraser);

            Action = () =>
            {
                if (lyricCaretState.BindableCaretPosition.Value is not TimeTagIndexCaretPosition timeTagIndexCaretPosition)
                    throw new InvalidOperationException();

                var index = timeTagIndexCaretPosition.Index;
                lyricTimeTagsChangeHandler.RemoveByPosition(index);
            };
        }
    }
}
