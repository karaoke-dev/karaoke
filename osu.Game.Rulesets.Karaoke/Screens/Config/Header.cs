// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class Header : Container
    {
        public const float HEIGHT = 75;

        [Resolved]
        private ConfigColourProvider colourProvider { get; set; }

        private readonly Box background;
        private readonly KaraokeConfigHeaderTitle title;
        private readonly KaraokeConfigPageTabControl tabs;

        public Header()
        {
            RelativeSizeAxes = Axes.X;
            Height = HEIGHT;

            Children = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4Extensions.FromHex(@"#1f1921"),
                },
                new Container
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Left = 10 },
                    Children = new Drawable[]
                    {
                        title = new KaraokeConfigHeaderTitle
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.BottomLeft,
                        },
                        tabs = new KaraokeConfigPageTabControl
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            RelativeSizeAxes = Axes.X,
                            Scale = new Vector2(1.5f)
                        },
                    },
                },
            };

            tabs.Current.BindValueChanged(x =>
            {
                background.Delay(200).Then().FadeColour(colourProvider.GetBackground2Colour(x.NewValue), 500);

                tabs.Colour = colourProvider.GetContent2Colour(x.NewValue);
                tabs.StripColour = colourProvider.GetContentColour(x.NewValue);
            });
        }

        public IReadOnlyList<SettingsSection> TabItems
        {
            get => tabs.Items;
            set => tabs.Items = value;
        }

        [BackgroundDependencyLoader]
        private void load(Bindable<SettingsSection> selectedSection)
        {
            tabs.Current.BindTo(selectedSection);
        }

        private class KaraokeConfigHeaderTitle : CompositeDrawable
        {
            private const float spacing = 6;

            private readonly OsuSpriteText dot;
            private readonly OsuSpriteText pageTitle;

            public KaraokeConfigHeaderTitle()
            {
                AutoSizeAxes = Axes.Both;

                InternalChildren = new Drawable[]
                {
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Spacing = new Vector2(spacing, 0),
                        Direction = FillDirection.Horizontal,
                        Children = new Drawable[]
                        {
                            new OsuSpriteText
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Font = OsuFont.GetFont(size: 24),
                                Text = "Karaoke"
                            },
                            dot = new OsuSpriteText
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Font = OsuFont.GetFont(size: 24),
                                Text = "·"
                            },
                            pageTitle = new OsuSpriteText
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Font = OsuFont.GetFont(size: 24),
                            }
                        }
                    },
                };
            }

            [BackgroundDependencyLoader]
            private void load(ConfigColourProvider colourProvider, Bindable<SettingsSection> selectedSection)
            {
                selectedSection.BindValueChanged(x =>
                {
                    var colour = colourProvider.GetContentColour(x.NewValue);

                    pageTitle.Text = x.NewValue.Header;
                    pageTitle.FadeColour(colour, 200);
                });
            }
        }

        private class KaraokeConfigPageTabControl : PageTabControl<SettingsSection>
        {
            protected override TabItem<SettingsSection> CreateTabItem(SettingsSection value)
                => new KaraokeConfigPageTabItem(value);

            internal class KaraokeConfigPageTabItem : PageTabItem
            {
                public KaraokeConfigPageTabItem(SettingsSection value)
                    : base(value)
                {
                }

                protected override string CreateText() => Value.Header;
            }
        }
    }
}
