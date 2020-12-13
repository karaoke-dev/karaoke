// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos.Badges
{
    public abstract class Badge : Container
    {
        private readonly Box box;
        private readonly OsuSpriteText badgeText;

        protected Lyric Lyric { get; }

        protected Badge(Lyric lyric)
        {
            Lyric = lyric;

            AutoSizeAxes = Axes.Both;
            Masking = true;
            CornerRadius = 3;
            Children = new Drawable[]
            {
                box = new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                badgeText = new OsuSpriteText
                {
                    Margin = new MarginPadding
                    {
                        Left = 5,
                        Right = 5,
                        Top = 2,
                        Bottom = 2
                    },
                    Text = "Badge"
                }
            };
        }

        public string BadgeText
        {
            get => badgeText.Text;
            set => badgeText.Text = value;
        }

        public ColourInfo BadgeColour
        {
            get => box.Colour;
            set => box.Colour = value;
        }
    }
}
