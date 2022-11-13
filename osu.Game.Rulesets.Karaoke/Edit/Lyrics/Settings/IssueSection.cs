// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit.Checks.Components;
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

                // todo: add the issue amount display also.
                issueTable = CreateIssueTable()
            };

            bindableIssues.BindCollectionChanged((_, _) =>
            {
                bool hasIssue = bindableIssues.Any();
                emptyIssue.Alpha = hasIssue ? 0 : 1;
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

        protected class EmptyIssue : ClickableContainer
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

        protected abstract class IssueTable : LyricEditorTable
        {
            [Resolved, AllowNull]
            private IIssueNavigator issueNavigator { get; set; }

            protected IssueTable()
            {
                Columns = CreateHeaders();
            }

            public IEnumerable<Issue> Issues
            {
                set
                {
                    Content = null;
                    BackgroundFlow.Clear();

                    foreach (var issue in value)
                    {
                        BackgroundFlow.Add(new RowBackground(issue)
                        {
                            Action = () =>
                            {
                                issueNavigator.Navigate(issue);
                            },
                        });
                    }

                    Content = value.Select(CreateContent).ToArray().ToRectangular();
                }
            }

            protected abstract TableColumn[] CreateHeaders();

            protected abstract Drawable[] CreateContent(Issue issue);
        }
    }
}
