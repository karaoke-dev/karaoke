// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Mods;

public interface IApplicableToStageHitObjectCommand : IApplicableToStage
{
    IEnumerable<IStageCommand> PostProcessInitialCommands(HitObject hitObject, IEnumerable<IStageCommand> commands);

    IEnumerable<IStageCommand> PostProcessStartTimeStateCommands(HitObject hitObject, IEnumerable<IStageCommand> commands);

    IEnumerable<IStageCommand> PostProcessHitStateCommands(HitObject hitObject, IEnumerable<IStageCommand> commands);
}
