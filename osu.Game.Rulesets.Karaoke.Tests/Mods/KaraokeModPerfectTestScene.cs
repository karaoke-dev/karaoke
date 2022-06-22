// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Mods;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public abstract class KaraokeModPerfectTestScene : ModPerfectTestScene
    {
        protected override Ruleset CreatePlayerRuleset() => new KaraokeRuleset();

        protected KaraokeModPerfectTestScene(ModPerfect mod)
            : base(mod)
        {
        }
    }
}
