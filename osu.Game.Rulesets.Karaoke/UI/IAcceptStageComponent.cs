// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.UI.Stages;

namespace osu.Game.Rulesets.Karaoke.UI;

public interface IAcceptStageComponent : ITransformable
{
    void Add(IStageComponent component);

    bool Remove(IStageComponent component);
}
