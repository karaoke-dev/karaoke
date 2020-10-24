// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class ColorPicker : Box
    {
        public Bindable<Color4> CurrentColor { get; set; } = new Bindable<Color4>();

        public ColorPicker()
        {
            Height = 200;
            Colour = Color4.Blue;
        }
    }
}
