// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags
{
    public partial class TimeTagIssueSection : LyricEditorIssueSection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditTimeTag;

        protected override LyricsIssueTable CreateLyricsIssueTable() => new TimeTagIssueTable();

        private partial class TimeTagIssueTable : LyricsIssueTable
        {
            protected override TableColumn[] CreateHeaders() => new[]
            {
                new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
                new TableColumn("Lyric", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 40)),
                new TableColumn("Position", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 60)),
                new TableColumn("Message", Anchor.CentreLeft),
            };

            protected override Drawable[] CreateContent(Issue issue)
            {
                (var lyric, TimeTag? timeTag) = getInvalidByIssue(issue);

                // show the issue with the invalid time-tag.
                if (timeTag != null)
                {
                    return new Drawable[]
                    {
                        new IssueIcon
                        {
                            Origin = Anchor.Centre,
                            Size = new Vector2(10),
                            Margin = new MarginPadding { Left = 10 },
                            Issue = issue,
                        },
                        new OsuSpriteText
                        {
                            Text = $"#{lyric.Order}",
                            Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                            Margin = new MarginPadding { Right = 10 },
                        },
                        new OsuSpriteText
                        {
                            Text = TextIndexUtils.PositionFormattedString(timeTag.Index),
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

                // show the default issue if not able to get the time-tag.
                return new Drawable[]
                {
                    new SpriteIcon
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(10),
                        Colour = issue.Template.Colour,
                        Margin = new MarginPadding { Left = 10 },
                        Icon = FontAwesome.Solid.AlignLeft
                    },
                    new OsuSpriteText
                    {
                        Text = $"#{lyric.Order}",
                        Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                        Margin = new MarginPadding { Right = 10 },
                    },
                    new OsuSpriteText
                    {
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

            private Tuple<Lyric, TimeTag?> getInvalidByIssue(Issue issue) =>
                issue switch
                {
                    LyricTimeTagIssue timeTagIssue => new Tuple<Lyric, TimeTag?>(timeTagIssue.Lyric, timeTagIssue.TimeTag),
                    LyricIssue lyricIssue => new Tuple<Lyric, TimeTag?>(lyricIssue.Lyric, null),
                    _ => throw new InvalidCastException()
                };
        }
    }
}
