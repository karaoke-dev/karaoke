// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class DrawableKaraokeEditRuleset : DrawableKaraokeRuleset
    {
        public DrawableKaraokeEditRuleset(Ruleset ruleset, IWorkingBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
        }
    }
}
