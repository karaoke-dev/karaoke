// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Detail
{
    public class SingerEditPopover : OsuPopover
    {
        public SingerEditPopover(Singer singer)
        {
            if (singer == null)
                throw new ArgumentNullException(nameof(singer));

            Child = new OsuScrollContainer
            {
                Height = 500,
                Width = 300,
                Child = new FillFlowContainer<EditSingerSection>
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = new EditSingerSection[]
                    {
                        new AvatarSection(singer),
                        new MetadataSection(singer),
                    }
                }
            };
        }
    }
}
