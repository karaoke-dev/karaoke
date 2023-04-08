// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Patterns;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Types;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Beatmaps
{
    public class KaraokeBeatmapProcessor : BeatmapProcessor
    {
        public new KaraokeBeatmap Beatmap => (KaraokeBeatmap)base.Beatmap;

        public KaraokeBeatmapProcessor(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        public override void PreProcess()
        {
            applyStage(Beatmap);

            base.PreProcess();
            applyInvalidProperty(Beatmap);
        }

        private void applyStage(KaraokeBeatmap beatmap)
        {
            // current stage info will be null if not select any mod or first load.
            // trying to load the first stage or create a default one.
            beatmap.CurrentStageInfo ??= getWorkingStage() ?? createDefaultWorkingStage();

            if (beatmap.CurrentStageInfo is IHasCalculatedProperty calculatedProperty)
                calculatedProperty.ValidateCalculatedProperty(beatmap);

            StageInfo? getWorkingStage()
                => Beatmap.StageInfos.FirstOrDefault();

            StageInfo createDefaultWorkingStage() => new PreviewStageInfo();
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

        public override void PostProcess()
        {
            base.PostProcess();

            var lyrics = Beatmap.HitObjects.OfType<Lyric>().ToList();

            if (!lyrics.Any())
                return;

            var pattern = new LegacyLyricTimeGenerator();
            pattern.Generate(lyrics);
        }
    }
}
