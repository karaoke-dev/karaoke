// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Bindables;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Skinning.Fonts;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    public class TestSceneFontSelector : OsuManualInputManagerTestScene
    {
        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        private FontManager fontManager;

        [BackgroundDependencyLoader]
        private void load()
        {
            base.Content.AddRange(new Drawable[]
            {
                Content,
                fontManager = new FontManager(),
            });

            Dependencies.Cache(fontManager);
        }

        [Test]
        public void TestAllFiles()
        {
            AddStep("create", () =>
            {
                var language = new BindableFontUsage
                {
                    MinFontSize = 32,
                    MaxFontSize = 72
                };
                Child = new FontSelector
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.6f, 0.8f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Current = language
                };
            });
        }
    }
}
