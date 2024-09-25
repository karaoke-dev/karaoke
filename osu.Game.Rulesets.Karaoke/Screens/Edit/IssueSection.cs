// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class IssueSection : EditorSection
{
    protected sealed override LocalisableString Title => "Issues";

    protected readonly IBindableList<Issue> Issues = new BindableList<Issue>();

    protected IssueSection()
    {
        EmptyIssue emptyIssue;

        IssueNavigator issueNavigator;
        IssueTable issueTable;

        Children = new Drawable[]
        {
            emptyIssue = CreateEmptyIssue().With(x =>
            {
                x.Anchor = Anchor.Centre;
                x.Origin = Anchor.Centre;
                x.RelativeSizeAxes = Axes.X;
                x.AutoSizeAxes = Axes.Y;
                x.Padding = new MarginPadding(10);
            }),

            issueNavigator = CreateIssueNavigator(),
            issueTable = CreateIssueTable(),
        };

        Issues.BindCollectionChanged((_, _) =>
        {
            bool hasIssue = Issues.Any();

            emptyIssue.Alpha = hasIssue ? 0 : 1;

            issueNavigator.Alpha = hasIssue ? 1 : 0;
            issueNavigator.Issues = Issues;

            issueTable.Alpha = hasIssue ? 1 : 0;
            issueTable.Issues = Issues.Take(100);
        }, true);
    }

    protected abstract EmptyIssue CreateEmptyIssue();

    protected abstract IssueNavigator CreateIssueNavigator();

    protected abstract IssueTable CreateIssueTable();

    protected abstract partial class EmptyIssue : ClickableContainer
    {
        private readonly SpriteIcon icon;

        protected readonly Box Background;
        protected readonly OsuSpriteText Text;

        protected EmptyIssue()
        {
            Action = OnRefreshButtonClicked;

            InternalChild = new Container
            {
                CornerRadius = 20,
                Masking = true,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    Background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.8f,
                    },
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Padding = new MarginPadding(20),
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                            icon = new SpriteIcon
                            {
                                Icon = FontAwesome.Solid.CheckCircle,
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Size = new Vector2(50),
                            },
                            Text = new OsuSpriteText
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Text = "No issue here!",
                                Font = OsuFont.GetFont(size: 28),
                            },
                            new OsuSpriteText
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Text = "Click this area to re-check again.",
                                Font = OsuFont.GetFont(size: 14),
                            },
                        },
                    },
                },
            };

            AddInternal(new HoverClickSounds(HoverSampleSet.Button));
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider, OsuColour colours)
        {
            Background.Colour = colourProvider.Background5;
            icon.Colour = colours.Green;
            Text.Colour = colourProvider.Colour1;
        }

        protected abstract void OnRefreshButtonClicked();

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Content.ScaleTo(0.9f, 4000, Easing.OutQuint);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            Content.ScaleTo(1, 1000, Easing.OutElastic);
            base.OnMouseUp(e);
        }
    }

    protected abstract partial class IssueNavigator : CompositeDrawable
    {
        private readonly FillFlowContainer<IssueCategory> categoryList;
        private readonly IconButton reloadButton;

        protected readonly Box Background;
        protected readonly Box BlockBox;

        protected IssueNavigator()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            InternalChild = new Container
            {
                CornerRadius = 5,
                Masking = true,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    Background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.8f,
                    },
                    categoryList = new FillFlowContainer<IssueCategory>
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Padding = new MarginPadding(10),
                        Direction = FillDirection.Horizontal,
                        Spacing = new Vector2(5),
                        Children = createCategory(),
                    },
                    BlockBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        RelativePositionAxes = Axes.Both,
                        X = 0.5f,
                        Size = new Vector2(0.5f, 1f),
                    },
                    reloadButton = new IconButton
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        X = -5,
                        Icon = FontAwesome.Solid.Redo,
                    },
                },
            };
        }

        private IssueCategory[] createCategory()
            => Enum.GetValues<IssueType>().Select(type => new IssueCategory
            {
                Type = type,
                Text = getTextByIssueType(type),
                IssueColour = getColourByIssueType(type),
            }).ToArray();

        private LocalisableString getTextByIssueType(IssueType issueType) =>
            issueType switch
            {
                IssueType.Problem => "Problem",
                IssueType.Warning => "Warning",
                IssueType.Error => "Internal error",
                IssueType.Negligible => "Suggestion",
                _ => throw new ArgumentOutOfRangeException(nameof(issueType), issueType, null),
            };

        private Colour4 getColourByIssueType(IssueType issueType) =>
            new IssueTemplate(new EmptyCheck(), issueType, string.Empty).Colour;

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            var colour = colourProvider.Background5;
            Background.Colour = colour;
            BlockBox.Colour = ColourInfo.GradientHorizontal(colour.Opacity(0), colour);
            reloadButton.Action = OnRefreshButtonClicked;
        }

        protected abstract void OnRefreshButtonClicked();

        public IReadOnlyList<Issue> Issues
        {
            set
            {
                foreach (var category in categoryList.Children)
                {
                    int count = value.Count(x => x.Template.Type == category.Type);

                    category.Alpha = count == 0 ? 0 : 1;
                    category.Count = count;
                }
            }
        }

        private partial class IssueCategory : CompositeDrawable
        {
            private const int text_size = 14;

            private readonly Box background;
            private readonly OsuSpriteText issueName;
            private readonly OsuSpriteText countSpriteText;

            public IssueCategory()
            {
                AutoSizeAxes = Axes.X;
                Height = 20;

                InternalChild = new Container
                {
                    CornerRadius = 5,
                    Masking = true,
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Y,
                            AutoSizeAxes = Axes.X,
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(5),
                            Padding = new MarginPadding
                            {
                                Horizontal = 5,
                            },
                            Children = new[]
                            {
                                issueName = new OsuSpriteText
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Font = OsuFont.GetFont(size: text_size, weight: FontWeight.Bold),
                                },
                                countSpriteText = new OsuSpriteText
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Font = OsuFont.GetFont(size: text_size, weight: FontWeight.Bold),
                                },
                            },
                        },
                    },
                };
            }

            public IssueType Type { get; init; }

            public LocalisableString Text
            {
                get => issueName.Text;
                set => issueName.Text = value;
            }

            private Colour4 issueColour;

            public Colour4 IssueColour
            {
                get => issueColour;
                set
                {
                    issueColour = value;

                    background.Colour = value;
                    issueName.Colour = value.Darken(0.7f);
                    countSpriteText.Colour = value.Lighten(1f);
                }
            }

            private int count;

            public int Count
            {
                get => count;
                set
                {
                    count = value;
                    countSpriteText.Text = value.ToString("#,0");
                }
            }
        }
    }

    private class EmptyCheck : ICheck
    {
        public IEnumerable<Issue> Run(BeatmapVerifierContext context)
        {
            throw new NotImplementedException();
        }

        public CheckMetadata Metadata => new(CheckCategory.Metadata, string.Empty);

        public IEnumerable<IssueTemplate> PossibleTemplates => Array.Empty<IssueTemplate>();
    }
}
