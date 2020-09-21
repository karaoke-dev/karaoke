// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public class TestSceneKaraokeModPerfect : KaraokeModPerfectTestScene
    {
        public TestSceneKaraokeModPerfect()
            : base(new KaraokeModPerfect())
        {
        }

        // TODO : test case = false will be added after saiten system is implemented.
        [Ignore("Saiten should judgement by note, not lyric.")]
        public void TestLyric(bool shouldMiss) => CreateHitObjectTest(new HitObjectTestData(new LyricLine
        {
            StartTime = 1000,
            Duration = 1000,
            Text = "カラオケ!",
            TimeTags = new Dictionary<TimeTagIndex, double>()
        }), shouldMiss);
    }
}
