// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

/// <summary>
/// Specifies which properties in the <see cref="Note"/> are being invalidated.
/// </summary>
[Flags]
public enum NoteWorkingProperty
{
    /// <summary>
    /// <see cref="Note.PreemptTime"/> is being invalidated.
    /// </summary>
    PreemptTime = 1,

    /// <summary>
    /// <see cref="Note.PageIndex"/> is being invalidated.
    /// </summary>
    Page = 1 << 1,

    /// <summary>
    /// <see cref="Note.ReferenceLyric"/> is being invalidated.
    /// </summary>
    ReferenceLyric = 1 << 2,

    /// <summary>
    /// <see cref="Note.EffectApplier"/> is being invalidated.
    /// </summary>
    EffectApplier = 1 << 3,
}
