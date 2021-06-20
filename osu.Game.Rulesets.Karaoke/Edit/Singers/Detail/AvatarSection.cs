// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Detail
{
    internal class AvatarSection : EditSingerSection
    {
        protected override string Title => "Avatar";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new LabelledImageSelector
                {
                    Label = "Avatar",
                    Description = "Select image to set avatar."
                },
                new LabelledColourSelector
                {
                    Label = "Colour",
                    Description = "Select singer colour."
                }
            };
        }
    }
}
