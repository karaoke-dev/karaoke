// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public interface ITimeTagModeState : IHasBlueprintSelection<TimeTag>, IHasEditModeState<TimeTagEditMode>
    {
        BindableFloat BindableRecordZoom { get; }

        BindableFloat BindableAdjustZoom { get; }
    }
}
