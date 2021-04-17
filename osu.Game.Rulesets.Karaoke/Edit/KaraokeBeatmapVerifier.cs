// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Checks;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeBeatmapVerifier : IBeatmapVerifier
    {
        private readonly List<ICheck> checks = new List<ICheck>
        {
            // todo : implement config apply.
            new CheckInvalidLyrics(new LyricCheckerConfig())
        };

        public IEnumerable<Issue> Run(IBeatmap beatmap) => checks.SelectMany(check => check.Run(beatmap));
    }
}
