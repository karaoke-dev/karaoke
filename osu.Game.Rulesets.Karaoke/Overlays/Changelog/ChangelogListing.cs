// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    /// <summary>
    /// Display list of <see cref="APIChangelogBuild"/>
    /// </summary>
    public class ChangelogListing : ChangelogContent
    {
        private readonly List<APIChangelogBuild> entries;

        public ChangelogListing(List<APIChangelogBuild> entries)
        {
            this.entries = entries.Take(4).ToList();
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider, Bindable<APIChangelogBuild> current)
        {
            if (entries == null)
                return;

            foreach (var build in entries)
            {
                if (Children.Count != 0)
                {
                    Add(new Box
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 2,
                        Colour = colourProvider.Background6,
                        Margin = new MarginPadding { Top = 30 },
                    });
                }

                Add(new ChangelogBuild(build)
                {
                    Masking = true,
                    AutoSizeAxes = Axes.None,
                    Height = 300,
                    SelectBuild = SelectBuild
                });
            }

            if (entries.Any())
            {
                Add(new ShowMoreButton
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Padding = new MarginPadding { Top = 15, Bottom = 15 },
                    Action = () =>
                    {
                        current.Value = entries.LastOrDefault();
                    },
                });
            }
        }
    }
}
