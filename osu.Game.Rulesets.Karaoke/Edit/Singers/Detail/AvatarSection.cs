// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Detail
{
    internal class AvatarSection : EditSingerSection
    {
        protected override string Title => "Avatar";

        public AvatarSection(Singer singer)
        {
            Children = new Drawable[]
            {
                // todo: because cannot open the popover inside the popover.
                // so might change the avatar only if click the avatar image in the singer editor.
                new LabelledImageSelector
                {
                    Label = "Avatar",
                    Description = "Select image to set avatar.",
                    Current = singer.AvatarBindable,
                },
                new LabelledHueSelector
                {
                    Label = "Colour",
                    Description = "Select singer colour.",
                    Current = singer.HueBindable,
                }
            };
        }
    }
}
