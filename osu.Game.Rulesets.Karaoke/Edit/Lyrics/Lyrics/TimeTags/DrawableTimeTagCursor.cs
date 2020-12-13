// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Lyrics.TimeTags
{
    public class DrawableTimeTagCursor : CompositeDrawable
    {
        /// <summary>
        /// Height of major bar line triangles.
        /// </summary>
        private const float triangle_width = 4;

        private readonly TimeTag timeTag;

        public DrawableTimeTagCursor(TimeTag timeTag)
        {
            this.timeTag = timeTag;
            AutoSizeAxes = Axes.Both;

            InternalChild = new RightTriangle
            {
                Name = "Time tag triangle",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Size = new Vector2(triangle_width),
                Scale = new Vector2(timeTag.Index.State == TimeTagIndex.IndexState.Start ? 1 : -1, 1)
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            InternalChild.Colour = timeTag.Time.HasValue ? colours.YellowDarker : colours.Gray3;
        }
    }
}
