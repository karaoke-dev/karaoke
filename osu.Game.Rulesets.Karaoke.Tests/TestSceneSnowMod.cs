// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestSceneSnowMod : TestSceneKaraokePlayer
    {
        public TestSceneSnowMod()
        {
            Mods.Value = new[] { new KaraokeModSnow() };
        }
    }
}
