// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Mods;

public interface IApplicableToStageElement : IApplicableToStage
{
    IEnumerable<IStageElement> PostProcess(IEnumerable<IStageElement> stageElements);
}
