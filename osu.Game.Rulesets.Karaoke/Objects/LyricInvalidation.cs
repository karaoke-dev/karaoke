// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects;

/// <summary>
/// Specifies which properties in the <see cref="Lyric"/> are being invalidated.
/// </summary>
[Flags]
public enum LyricInvalidation
{
    /// <summary>
    /// <see cref="Lyric.PageIndex"/> is being invalidated.
    /// </summary>
    Page = 1,

    /// <summary>
    /// <see cref="Lyric.ReferenceLyric"/> is being invalidated.
    /// </summary>
    ReferenceLyric = 1 << 1,
}
