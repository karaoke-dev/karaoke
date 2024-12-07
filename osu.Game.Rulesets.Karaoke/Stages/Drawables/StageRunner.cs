// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public abstract partial class StageRunner
{
    public abstract void OnStageInfoChanged(StageInfo stageInfo, bool scorable, IReadOnlyList<Mod> mods);

    public abstract void TriggerUpdateCommand();
}
