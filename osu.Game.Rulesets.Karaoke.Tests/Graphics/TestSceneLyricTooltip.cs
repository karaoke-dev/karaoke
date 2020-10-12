// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    [TestFixture]
    public class TestSceneLyricTooltip : OsuTestScene
    {
        private LyricTooltip toolTip;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = toolTip = new LyricTooltip
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            toolTip.Show();
        });

        [Test]
        public void TestDisplayToolTip()
        {
            var beatmap = new TestKaraokeBeatmap(null);
            var lyrics = beatmap.HitObjects.OfType<LyricLine>().ToList();

            foreach (var lyric in lyrics)
            {
                AddStep($"Test lyric: {lyric.Text}", () => { toolTip.SetContent(lyric); });
            }
        }
    }
}
