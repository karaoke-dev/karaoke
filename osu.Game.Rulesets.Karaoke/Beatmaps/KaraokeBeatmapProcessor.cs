// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.ObjectExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Patterns;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Workings;

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

            beatmap.CurrentStageInfo.ReloadBeatmap(beatmap);

            StageInfo? getWorkingStage()
                => Beatmap.StageInfos.FirstOrDefault();

            StageInfo createDefaultWorkingStage() => new PreviewStageInfo();
        }

        private void applyInvalidProperty(KaraokeBeatmap beatmap)
        {
            foreach (var hitObject in beatmap.HitObjects.OfType<KaraokeHitObject>())
            {
                switch (hitObject)
                {
                    case Lyric lyric:
                        foreach (var flag in lyric.GetAllInvalidWorkingProperties())
                        {
                            applyInvalidProperty(beatmap, lyric, flag);
                        }

                        break;

                    case Note note:
                        foreach (var flag in note.GetAllInvalidWorkingProperties())
                        {
                            applyInvalidProperty(beatmap, note, flag);
                        }

                        break;
                }
            }
        }

        private static void applyInvalidProperty(KaraokeBeatmap beatmap, Lyric lyric, LyricWorkingProperty flag)
        {
            switch (flag)
            {
                case LyricWorkingProperty.StartTime:
                    lyric.StartTime = getStartTime(beatmap.CurrentStageInfo.AsNonNull(), lyric);
                    break;

                case LyricWorkingProperty.Duration:
                    lyric.Duration = getDuration(beatmap.CurrentStageInfo.AsNonNull(), lyric);
                    break;

                case LyricWorkingProperty.Timing:
                    // start time and duration should be set by other condition.
                    break;

                case LyricWorkingProperty.Singers:
                    lyric.Singers = beatmap.SingerInfo.GetSingerByIds(lyric.SingerIds.ToArray());
                    break;

                case LyricWorkingProperty.Page:
                    var pageInfo = beatmap.PageInfo;
                    lyric.PageIndex = pageInfo.GetPageIndexAt(lyric.LyricStartTime);
                    break;

                case LyricWorkingProperty.ReferenceLyric:
                    lyric.ReferenceLyric = findLyricById(beatmap, lyric.ReferenceLyricId);
                    break;

                case LyricWorkingProperty.StageElements:
                    lyric.StageElements = getStageElements(beatmap.CurrentStageInfo.AsNonNull(), lyric);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            static double getStartTime(StageInfo stageInfo, Lyric lyric)
            {
                (double? startTime, double? _) = stageInfo.GetStartAndEndTime(lyric);
                return startTime ?? 0;
            }

            static double getDuration(StageInfo stageInfo, Lyric lyric)
            {
                (double? startTime, double? endTime) = stageInfo.GetStartAndEndTime(lyric);
                return endTime - startTime ?? 0;
            }

            static IList<StageElement> getStageElements(StageInfo stageInfo, Lyric lyric)
                => stageInfo.GetStageElements(lyric).ToList();
        }

        private static void applyInvalidProperty(KaraokeBeatmap beatmap, Note note, NoteWorkingProperty flag)
        {
            switch (flag)
            {
                case NoteWorkingProperty.Page:
                    var pageInfo = beatmap.PageInfo;
                    note.PageIndex = pageInfo.GetPageIndexAt(note.StartTime);
                    break;

                case NoteWorkingProperty.ReferenceLyric:
                    note.ReferenceLyric = findLyricById(beatmap, note.ReferenceLyricId);
                    break;

                case NoteWorkingProperty.StageElements:
                    note.StageElements = getStageElements(beatmap.CurrentStageInfo.AsNonNull(), note);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            static IList<StageElement> getStageElements(StageInfo stageInfo, Note note)
                => stageInfo.GetStageElements(note).ToList();
        }

        private static Lyric? findLyricById(IBeatmap beatmap, int? id) =>
            id == null ? null : beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == id);

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
