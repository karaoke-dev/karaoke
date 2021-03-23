// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    public class TestSceneFontSelectionDialog : OsuManualInputManagerTestScene
    {
        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        private DialogOverlay dialogOverlay;

        private LanguageSelectionDialog dialog;

        [BackgroundDependencyLoader]
        private void load()
        {
            base.Content.AddRange(new Drawable[]
            {
                Content,
                dialogOverlay = new DialogOverlay()
            });

            Dependencies.Cache(dialogOverlay);
        }

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            var language = new Bindable<FontUsage>(new FontUsage());
            Child = dialog = new FontSelectionDialog
            {
                Current = language,
            };
        });

        [SetUpSteps]
        public void SetUpSteps()
        {
            AddStep("show dialog", () => dialog.Show());
        }
    }
}
