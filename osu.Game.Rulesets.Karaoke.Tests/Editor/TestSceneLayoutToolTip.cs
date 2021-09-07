// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Legacy;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneLayoutToolTip : OsuTestScene
    {
        private readonly KaraokeLegacySkinTransformer skin = TestResources.GetKaraokeLegacySkinTransformer("special-skin");
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
            if (layouts == null)
                return;

            foreach (var (key, value) in layouts)
            {
                setTooltip($"Test lyric with layout {value}", lyric => { lyric.LayoutIndex = key; });
            }
        }

        private void setTooltip(string testName, Action<Lyric> callBack)
        {
            AddStep(testName, () =>
            {
                var singer = new Lyric
                {
                    Text = "karaoke!"
                };
                callBack?.Invoke(singer);
                toolTip.SetContent(singer);
            });
        }
    }
}
