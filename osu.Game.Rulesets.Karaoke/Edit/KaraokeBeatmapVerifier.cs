// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Configs;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeBeatmapVerifier : IBeatmapVerifier
    {
        private readonly List<ICheck> checks = new List<ICheck>
        {
            new CheckInvalidPropertyLyrics(),
            new CheckInvalidRubyRomajiLyrics(new LyricCheckerConfig().CreateDefaultConfig()), // todo : implement config apply.
            new CheckInvalidTimeLyrics(new LyricCheckerConfig().CreateDefaultConfig()), // todo : implement config apply.
            new CheckInvalidPropertyNotes(),
            new CheckTranslate(),
        };

        public IEnumerable<Issue> Run(IBeatmap beatmap, WorkingBeatmap workingBeatmap) => checks.SelectMany(check => check.Run(beatmap, workingBeatmap));
    }
}
