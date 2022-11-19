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
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings
{
    public abstract class IssueSection : LyricEditorSection
    {
        protected sealed override LocalisableString Title => "Issues";

        private readonly IBindableList<Issue> bindableIssues = new BindableList<Issue>();

        protected IssueSection()
        {
            EmptyIssue emptyIssue;

            IssueNavigator issueNavigator;
            IssueTable issueTable;

            Children = new Drawable[]
            {
                emptyIssue = new EmptyIssue
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding(10),
                },

                issueNavigator = new IssueNavigator(),
                issueTable = CreateIssueTable()
            };

            bindableIssues.BindCollectionChanged((_, _) =>
            {
                bool hasIssue = bindableIssues.Any();

                emptyIssue.Alpha = hasIssue ? 0 : 1;

                issueNavigator.Alpha = hasIssue ? 1 : 0;
                issueNavigator.Issues = bindableIssues;

                issueTable.Alpha = hasIssue ? 1 : 0;
                issueTable.Issues = bindableIssues.Take(100);
            }, true);
        }

        protected abstract LyricEditorMode EditMode { get; }

        protected abstract IssueTable CreateIssueTable();

        [BackgroundDependencyLoader]
        private void load(ILyricEditorVerifier verifier)
        {
            bindableIssues.BindTo(verifier.GetIssueByEditMode(EditMode));
        }

        private class EmptyIssue : ClickableContainer
        {
            [BackgroundDependencyLoader]
            private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state, ILyricEditorVerifier verifier, OsuColour colours)
            {
                Action = verifier.Refresh;

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
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = colourProvider.Background5(state.Mode),
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
                                new SpriteIcon
                                {
                                    Icon = FontAwesome.Solid.CheckCircle,
                                    Colour = colours.Green,
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Size = new Vector2(50),
                                },
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = "No issue here!",
                                    Colour = colourProvider.Colour1(state.Mode),
                                    Font = OsuFont.GetFont(size: 28),
                                },
                                new OsuSpriteText
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Text = "Click this area to re-check again.",
                                    Font = OsuFont.GetFont(size: 14),
                                },
                            }
                        },
                    }
                };

                AddInternal(new HoverClickSounds(HoverSampleSet.Button));
            }

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

        private class IssueNavigator : CompositeDrawable
        {
            private readonly Box background;
            private readonly FillFlowContainer<IssueCategory> categoryList;
            private readonly Box blockBox;
            private readonly IconButton reloadButton;

            public IssueNavigator()
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
                        background = new Box
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
                            Children = createCategory()
                        },
                        blockBox = new Box
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
                        }
                    }
                };
            }

            private IssueCategory[] createCategory()
                => EnumUtils.GetValues<IssueType>().Select(type => new IssueCategory
                {
                    Type = type,
                    Text = getTextByIssueType(type),
                    IssueColour = getColourByIssueType(type)
                }).ToArray();

            private LocalisableString getTextByIssueType(IssueType issueType) =>
                issueType switch
                {
                    IssueType.Problem => "Problem",
                    IssueType.Warning => "Warning",
                    IssueType.Error => "Internal error",
                    IssueType.Negligible => "Suggestion",
                    _ => throw new ArgumentOutOfRangeException(nameof(issueType), issueType, null)
                };

            private Colour4 getColourByIssueType(IssueType issueType) =>
                new IssueTemplate(new EmptyCheck(), issueType, "").Colour;

            [BackgroundDependencyLoader]
            private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state, ILyricEditorVerifier verifier)
            {
                var colour = colourProvider.Background5(state.Mode);
                background.Colour = colour;
                blockBox.Colour = ColourInfo.GradientHorizontal(colour.Opacity(0), colour);
                reloadButton.Action = verifier.Refresh;
            }

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

            private class IssueCategory : CompositeDrawable
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
                                    Horizontal = 5
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
                                    }
                                }
                            }
                        }
                    };
                }

                public IssueType Type { get; set; }

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

            public CheckMetadata Metadata { get; } = null!;

            public IEnumerable<IssueTemplate> PossibleTemplates { get; } = null!;
        }
    }
}
