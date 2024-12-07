// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Mods;

public interface IApplicableToStagePlayfieldCommand : IApplicableToStage
{
    IEnumerable<IStageCommand> PostProcessCommands(Playfield playfield, IEnumerable<IStageCommand> commands);
}
