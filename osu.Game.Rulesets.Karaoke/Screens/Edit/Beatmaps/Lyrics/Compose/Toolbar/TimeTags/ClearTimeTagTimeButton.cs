// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar.TimeTags;

public partial class ClearTimeTagTimeButton : KeyActionButton
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
            if (lyricCaretState.CaretPosition is not TimeTagCaretPosition timeTagCaretPosition)
                throw new InvalidOperationException();

            var timeTag = timeTagCaretPosition.TimeTag;
            lyricTimeTagsChangeHandler.ClearTimeTagTime(timeTag);
        };
    }
}
