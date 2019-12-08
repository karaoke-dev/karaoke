// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestSceneSnowMod : TestSceneKaraokePlayer
    {
        protected override Player CreatePlayer(Ruleset ruleset)
        {
            Mods.Value = new[] { new KaraokeModSnow() };

            return base.CreatePlayer(ruleset);
        }
    }
}
