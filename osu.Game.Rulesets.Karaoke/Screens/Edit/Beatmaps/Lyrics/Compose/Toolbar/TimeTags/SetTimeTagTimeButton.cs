// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar.TimeTags;

public partial class SetTimeTagTimeButton : KeyActionButton
{
    protected override KaraokeEditAction EditAction => KaraokeEditAction.SetTime;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    [Resolved]
    private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

    [Resolved]
    private EditorClock editorClock { get; set; } = null!;

    public SetTimeTagTimeButton()
    {
        SetIcon(FontAwesome.Solid.Stopwatch);

        Action = () =>
        {
            if (lyricCaretState.CaretPosition is not TimeTagCaretPosition timeTagCaretPosition)
                throw new InvalidOperationException();

            var timeTag = timeTagCaretPosition.TimeTag;
            double currentTime = editorClock.CurrentTime;
            lyricTimeTagsChangeHandler.SetTimeTagTime(timeTag, currentTime);
        };
    }
}
