// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public class StageElementRunner : StageRunner, IStageElementRunner
{
    private IStageElementProvider? elementProvider;
    private Container? elementContainer;

    public override void OnStageInfoChanged(StageInfo stageInfo, bool scorable, IReadOnlyList<Mod> mods)
    {
        elementProvider = stageInfo.CreateStageElementProvider(scorable);
        applyTransforms();
    }

    public override void TriggerUpdateCommand()
    {
        applyTransforms();
    }

    public void UpdateStageElements(Container container)
    {
        elementContainer = container;
        applyTransforms();
    }

    private void applyTransforms()
    {
        if (elementProvider == null || elementContainer == null)
            return;

        elementContainer.Clear();

        foreach (var element in elementProvider.GetElements())
            elementContainer.Add(element.CreateDrawable());
    }
}
