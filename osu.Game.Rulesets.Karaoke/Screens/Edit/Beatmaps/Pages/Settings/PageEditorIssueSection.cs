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

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Settings;

public partial class PageEditorIssueSection : IssueSection
{
    protected override EmptyIssue CreateEmptyIssue() => new PageEditorEmptyIssue();

    protected override IssueNavigator CreateIssueNavigator() => new PageEditorIssueNavigator();

    protected override IssueTable CreateIssueTable() => new PageEditorIssueTable();

    [BackgroundDependencyLoader]
    private void load(IPageEditorVerifier pageEditorVerifier)
    {
        Issues.BindTo(pageEditorVerifier.Issues);
    }

    private partial class PageEditorEmptyIssue : EmptyIssue
    {
        [Resolved]
        private IPageEditorVerifier pageEditorVerifier { get; set; } = null!;

        protected override void OnRefreshButtonClicked()
            => pageEditorVerifier.Refresh();
    }

    private partial class PageEditorIssueNavigator : IssueNavigator
    {
        [Resolved]
        private IPageEditorVerifier pageEditorVerifier { get; set; } = null!;

        protected override void OnRefreshButtonClicked()
            => pageEditorVerifier.Refresh();
    }

    public partial class PageEditorIssueTable : IssueTable
    {
        [Resolved]
        private IPageEditorVerifier pageEditorVerifier { get; set; } = null!;

        protected override void OnIssueClicked(Issue issue)
            => pageEditorVerifier.Navigate(issue);

        protected override TableColumn[] CreateHeaders() => new[]
        {
            new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
            new TableColumn("Time", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 40)),
            new TableColumn("Message", Anchor.CentreLeft),
        };

        protected override Drawable[] CreateContent(Issue issue)
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

        private static string getInvalidObjectTimeByIssue(Issue issue) => issue.Time?.ToEditorFormattedString() ?? string.Empty;
    }
}
