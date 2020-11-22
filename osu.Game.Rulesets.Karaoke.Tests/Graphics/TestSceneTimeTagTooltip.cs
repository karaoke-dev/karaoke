// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    [TestFixture]
    public class TestSceneTimeTagTooltip : OsuTestScene
    {
        private TimeTagTooltip toolTip;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = toolTip = new TimeTagTooltip
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            toolTip.Show();
        });

        [Test]
        public void TestDisplayToolTip()
        {
            setTooltip("Start time tag.", TimeTagsUtils.Create(new TimeTagIndex(0), 1280));
            setTooltip("End time tag.", TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.End), 1280));
            setTooltip("Null time", TimeTagsUtils.Create(new TimeTagIndex(0), null));
        }

        private void setTooltip(string testName, Tuple<TimeTagIndex, double?> timeTag)
        {
            AddStep(testName, () =>
            {
                toolTip.SetContent(timeTag);
            });
        }
    }
}
