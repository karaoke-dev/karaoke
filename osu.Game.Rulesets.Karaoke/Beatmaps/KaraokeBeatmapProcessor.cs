// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects.Types;
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
                _ => throw new InvalidCastException($"The beatmap is not a {nameof(KaraokeBeatmap)}"),
            };
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
