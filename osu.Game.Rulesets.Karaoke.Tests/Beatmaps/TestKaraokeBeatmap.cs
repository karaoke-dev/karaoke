// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Tests.Resources;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps
{
    public class TestKaraokeBeatmap : Beatmap
    {
        public TestKaraokeBeatmap(RulesetInfo ruleset)
        {
            // It's a tricky way not to trigger change handler throw exception.
            // not a good solution, but seems ok because there's no other ruleset in the test case.
            ruleset.OnlineID = 1;

            var baseBeatmap = createTestBeatmap();

            BeatmapInfo = baseBeatmap.BeatmapInfo;
            ControlPointInfo = baseBeatmap.ControlPointInfo;
            Breaks = baseBeatmap.Breaks;
            HitObjects = baseBeatmap.HitObjects;

            BeatmapInfo.Ruleset = ruleset;

            Debug.Assert(BeatmapInfo.BeatmapSet != null);

            BeatmapInfo.BeatmapSet.Beatmaps.Add(BeatmapInfo);
        }

        private static Beatmap createTestBeatmap()
        {
            using (var stream = TestResources.OpenBeatmapResource("karaoke-file-samples"))
            using (var reader = new LineBufferedReader(stream))
                return Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
        }
    }
}
