// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using System;
using osu.Game.Overlays;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    /// <summary>
    /// Display <see cref="KaraokeChangelogBuild"/> detail
    /// </summary>
    public class ChangelogSingleBuild : ChangelogContent
    {
        private readonly KaraokeChangelogBuild build;

        public ChangelogSingleBuild(KaraokeChangelogBuild build)
        {
            this.build = build;
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            // todo: get result from here

            if (true)
            {
                Children = new Drawable[]
                {
                    new ChangelogBuildWithNavigation(build) { SelectBuild = SelectBuild },
                    new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 2,
                        Colour = colourProvider.Background6,
                        Margin = new MarginPadding { Top = 30 },
                    },
                };
            }
        }

        public class ChangelogBuildWithNavigation : ChangelogBuild
        {
            public ChangelogBuildWithNavigation(KaraokeChangelogBuild build)
                : base(build)
            {
            }

            protected override FillFlowContainer CreateHeader()
            {
                var fill = base.CreateHeader();

                fill.Insert(-1, new NavigationIconButton(Build.Versions.Next)
                {
                    Icon = FontAwesome.Solid.ChevronLeft,
                    SelectBuild = b => SelectBuild(b)
                });
                fill.Insert(1, new NavigationIconButton(Build.Versions?.Previous)
                {
                    Icon = FontAwesome.Solid.ChevronRight,
                    SelectBuild = b => SelectBuild(b)
                });

                return fill;
            }
        }

        private class NavigationIconButton : IconButton
        {
            public Action<KaraokeChangelogBuild> SelectBuild;

            public NavigationIconButton(KaraokeChangelogBuild build)
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                if (build == null) return;

                TooltipText = build.DisplayVersion;

                Action = () =>
                {
                    SelectBuild?.Invoke(build);
                    Enabled.Value = false;
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                HoverColour = colours.GreyVioletLight.Opacity(0.6f);
                FlashColour = colours.GreyVioletLighter;
            }
        }
    }
}
