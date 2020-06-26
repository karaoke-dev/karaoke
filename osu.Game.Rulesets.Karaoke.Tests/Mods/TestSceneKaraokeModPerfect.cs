﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public class TestSceneKaraokeModPerfect : ModPerfectTestScene
    {
        protected override Ruleset CreatePlayerRuleset() => new KaraokeRuleset();

        public TestSceneKaraokeModPerfect()
            : base(new KaraokeModPerfect())
        {
        }

        // TODO : test case = false will be added after saiten system is implemented.
        [TestCase(true)]
        public void TestLyric(bool shouldMiss) => CreateHitObjectTest(new HitObjectTestData(new LyricLine
        {
            StartTime = 1000,
            Duration = 1000,
            Text = "カラオケ!",
            TimeTags = new Dictionary<TimeTagIndex, double>()
        }), shouldMiss);
    }
}
