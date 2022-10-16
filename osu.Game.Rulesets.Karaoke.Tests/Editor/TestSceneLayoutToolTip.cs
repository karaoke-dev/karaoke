// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneLayoutToolTip : OsuTestScene
    {
        private readonly ISkin skin = new TestingSkin(null);
        private LayoutToolTip toolTip = null!;

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

            foreach ((int key, string value) in layouts)
            {
                setTooltip($"Test lyric with layout {value}", lyric =>
                {
                    // todo: should change mapping group id from the lyric.
                });
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
                callBack.Invoke(singer);
                toolTip.SetContent(singer);
            });
        }

        /// <summary>
        /// todo: it's a tricky way to create ruleset's own skin class.
        /// should use generic skin like <see cref="LegacySkin"/> eventually.
        /// </summary>
        public class TestingSkin : KaraokeSkin
        {
            internal static readonly Guid DEFAULT_SKIN = new("FEC5A291-5709-11EC-9F10-0800200C9A66");

            public static SkinInfo CreateInfo() => new()
            {
                ID = DEFAULT_SKIN,
                Name = "karaoke! (default skin)",
                Creator = "team karaoke!",
                Protected = true,
                InstantiationInfo = typeof(TestingSkin).GetInvariantInstantiationInfo()
            };

            public TestingSkin(IStorageResourceProvider? resources)
                : this(CreateInfo(), resources)
            {
            }

            [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
            public TestingSkin(SkinInfo skin, IStorageResourceProvider? resources)
                : base(skin, resources)
            {
                DefaultElement[ElementType.LyricConfig] = LyricConfig.CreateDefault();
                DefaultElement[ElementType.LyricStyle] = LyricStyle.CreateDefault();
                DefaultElement[ElementType.NoteStyle] = NoteStyle.CreateDefault();
            }
        }
    }
}
