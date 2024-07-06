// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

[Flags]
public enum LyricEditorLayout
{
    /// <summary>
    /// Show the list of lyrics in the main content area.
    /// </summary>
    List = 1,

    /// <summary>
    /// Show the composer at the top of the main content area.
    /// User can select the edit lyric at the bottom of the compose area.
    /// </summary>
    Compose = 1 << 1,
}
