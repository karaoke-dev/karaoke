// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    internal class NoteColorSection : StyleSection
    {
        private ColorPicker noteColorPicker;
        private ColorPicker blinkColorPicker;

        protected override string Title => "Color";

        [BackgroundDependencyLoader]
        private void load(SkinManager manager)
        {
            Children = new Drawable[]
            {
                noteColorPicker = new ColorPicker
                {
                    RelativeSizeAxes = Axes.X,
                },
                blinkColorPicker = new ColorPicker
                {
                    RelativeSizeAxes = Axes.X,
                }
            };
        }
    }
}
