// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos
{
    public class TimeInfoContainer : Container
    {
        private readonly Box background;
        private readonly OsuSpriteText timeRange;

        public TimeInfoContainer(Lyric lyric)
        {
            Children = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                timeRange = new OsuSpriteText
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Font = OsuFont.GetFont(size: 16, fixedWidth: true),
                    Padding = new MarginPadding(10),
                }
            };

            // todo : might move to another function for updating time.
            var startTime = lyric.StartTime.ToEditorFormattedString();
            var endTime = lyric.EndTime.ToEditorFormattedString();
            timeRange.Text = startTime + " - " + endTime;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray4;
        }
    }
}
