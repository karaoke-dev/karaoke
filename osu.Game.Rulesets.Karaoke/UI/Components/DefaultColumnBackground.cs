// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace osu.Game.Rulesets.Karaoke.UI.Components;

public partial class DefaultColumnBackground : Box
{
    public const float COLUMN_HEIGHT = 20;

    public DefaultColumnBackground(int index)
    {
        RelativeSizeAxes = Axes.X;
        Height = COLUMN_HEIGHT;
        Alpha = 0.15f;
    }

    public bool IsSpecial { get; set; }
}
