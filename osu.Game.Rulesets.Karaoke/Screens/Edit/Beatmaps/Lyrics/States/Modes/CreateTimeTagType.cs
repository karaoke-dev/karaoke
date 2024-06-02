// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public enum CreateTimeTagType
{
    /// <summary>
    /// Use mouse to move the caret, and click the button in the UI to create/remove the start/end time tag.
    /// It's the slowest way to create the time tag.
    /// </summary>
    Mouse,

    /// <summary>
    /// Press the hotkey to prepare create/remove the start/end time tag, click the character in the lyric to confirm.
    /// It might be useful for those english-like lyric.
    /// </summary>
    HotkeyThenPress,

    /// <summary>
    /// Use keyboard to move the caret, and press hotkey to create/remove the start/end time tag.
    /// It's the fastest way to create the time tag for Japanese/Chinses lyric.
    /// </summary>
    Keyboard,
}
