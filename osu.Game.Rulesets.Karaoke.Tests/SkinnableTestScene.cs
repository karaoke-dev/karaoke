// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;
using System;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    public abstract class SkinnableTestScene : OsuGridTestScene
    {
        private Skin metricsSkin;
        private Skin defaultSkin;
        private Skin specialSkin;

        protected SkinnableTestScene()
            : base(2, 2)
        {
        }

        [BackgroundDependencyLoader]
        private void load(SkinManager skinManager)
        {
            var dllStore = new DllResourceStore("osu.Game.Rulesets.Karaoke.Tests.dll");

            // TODO : apply skin
            metricsSkin = skinManager.GetSkin(DefaultLegacySkin.Info);
            defaultSkin = skinManager.GetSkin(DefaultLegacySkin.Info);
            specialSkin = skinManager.GetSkin(DefaultLegacySkin.Info);
        }

        public void SetContents(Func<Drawable> creationFunction)
        {
            Cell(0).Child = createProvider(null, creationFunction);
            Cell(1).Child = createProvider(metricsSkin, creationFunction);
            Cell(2).Child = createProvider(defaultSkin, creationFunction);
            Cell(3).Child = createProvider(specialSkin, creationFunction);
        }

        private Drawable createProvider(Skin skin, Func<Drawable> creationFunction)
        {
            var mainProvider = new SkinProvidingContainer(skin);

            return mainProvider
                .WithChild(new SkinProvidingContainer(Ruleset.Value.CreateInstance().CreateLegacySkinProvider(mainProvider))
                {
                    Child = creationFunction()
                });
        }
    }
}
