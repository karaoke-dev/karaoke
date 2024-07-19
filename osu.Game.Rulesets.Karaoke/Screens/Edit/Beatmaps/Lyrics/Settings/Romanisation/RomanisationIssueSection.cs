// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation;

public partial class RomanisationIssueSection : LyricEditorIssueSection
{
    protected override LyricEditorMode EditMode => LyricEditorMode.EditRomanisation;

    protected override LyricsIssueTable CreateLyricsIssueTable() => new RomanisationIssueTable();

    private partial class RomanisationIssueTable : LyricsIssueTable
    {
        protected override Dimension[] CreateDimensions()
        {
            return new[]
            {
                new Dimension(GridSizeMode.AutoSize, minSize: 30),
                new Dimension(GridSizeMode.AutoSize, minSize: 40),
                new Dimension(GridSizeMode.AutoSize, minSize: 60),
                new Dimension(),
            };
        }

        protected override IssueTableHeaderText[] CreateHeaders() => new[]
        {
            new IssueTableHeaderText(string.Empty, Anchor.CentreLeft),
            new IssueTableHeaderText("Lyric", Anchor.CentreLeft),
            new IssueTableHeaderText("Position", Anchor.CentreLeft),
            new IssueTableHeaderText("Message", Anchor.CentreLeft),
        };

        protected override Tuple<Drawable[], Action<Issue>> CreateContent()
        {
            IssueIcon issueIcon;
            OsuSpriteText orderSpriteText;
            OsuSpriteText timeSpriteText;
            TruncatingSpriteText issueSpriteText;

            return new Tuple<Drawable[], Action<Issue>>(
                new Drawable[]
                {
                    issueIcon = new IssueIcon
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(10),
                        Margin = new MarginPadding { Left = 10 },
                    },
                    orderSpriteText = new OsuSpriteText
                    {
                        Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                        Margin = new MarginPadding { Right = 10 },
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
                    (var lyric, TimeTag timeTag) = getInvalidByIssue(issue);

                    issueIcon.Issue = issue;
                    orderSpriteText.Text = $"#{lyric.Order}";
                    timeSpriteText.Text = TimeTagUtils.FormattedString(timeTag);
                    issueSpriteText.Text = issue.ToString();
                });

            static Tuple<Lyric, TimeTag> getInvalidByIssue(Issue issue)
            {
                if (issue is not LyricTimeTagIssue timeTagIssue)
                    throw new InvalidCastException();

                return new Tuple<Lyric, TimeTag>(timeTagIssue.Lyric, timeTagIssue.TimeTag);
            }
        }
    }
}
