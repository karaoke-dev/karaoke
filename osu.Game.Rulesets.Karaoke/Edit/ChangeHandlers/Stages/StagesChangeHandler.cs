// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;

public partial class StagesChangeHandler : StagePropertyChangeHandler, IStagesChangeHandler
{
    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    private KaraokeBeatmap karaokeBeatmap => EditorBeatmapUtils.GetPlayableBeatmap(beatmap);

    [Resolved]
    private KaraokeRulesetEditGeneratorConfigManager generatorConfigManager { get; set; } = null!;

    bool IAutoGenerateChangeHandler<StageInfo>.CanGenerate<TStageInfo>()
        => CanGenerate<TStageInfo>();

    public bool CanGenerate<TStageInfo>() where TStageInfo : StageInfo
    {
        return GetGeneratorNotSupportedMessage<TStageInfo>() == null;
    }

    public LocalisableString? GetGeneratorNotSupportedMessage<TStageInfo>() where TStageInfo : StageInfo
    {
        var stage = getStageInfo<TStageInfo>();
        if (stage != null)
            return $"{nameof(TStageInfo)} already exist in the beatmap.";

        var generator = new StageInfoGeneratorSelector<TStageInfo>(generatorConfigManager);
        return generator.GetInvalidMessage(karaokeBeatmap);
    }

    void IAutoGenerateChangeHandler<StageInfo>.AutoGenerate<TStageInfo>()
        => AutoGenerate<TStageInfo>();

    public void AutoGenerate<TStageInfo>() where TStageInfo : StageInfo
    {
        var stage = getStageInfo<TStageInfo>();
        if (stage != null)
            throw new InvalidOperationException($"{nameof(TStageInfo)} already exist in the beatmap.");

        var generator = new StageInfoGeneratorSelector<TStageInfo>(generatorConfigManager);
        var stageInfo = generator.Generate(karaokeBeatmap);

        getStageInfos().Add(stageInfo);
    }

    public void Remove<TStageInfo>() where TStageInfo : StageInfo
    {
        var stage = getStageInfo<TStageInfo>();
        if (stage == null)
            throw new InvalidOperationException($"There's no {nameof(TStageInfo)} in the beatmap.");

        getStageInfos().Remove(stage);

        // todo: maybe should update the current stage.
    }

    private IList<StageInfo> getStageInfos()
        => throw new NotImplementedException();

    private TStageInfo? getStageInfo<TStageInfo>() where TStageInfo : StageInfo
        => throw new NotImplementedException();
}
