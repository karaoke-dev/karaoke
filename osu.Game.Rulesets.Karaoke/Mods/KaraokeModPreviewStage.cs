// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;

namespace osu.Game.Rulesets.Karaoke.Mods;

public class KaraokeModPreviewStage : ModStage<PreviewStageInfo>
{
    public override string Name => "Preview stage";

    public override string Acronym => "PS";

    public override LocalisableString Description => "Focus on preview the lyric text.";

    protected override void ApplyToCurrentStageInfo(PreviewStageInfo stageInfo)
    {
        throw new System.NotImplementedException();
    }

    protected override PreviewStageInfo CreateStageInfo(IBeatmap beatmap) => new();
}
