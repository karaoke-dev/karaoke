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

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
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
                    new ChangelogBuildWithNavigation(),
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

        public class ChangelogBuildWithNavigation : Container
        {
            
        }

        private class NavigationIconButton : IconButton
        {
            public Action<string> SelectBuild;

            public NavigationIconButton(string build)
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                if (build == null) return;

                TooltipText = build;

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
