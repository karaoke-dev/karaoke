// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Timing;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Graphics;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public class TestManageFontPreview : OsuTestScene
    {
        private readonly NowPlayingOverlay np;

        public TestManageFontPreview()
        {
            Clock = new FramedClock();
            Clock.ProcessFrame();

            Add(np = new NowPlayingOverlay
            {
                Origin = Anchor.TopRight,
                Anchor = Anchor.TopRight,
            });
        }

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            var resources = new KaraokeRuleset().CreateResourceStore();
            var textureStore = new TextureStore(host.CreateTextureLoaderStore(new NamespacedResourceStore<byte[]>(resources, @"Textures")));
            Dependencies.CacheAs(textureStore);

            Add(new ManageFontPreview
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            np.ToggleVisibility();
        }
    }
}
