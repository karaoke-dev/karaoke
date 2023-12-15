// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

/// <summary>
/// Specifies which properties in the <see cref="Note"/> are being invalidated.
/// </summary>
[Flags]
public enum NoteStageWorkingProperty
{
    /// <summary>
    /// <see cref="Note.EffectApplier"/> is being invalidated.
    /// </summary>
    EffectApplier = 1,
}
