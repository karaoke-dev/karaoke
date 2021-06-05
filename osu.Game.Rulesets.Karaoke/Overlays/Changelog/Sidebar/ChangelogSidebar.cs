// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog.Sidebar
{
    public class ChangelogSidebar : CompositeDrawable
    {
        [Cached]
        public readonly Bindable<APIChangelogSidebar> Metadata = new Bindable<APIChangelogSidebar>();

        private FillFlowContainer<ChangelogSection> changelogsFlow;

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            RelativeSizeAxes = Axes.Y;
            Width = 250;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background4
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = OsuScrollContainer.SCROLL_BAR_HEIGHT,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Colour = colourProvider.Background3,
                    Alpha = 0.5f
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Right = -3 }, // Compensate for scrollbar margin
                    Child = new OsuScrollContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Padding = new MarginPadding { Right = 3 }, // Addeded 3px back
                            Child = new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Padding = new MarginPadding
                                {
                                    Vertical = 20,
                                    Left = 50,
                                    Right = 30
                                },
                                Child = new FillFlowContainer
                                {
                                    Direction = FillDirection.Vertical,
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Spacing = new Vector2(0, 20),
                                    Children = new Drawable[]
                                    {
                                        new YearsPanel(),
                                        changelogsFlow = new FillFlowContainer<ChangelogSection>
                                        {
                                            AutoSizeAxes = Axes.Y,
                                            RelativeSizeAxes = Axes.X,
                                            Direction = FillDirection.Vertical,
                                            Spacing = new Vector2(0, 10)
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Metadata.BindValueChanged(onMetadataChanged, true);
        }

        private void onMetadataChanged(ValueChangedEvent<APIChangelogSidebar> metadata)
        {
            changelogsFlow.Clear();

            if (metadata.NewValue == null)
                return;

            var allPosts = metadata.NewValue.Changelogs;

            if (allPosts?.Any() != true)
                return;

            var lookup = metadata.NewValue.Changelogs.ToLookup(post => post.PublishedAt.Month);

            var keys = lookup.Select(kvp => kvp.Key);
            var sortedKeys = keys.OrderByDescending(k => k).ToList();

            var year = metadata.NewValue.CurrentYear;

            for (int i = 0; i < sortedKeys.Count; i++)
            {
                var month = sortedKeys[i];
                var posts = lookup[month];

                changelogsFlow.Add(new ChangelogSection(month, year, posts)
                {
                    Expanded = { Value = i == 0 }
                });
            }
        }
    }
}
