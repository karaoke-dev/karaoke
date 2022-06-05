// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Parts
{
    public class DrawableTimeTag : CompositeDrawable, IHasCustomTooltip<TimeTag>
    {
        /// <summary>
        /// Height of major bar line triangles.
        /// </summary>
        private const float triangle_width = 6;

        private readonly TimeTag timeTag;

        public DrawableTimeTag(TimeTag timeTag)
        {
            AutoSizeAxes = Axes.Both;
            Origin = TextIndexUtils.GetValueByState(timeTag.Index, Anchor.BottomLeft, Anchor.BottomRight);

            this.timeTag = timeTag;
            var state = timeTag.Index.State;

            InternalChild = new DrawableTextIndex
            {
                Name = "Text index",
                Size = new Vector2(triangle_width),
                State = state
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            InternalChild.Colour = colours.GetTimeTagColour(timeTag);
        }

        public ITooltip<TimeTag> GetCustomTooltip() => new TimeTagTooltip();

        public TimeTag TooltipContent => timeTag;
    }
}
