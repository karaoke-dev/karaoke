// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public class StageElementRunner : IStageElementRunner
{
    private IStageElementProvider? elementProvider;
    private Container? elementContainer;

    public void UpdateCommandGenerator(IStageElementProvider provider)
    {
        elementProvider = provider;
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
