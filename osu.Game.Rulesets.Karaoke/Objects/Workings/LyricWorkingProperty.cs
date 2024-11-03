// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

/// <summary>
/// Specifies which properties in the <see cref="Lyric"/> are being invalidated.
/// </summary>
[Flags]
public enum LyricWorkingProperty
{
    /// <summary>
    /// <see cref="Lyric.Singers"/> is being invalidated.
    /// </summary>
    Singers = 1 << 0,

    /// <summary>
    /// <see cref="Lyric.PageIndex"/> is being invalidated.
    /// </summary>
    Page = 1 << 1,

    /// <summary>
    /// <see cref="Lyric.ReferenceLyric"/> is being invalidated.
    /// </summary>
    ReferenceLyric = 1 << 2,

    /// <summary>
    /// <see cref="Lyric.CommandGenerator"/> is being invalidated.
    /// </summary>
    CommandGenerator = 1 << 3,
}
