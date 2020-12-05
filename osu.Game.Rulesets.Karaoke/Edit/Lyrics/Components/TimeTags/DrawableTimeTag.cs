// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osuTK;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.TimeTags
{
    public class DrawableTimeTag : CompositeDrawable
    {
        /// <summary>
        /// Height of major bar line triangles.
        /// </summary>
        private const float triangle_width = 6;

        private readonly Tuple<TimeTagIndex, double?> timeTag;

        public DrawableTimeTag(Tuple<TimeTagIndex, double?> timeTag)
        {
            this.timeTag = timeTag;

            InternalChild = new EquilateralTriangle
            {
                Name = "Time tag triangle",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Size = new Vector2(triangle_width),
                Rotation = timeTag.Item1.State == TimeTagIndex.IndexState.Start ? 90 : 270
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            InternalChild.Colour = timeTag.Item2.HasValue ? colours.Yellow : colours.Gray7;
        }
    }
}
