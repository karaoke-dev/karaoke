// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos.Badges;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos.TimeInfo;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos
{
    public class InfoControl : Container
    {
        private const int max_height = 120;

        private Box headerBackground;

        public Lyric Lyric { get; }

        public InfoControl(Lyric lyric)
        {
            Lyric = lyric;

            Children = new Drawable[]
            {
                headerBackground = new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Height = max_height,
                    Alpha = 0.7f
                },
                new BadgeFillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Spacing = new Vector2(5),
                    Children = new Drawable[]
                    {
                        new TimeInfoContainer(Lyric)
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 36,
                        },

                        // todo : in small display size use badge.
                        // in larger size should use real icon.
                        new LanguageInfoBadge(Lyric)
                        {
                            Margin = new MarginPadding { Right = 5 }
                        }
                    }
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            headerBackground.Colour = colours.Gray2;
        }

        public class BadgeFillFlowContainer : FillFlowContainer
        {
            public override void Add(Drawable drawable)
            {
                drawable.Anchor = Anchor.TopRight;
                drawable.Origin = Anchor.TopRight;
                base.Add(drawable);
            }
        }
    }
}
