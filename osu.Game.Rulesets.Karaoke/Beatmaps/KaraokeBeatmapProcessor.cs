// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Stages.Types;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Beatmaps;

public class KaraokeBeatmapProcessor : BeatmapProcessor
{
    public KaraokeBeatmapProcessor(IBeatmap beatmap)
        : base(beatmap)
    {
    }

    public override void PreProcess()
    {
        var karaokeBeatmap = getKaraokeBeatmap(Beatmap);
        applyStage(karaokeBeatmap);

        base.PreProcess();
        applyInvalidProperty(karaokeBeatmap);
        return;

        static KaraokeBeatmap getKaraokeBeatmap(IBeatmap beatmap) =>
            beatmap switch
            {
                // goes to there while parsing the beatmap.
                KaraokeBeatmap karaokeBeatmap => karaokeBeatmap,
                // goes to there while editing the beatmap.
                EditorBeatmap editorBeatmap => getKaraokeBeatmap(editorBeatmap.PlayableBeatmap),
                _ => throw new InvalidCastException($"The beatmap is not a {nameof(KaraokeBeatmap)}")
            };
    }

    private void applyStage(KaraokeBeatmap beatmap)
    {
        // current stage info will be null if not select any mod or first load.
        // trying to load the first stage or create a default one.
        if (beatmap.CurrentStageInfo == null)
        {
            beatmap.CurrentStageInfo = getWorkingStage(beatmap) ?? createDefaultWorkingStage();

            // should invalidate the working property here because the stage info is changed.
            beatmap.HitObjects.OfType<Lyric>().ForEach(x =>
            {
                x.InvalidateWorkingProperty(LyricWorkingProperty.Timing);
                x.InvalidateWorkingProperty(LyricWorkingProperty.EffectApplier);
            });
            beatmap.HitObjects.OfType<Note>().ForEach(x => x.InvalidateWorkingProperty(NoteWorkingProperty.EffectApplier));
        }

        if (beatmap.CurrentStageInfo is IHasCalculatedProperty calculatedProperty)
            calculatedProperty.ValidateCalculatedProperty(beatmap);

        static StageInfo? getWorkingStage(KaraokeBeatmap beatmap)
            => beatmap.StageInfos.FirstOrDefault();

        static StageInfo createDefaultWorkingStage() => new PreviewStageInfo();
    }

    private void applyInvalidProperty(KaraokeBeatmap beatmap)
    {
        // should convert to array here because validate the working property might change the start-time and the end time.
        // which will cause got the wrong item in the array.
        foreach (var hitObject in beatmap.HitObjects.OfType<IHasWorkingProperty>().ToArray())
        {
            hitObject.ValidateWorkingProperty(beatmap);
        }
    }
}
