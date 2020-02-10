// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestSceneAutoplayMod : TestSceneKaraokePlayer
    {
        protected override Player CreatePlayer(Ruleset ruleset)
        {
            SelectedMods.Value = new[] { new KaraokeModAutoplay() };

            return base.CreatePlayer(ruleset);
        }
    }
}
