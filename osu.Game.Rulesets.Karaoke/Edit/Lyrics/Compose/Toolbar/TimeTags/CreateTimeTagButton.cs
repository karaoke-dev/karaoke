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
    public class CreateTimeTagButton : KeyActionButton
    {
        protected override KaraokeEditAction EditAction => KaraokeEditAction.CreateTimeTag;

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        [Resolved, AllowNull]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        public CreateTimeTagButton()
        {
            SetIcon(FontAwesome.Solid.Tag);

            Action = () =>
            {
                if (lyricCaretState.BindableCaretPosition.Value is not TimeTagIndexCaretPosition timeTagIndexCaretPosition)
                    throw new InvalidOperationException();

                var index = timeTagIndexCaretPosition.Index;
                lyricTimeTagsChangeHandler.AddByPosition(index);
            };
        }
    }
}
