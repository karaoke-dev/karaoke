// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.RubyRomaji.Components
{
    public abstract class TextTagIssueTable<TTextTag> : IssueTableContainer where TTextTag : ITextTag
    {
        [Resolved]
        private OsuColour colours { get; set; }

        public IEnumerable<Issue> Issues
        {
            set
            {
                Content = null;
                BackgroundFlow.Clear();

                if (value == null)
                    return;

                Content = value.Select(createContent).ToArray().ToRectangular();
                BackgroundFlow.Children = value.Select(x =>
                {
                    (var lyric, TTextTag textTag) = getInvalidByIssue(x);
                    return new TextTagRowBackground(lyric, textTag);
                }).ToArray();
            }
        }

        protected override TableColumn[] CreateHeaders() => new[]
        {
            new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
            new TableColumn("Lyric", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 40)),
            new TableColumn("Position", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 60)),
            new TableColumn("Message", Anchor.CentreLeft),
        };

        private Drawable[] createContent(Issue issue)
        {
            (var lyric, TTextTag textTag) = getInvalidByIssue(issue);

            return new Drawable[]
            {
                new SpriteIcon
                {
                    Origin = Anchor.Centre,
                    Size = new Vector2(10),
                    Colour = colours.Red,
                    Margin = new MarginPadding { Left = 10 },
                    Icon = FontAwesome.Solid.Tag,
                },
                new OsuSpriteText
                {
                    Text = $"#{lyric.Order}",
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                    Margin = new MarginPadding { Right = 10 },
                },
                new OsuSpriteText
                {
                    Text = TextTagUtils.PositionFormattedString(textTag),
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

        private Tuple<Lyric, TTextTag> getInvalidByIssue(Issue issue)
        {
            var lyric = issue.HitObjects.OfType<Lyric>().Single();
            var textTag = issue.Arguments.OfType<TTextTag>().Single();

            return new Tuple<Lyric, TTextTag>(lyric, textTag);
        }

        private class TextTagRowBackground : RowBackground
        {
            private readonly Lyric lyric;
            private readonly TTextTag textTag;

            public TextTagRowBackground(Lyric lyric, TTextTag textTag)
            {
                this.lyric = lyric;
                this.textTag = textTag;
            }
        }
    }
}
