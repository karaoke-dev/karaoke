// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
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
            setTooltip("Start time tag.", new TimeTag(new TextIndex(0), 1280));
            setTooltip("End time tag.", new TimeTag(new TextIndex(0, TextIndex.IndexState.End), 1280));
            setTooltip("Null time", new TimeTag(new TextIndex(0)));
        }

        private void setTooltip(string testName, TimeTag timeTag)
        {
            AddStep(testName, () =>
            {
                toolTip.SetContent(timeTag);
            });
        }
    }
}
