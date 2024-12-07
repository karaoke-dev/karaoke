// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Stages.Infos;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Stages;

public abstract partial class BaseStageInfoChangeHandlerTest<TChangeHandler> : BaseChangeHandlerTest<TChangeHandler>
    where TChangeHandler : Component
{
    protected virtual void SetUpStageInfo<TStageInfo>(Action<TStageInfo>? action = null)
        => throw new NotImplementedException();

    public void AssertStageInfos(Action<IList<StageInfo>> assert)
        => throw new NotImplementedException();

    public void AssertStageInfo<TStageInfo>(Action<TStageInfo> assert) where TStageInfo : StageInfo
        => throw new NotImplementedException();
}
