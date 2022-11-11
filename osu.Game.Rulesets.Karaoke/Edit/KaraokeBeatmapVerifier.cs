// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeBeatmapVerifier : IBeatmapVerifier
    {
        protected readonly List<ICheck> Checks = new()
        {
            new CheckBeatmapAvailableTranslates(),
            new CheckLyricLanguage(),
            new CheckLyricReferenceLyric(),
            new CheckLyricRomajiTag(),
            new CheckLyricRubyTag(),
            new CheckLyricSinger(),
            new CheckLyricText(),
            new CheckLyricTime(),
            new CheckLyricTimeTag(),
            new CheckLyricTranslate(),
            new CheckNoteReferenceLyric(),
            new CheckNoteText(),
        };

        public IEnumerable<Issue> Run(BeatmapVerifierContext context) => AvailableChecks(Checks).SelectMany(check => check.Run(context));

        protected virtual IEnumerable<ICheck> AvailableChecks(List<ICheck> checks) => checks;
    }
}
