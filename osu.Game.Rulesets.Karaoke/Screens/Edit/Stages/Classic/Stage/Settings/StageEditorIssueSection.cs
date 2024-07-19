// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage.Settings;

public partial class StageEditorIssueSection : IssueSection
{
    private readonly StageEditorEditCategory category;

    public StageEditorIssueSection(StageEditorEditCategory category)
    {
        this.category = category;
    }

    protected override EmptyIssue CreateEmptyIssue() => new StageEditorEmptyIssue();

    protected override IssueNavigator CreateIssueNavigator() => new StageEditorIssueNavigator();

    protected override IssueTable CreateIssueTable() => new StageEditorIssueTable(category);

    [BackgroundDependencyLoader]
    private void load(IStageEditorVerifier stageEditorVerifier)
    {
        Issues.BindTo(stageEditorVerifier.GetIssueByType(category));
    }

    private partial class StageEditorEmptyIssue : EmptyIssue
    {
        [Resolved]
        private IStageEditorVerifier stageEditorVerifier { get; set; } = null!;

        protected override void OnRefreshButtonClicked()
            => stageEditorVerifier.Refresh();
    }

    private partial class StageEditorIssueNavigator : IssueNavigator
    {
        [Resolved]
        private IStageEditorVerifier stageEditorVerifier { get; set; } = null!;

        protected override void OnRefreshButtonClicked()
            => stageEditorVerifier.Refresh();
    }

    public partial class StageEditorIssueTable : IssueTable
    {
        [Resolved]
        private IStageEditorVerifier stageEditorVerifier { get; set; } = null!;

        [Resolved]
        private OverlayColourProvider colourProvider { get; set; } = null!;

        private readonly StageEditorEditCategory category;

        public StageEditorIssueTable(StageEditorEditCategory category)
        {
            this.category = category;
        }

        protected override Dimension[] CreateDimensions()
        {
            if (category == StageEditorEditCategory.Timing)
            {
                return new[]
                {
                    new Dimension(GridSizeMode.AutoSize, minSize: 30),
                    new Dimension(GridSizeMode.AutoSize, minSize: 40),
                    new Dimension(),
                };
            }

            return new[]
            {
                new Dimension(GridSizeMode.AutoSize, minSize: 30),
                new Dimension(),
            };
        }

        protected override IssueTableHeaderText[] CreateHeaders()
        {
            if (category == StageEditorEditCategory.Timing)
            {
                return new[]
                {
                    new IssueTableHeaderText(string.Empty, Anchor.CentreLeft),
                    new IssueTableHeaderText("Time", Anchor.CentreLeft),
                    new IssueTableHeaderText("Message", Anchor.CentreLeft),
                };
            }

            return new[]
            {
                new IssueTableHeaderText(string.Empty, Anchor.CentreLeft),
                new IssueTableHeaderText("Message", Anchor.CentreLeft),
            };
        }

        protected override Tuple<Drawable[], Action<Issue>> CreateContent()
        {
            if (category == StageEditorEditCategory.Timing)
            {
                IssueIcon issuesIcon;
                OsuSpriteText timeSpriteText;
                TruncatingSpriteText issueSpriteText;

                return new Tuple<Drawable[], Action<Issue>>(new Drawable[]
                {
                    issuesIcon = new IssueIcon
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(10),
                        Margin = new MarginPadding { Left = 10 },
                    },
                    timeSpriteText = new OsuSpriteText
                    {
                        Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                        Margin = new MarginPadding { Right = 10 },
                    },
                    issueSpriteText = new TruncatingSpriteText
                    {
                        RelativeSizeAxes = Axes.X,
                        Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium),
                    },
                }, issue =>
                {
                    issuesIcon.Issue = issue;
                    timeSpriteText.Text = getInvalidObjectTimeByIssue(issue);
                    issueSpriteText.Text = issue.ToString();
                });
            }
            else
            {
                IssueIcon issuesIcon;
                TruncatingSpriteText issueSpriteText;

                return new Tuple<Drawable[], Action<Issue>>(new Drawable[]
                {
                    issuesIcon = new IssueIcon
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(10),
                        Margin = new MarginPadding { Left = 10 },
                    },
                    issueSpriteText = new TruncatingSpriteText
                    {
                        RelativeSizeAxes = Axes.X,
                        Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium),
                    },
                }, issue =>
                {
                    issuesIcon.Issue = issue;
                    issueSpriteText.Text = issue.ToString();
                });
            }

            static string getInvalidObjectTimeByIssue(Issue issue) => issue.Time?.ToEditorFormattedString() ?? string.Empty;
        }

        protected override Color4 GetBackgroundColour(bool selected)
        {
            return selected ? colourProvider.Colour3 : colourProvider.Background1;
        }

        protected override void OnIssueClicked(Issue issue)
            => stageEditorVerifier.Navigate(issue);
    }
}
