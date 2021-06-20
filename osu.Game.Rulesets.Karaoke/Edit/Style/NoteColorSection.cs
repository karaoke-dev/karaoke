// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    internal class NoteColorSection : StyleSection
    {
        private LabelledColourSelector noteColorPicker;
        private LabelledColourSelector blinkColorPicker;

        protected override string Title => "Color";

        [BackgroundDependencyLoader]
        private void load(SkinManager manager)
        {
            Children = new Drawable[]
            {
                noteColorPicker = new LabelledColourSelector
                {
                    Label = "Note color",
                    Description = "Select color.",
                },
                blinkColorPicker = new LabelledColourSelector
                {
                    Label = "Blink color",
                    Description = "Select color.",
                }
            };
        }
    }
}
