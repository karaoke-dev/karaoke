// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Beatmaps.Utils;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public partial class DrawableCircleSingerAvatar : DrawableSingerAvatar
{
    private readonly IBindable<float> bindableHue = new Bindable<float>();

    [BackgroundDependencyLoader]
    private void load(OsuColour colour)
    {
        Masking = true;
        CornerRadius = Math.Min(DrawSize.X, DrawSize.Y) / 2f;
        BorderThickness = 5;

        bindableHue.BindValueChanged(_ =>
        {
            BorderColour = Singer != null ? SingerUtils.GetContentColour(Singer) : colour.Gray0;
        }, true);
    }

    public override ISinger? Singer
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
