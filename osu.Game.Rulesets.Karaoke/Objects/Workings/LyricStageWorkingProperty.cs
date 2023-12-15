// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

/// <summary>
/// Specifies which properties in the <see cref="Lyric"/> are being invalidated.
/// </summary>
[Flags]
public enum LyricStageWorkingProperty
{
    /// <summary>
    /// <see cref="Lyric.StartTime"/> is being invalidated.
    /// </summary>
    StartTime = 1,

    /// <summary>
    /// <see cref="Lyric.Duration"/> is being invalidated.
    /// </summary>
    Duration = 1 << 1,

    /// <summary>
    /// <see cref="Lyric.StartTime"/> and <see cref="Lyric.Duration"/> is being invalidated.
    /// </summary>
    Timing = StartTime | Duration,

    /// <summary>
    /// <see cref="Lyric.EffectApplier"/> is being invalidated.
    /// </summary>
    EffectApplier = 1 << 2,
}
