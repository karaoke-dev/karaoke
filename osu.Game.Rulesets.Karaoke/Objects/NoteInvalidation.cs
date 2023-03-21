// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects;

/// <summary>
/// Specifies which properties in the <see cref="Note"/> are being invalidated.
/// </summary>
[Flags]
public enum NoteInvalidation
{
    /// <summary>
    /// <see cref="Note.PageIndex"/> is being invalidated.
    /// </summary>
    Page = 1,

    /// <summary>
    /// <see cref="Note.ReferenceLyric"/> is being invalidated.
    /// </summary>
    ReferenceLyric = 1 << 1,
}
