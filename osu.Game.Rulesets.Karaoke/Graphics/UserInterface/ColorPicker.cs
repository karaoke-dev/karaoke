// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class ColorPicker : Box, IHasCurrentValue<Color4>
    {
        public Bindable<Color4> CurrentColor { get; set; } = new Bindable<Color4>();

        public ColorPicker()
        {
            Height = 200;
            Colour = Color4.Blue;
        }

        public Bindable<Color4> Current { get; set; } = new Bindable<Color4>();
    }
}
