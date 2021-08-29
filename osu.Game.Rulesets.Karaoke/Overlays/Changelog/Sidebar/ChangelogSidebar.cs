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
        public readonly Bindable<APIChangelogSidebar> Metadata = new();

        [Cached]
        public readonly Bindable<int> Year = new();

        private FillFlowContainer<ChangelogSection> changelogsFlow;

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider, Bindable<APIChangelogBuild> current)
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
                            Padding = new MarginPadding { Right = 3 }, // Add 3px back
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

            // should switch year selection if user switch changelog and the new changelog is not current year.
            current.BindValueChanged(e =>
            {
                if (e.NewValue == null)
                    return;

                Year.Value = e.NewValue.PublishedAt.Year;
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Metadata.BindValueChanged(e => onMetadataChanged(e.NewValue, Year.Value), true);
            Year.BindValueChanged(e => onMetadataChanged(Metadata.Value, e.NewValue), true);
        }

        private void onMetadataChanged(APIChangelogSidebar metadata, int targetYear)
        {
            changelogsFlow.Clear();

            if (metadata == null)
                return;

            var allPosts = metadata.Changelogs;

            if (allPosts?.Any() != true)
                return;

            var lookup = metadata.Changelogs.ToLookup(post => post.PublishedAt.Year);
            var posts = lookup[targetYear];
            changelogsFlow.Add(new ChangelogSection(targetYear, posts));
        }
    }
}
