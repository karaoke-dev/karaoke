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
    /// <see cref="Lyric.PreemptTime"/> is being invalidated.
    /// </summary>
    PreemptTime = 1,

    /// <summary>
    /// <see cref="Lyric.StartTime"/> is being invalidated.
    /// </summary>
    StartTime = 1 << 1,

    /// <summary>
    /// <see cref="Lyric.Duration"/> is being invalidated.
    /// </summary>
    Duration = 1 << 2,

    /// <summary>
    /// <see cref="Lyric.StartTime"/> and <see cref="Lyric.Duration"/> is being invalidated.
    /// </summary>
    Timing = PreemptTime | StartTime | Duration,

    /// <summary>
    /// <see cref="Lyric.Singers"/> is being invalidated.
    /// </summary>
    Singers = 1 << 3,

    /// <summary>
    /// <see cref="Lyric.PageIndex"/> is being invalidated.
    /// </summary>
    Page = 1 << 4,

    /// <summary>
    /// <see cref="Lyric.ReferenceLyric"/> is being invalidated.
    /// </summary>
    ReferenceLyric = 1 << 5,

    /// <summary>
    /// <see cref="Lyric.EffectApplier"/> is being invalidated.
    /// </summary>
    EffectApplier = 1 << 6,
}
