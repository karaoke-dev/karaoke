// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public class StageElementRunner : StageRunner, IStageElementRunner
{
    private IStageElementProvider? elementProvider;
    private IList<IApplicableToStageElement>? stageMods;
    private Container? elementContainer;

    public override void OnStageInfoChanged(StageInfo stageInfo, bool scorable, IReadOnlyList<Mod> mods)
    {
        elementProvider = stageInfo.CreateStageElementProvider(scorable);
        stageMods = mods.OfType<IApplicableToStageElement>().Where(x => x.CanApply(stageInfo)).ToList();
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
        if (elementContainer == null)
            return;

        elementContainer.Clear();

        foreach (var element in getCommand())
            elementContainer.Add(element.CreateDrawable());
    }

    private IEnumerable<IStageElement> getCommand()
    {
        if (elementProvider == null)
            return Array.Empty<IStageElement>();

        var commands = elementProvider.GetElements();

        if (stageMods == null)
            return commands;

        return stageMods.Aggregate(commands, (current, mod) => mod.PostProcess(current));
    }
}
