// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public abstract class KaraokeSkinEditorScreenTestScene<T> : EditorClockTestScene where T : KaraokeSkinEditorScreen
    {
        [Cached]
        private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Pink);

        private readonly KaraokeBeatmapSkin karaokeSkin = new TestKaraokeBeatmapSkin();

        protected override void LoadComplete()
        {
            Child = new SkinProvidingContainer(karaokeSkin)
            {
                RelativeSizeAxes = Axes.Both,
                Child = CreateEditorScreen(karaokeSkin).With(x =>
                {
                    x.State.Value = Visibility.Visible;
                })
            };
        }

        protected abstract T CreateEditorScreen(KaraokeSkin karaokeSkin);

        protected class TestKaraokeBeatmapSkin : KaraokeBeatmapSkin
        {
            public TestKaraokeBeatmapSkin()
                : base(new SkinInfo(), TestResources.CreateSkinStorageResourceProvider())
            {
            }
        }
    }
}
