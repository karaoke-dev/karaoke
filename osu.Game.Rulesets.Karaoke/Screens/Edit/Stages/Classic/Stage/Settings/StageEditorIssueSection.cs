// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osuTK;

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

        private readonly StageEditorEditCategory category;

        public StageEditorIssueTable(StageEditorEditCategory category)
        {
            this.category = category;
        }

        protected override void OnIssueClicked(Issue issue)
            => stageEditorVerifier.Navigate(issue);

        protected override TableColumn[] CreateHeaders()
        {
            if (category == StageEditorEditCategory.Timing)
            {
                return new[]
                {
                    new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
                    new TableColumn("Time", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 40)),
                    new TableColumn("Message", Anchor.CentreLeft),
                };
            }

            return new[]
            {
                new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
                new TableColumn("Message", Anchor.CentreLeft),
            };
        }

        protected override Drawable[] CreateContent(Issue issue)
        {
            if (category == StageEditorEditCategory.Timing)
            {
                return new Drawable[]
                {
                    new IssueIcon
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(10),
                        Margin = new MarginPadding { Left = 10 },
                        Issue = issue
                    },
                    new OsuSpriteText
                    {
                        Text = getInvalidObjectTimeByIssue(issue),
                        Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                        Margin = new MarginPadding { Right = 10 },
                    },
                    new OsuSpriteText
                    {
                        Text = issue.ToString(),
                        Truncate = true,
                        RelativeSizeAxes = Axes.X,
                        Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium)
                    },
                };
            }

            return new Drawable[]
            {
                new IssueIcon
                {
                    Origin = Anchor.Centre,
                    Size = new Vector2(10),
                    Margin = new MarginPadding { Left = 10 },
                    Issue = issue
                },
                new OsuSpriteText
                {
                    Text = issue.ToString(),
                    Truncate = true,
                    RelativeSizeAxes = Axes.X,
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium)
                },
            };
        }

        private static string getInvalidObjectTimeByIssue(Issue issue) => issue.Time?.ToEditorFormattedString() ?? string.Empty;
    }
}
