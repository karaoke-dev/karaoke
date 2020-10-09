// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Tests.Visual;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Skinning;
using System.Collections.Generic;
using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    [TestFixture]
    public class TestSceneLayoutToolTip : OsuTestScene
    {
        private KaraokeLayoutTestSkin skin = new KaraokeLayoutTestSkin();
        private LayoutToolTip toolTip;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = new SkinProvidingContainer(skin)
            {
                RelativeSizeAxes = Axes.Both,
                Child = toolTip = new LayoutToolTip
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            };
            toolTip.Show();
        });


        [Test]
        public void TestDisplayToolTip()
        {
            var layouts = skin.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Layout)?.Value;
            foreach (var layout in layouts)
            {
                setTooltip($"Test lyric with layout {layout.Value}", lyric => {
                    lyric.LayoutIndex = layout.Key;
                });
            }
        }

        private void setTooltip(string testName, Action<LyricLine> callBack)
        {
            AddStep(testName, () =>
            {
                var singer = new LyricLine();
                callBack?.Invoke(singer);
                toolTip.SetContent(singer);
            });
        }

        public class KaraokeLayoutTestSkin : KaraokeLegacySkinTransformer
        {
            public KaraokeLayoutTestSkin()
                : base(null)
            {
            }
        }
    }
}
