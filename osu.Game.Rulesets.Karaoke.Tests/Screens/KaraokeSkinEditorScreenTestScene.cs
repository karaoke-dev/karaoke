// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Database;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public abstract class KaraokeSkinEditorScreenTestScene<T> : EditorClockTestScene where T : KaraokeSkinEditorScreen
    {
        [Cached]
        private readonly OverlayColourProvider colourProvider = new(OverlayColourScheme.Pink);

        private KaraokeSkin karaokeSkin;

        [BackgroundDependencyLoader]
        private void load(SkinManager skinManager)
        {
            skinManager.CurrentSkinInfo.Value = DefaultKaraokeSkin.Default.ToLive();

            karaokeSkin = skinManager.CurrentSkin.Value as KaraokeSkin;
        }

        protected override void LoadComplete()
        {
            Child = CreateEditorScreen(karaokeSkin).With(x =>
            {
                x.State.Value = Visibility.Visible;
            });
        }

        protected abstract T CreateEditorScreen(KaraokeSkin karaokeSkin);
    }
}
