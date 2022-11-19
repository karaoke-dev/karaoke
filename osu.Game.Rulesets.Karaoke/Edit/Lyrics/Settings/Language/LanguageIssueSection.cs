// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Edit.Components.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Language
{
    public class LanguageIssueSection : IssueSection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.Language;

        protected override IssueTable CreateIssueTable() => new LanguageIssueTable();

        private class LanguageIssueTable : IssueTable
        {
            protected override TableColumn[] CreateHeaders() => new[]
            {
                new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
                new TableColumn("Lyric", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 40)),
                new TableColumn("Message", Anchor.CentreLeft),
            };

            protected override Drawable[] CreateContent(Issue issue)
            {
                var lyric = getInvalidByIssue(issue);

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
                        Text = $"#{lyric.Order}",
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

            private Lyric getInvalidByIssue(Issue issue)
            {
                if (issue is not LyricIssue lyricIssue)
                    throw new InvalidCastException();

                return lyricIssue.Lyric;
            }
        }
    }
}
