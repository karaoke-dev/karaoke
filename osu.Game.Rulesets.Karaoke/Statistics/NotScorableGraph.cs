// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Statistics
{
    public class NotScorableGraph : Container
    {
        public NotScorableGraph()
        {
            Masking = true;
            CornerRadius = 5;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(0.5f),
                },
                new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = OsuFont.GetFont(weight: FontWeight.Bold, size: 20),
                    Text = "Sorry, this beatmap is not scorable."
                }
            };
        }
    }
}
