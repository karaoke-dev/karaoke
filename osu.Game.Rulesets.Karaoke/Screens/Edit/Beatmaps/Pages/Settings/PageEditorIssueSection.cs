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

        [Resolved]
        private OverlayColourProvider colourProvider { get; set; } = null!;

        protected override Dimension[] CreateDimensions() => new[]
        {
            new Dimension(GridSizeMode.AutoSize, minSize: 30),
            new Dimension(GridSizeMode.AutoSize, minSize: 40),
            new Dimension(),
        };

        protected override IssueTableHeaderText[] CreateHeaders() => new[]
        {
            new IssueTableHeaderText(string.Empty, Anchor.CentreLeft),
            new IssueTableHeaderText("Time", Anchor.CentreLeft),
            new IssueTableHeaderText("Message", Anchor.CentreLeft),
        };

        protected override Tuple<Drawable[], Action<Issue>> CreateContent()
        {
            IssueIcon issueIcon;
            OsuSpriteText timeSpriteText;
            TruncatingSpriteText issueSpriteText;

            return new Tuple<Drawable[], Action<Issue>>(new Drawable[]
            {
                issueIcon = new IssueIcon
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
                issueIcon.Issue = issue;
                timeSpriteText.Text = getInvalidObjectTimeByIssue(issue);
                issueSpriteText.Text = issue.ToString();
            });

            static string getInvalidObjectTimeByIssue(Issue issue) => issue.Time?.ToEditorFormattedString() ?? string.Empty;
        }

        protected override Color4 GetBackgroundColour(bool selected)
        {
            return selected ? colourProvider.Colour3 : colourProvider.Background1;
        }

        protected override void OnIssueClicked(Issue issue)
            => pageEditorVerifier.Navigate(issue);
    }
}
