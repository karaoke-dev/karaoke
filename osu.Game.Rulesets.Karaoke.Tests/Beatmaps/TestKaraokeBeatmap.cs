// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Rulesets.Karaoke.Tests.Resources;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps
{
    public class TestKaraokeBeatmap : Beatmap
    {
        public TestKaraokeBeatmap(RulesetInfo ruleset)
        {
            var baseBeatmap = createTestBeatmap();

            BeatmapInfo = baseBeatmap.BeatmapInfo;
            ControlPointInfo = baseBeatmap.ControlPointInfo;
            Breaks = baseBeatmap.Breaks;
            HitObjects = baseBeatmap.HitObjects;

            BeatmapInfo.RulesetID = 1;
            BeatmapInfo.Ruleset = ruleset;
            BeatmapInfo.BeatmapSet.Metadata = BeatmapInfo.Metadata;
            BeatmapInfo.BeatmapSet.Beatmaps = new List<BeatmapInfo> { BeatmapInfo };
            BeatmapInfo.BeatmapSet.Files = new List<BeatmapSetFileInfo>();
        }

        private static Beatmap createTestBeatmap()
        {
            using (var stream = TestResources.OpenBeatmapResource("karaoke-file-samples"))
            using (var reader = new LineBufferedReader(stream))
                return Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
        }
    }
}
