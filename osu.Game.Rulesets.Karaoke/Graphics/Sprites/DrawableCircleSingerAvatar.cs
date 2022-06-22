// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites
{
    public class DrawableCircleSingerAvatar : DrawableSingerAvatar
    {
        private readonly IBindable<float> bindableHue = new Bindable<float>();

        [BackgroundDependencyLoader]
        private void load(LargeTextureStore textures)
        {
            Masking = true;
            CornerRadius = Math.Min(DrawSize.X, DrawSize.Y) / 2f;
            BorderThickness = 5;

            bindableHue.BindValueChanged(_ =>
            {
                BorderColour = SingerUtils.GetContentColour(Singer);
            }, true);
        }

        public override ISinger Singer
        {
            get => base.Singer;
            set
            {
                base.Singer = value;

                bindableHue.UnbindBindings();

                if (value is Singer singer)
                    bindableHue.BindTo(singer.HueBindable);
            }
        }
    }
}
