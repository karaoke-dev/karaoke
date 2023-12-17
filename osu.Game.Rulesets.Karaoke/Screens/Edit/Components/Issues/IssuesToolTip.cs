// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;

public partial class IssuesToolTip : BackgroundToolTip<Issue[]>
{
    private readonly MessageTextFlowContainer invalidMessage;

    public IssuesToolTip()
    {
        Child = invalidMessage = new MessageTextFlowContainer
        {
            Width = 300,
            AutoSizeAxes = Axes.Y,
            Colour = Color4.White.Opacity(0.75f),
            Spacing = new Vector2(0, 5),
            Name = "Invalid message",
        };
    }

    private Issue[]? lastIssues;

    public override void SetContent(Issue[] issues)
    {
        if (issues == lastIssues)
            return;

        lastIssues = issues;

        // clear exist warning.
        invalidMessage.Clear();

        // show no problem message
        if (issues.Length == 0)
        {
            invalidMessage.AddSuccessParagraph("Seems no issue in this lyric.");
        }
        else
        {
            foreach (var issue in issues)
            {
                invalidMessage.AddIssueParagraph(issue);
            }
        }
    }

    private partial class MessageTextFlowContainer : OsuTextFlowContainer
    {
        private const int font_size = 14;

        [Resolved]
        private OsuColour colours { get; set; } = null!;

        public MessageTextFlowContainer()
            : base(s => s.Font = s.Font.With(size: font_size))
        {
        }

        public void AddIssueParagraph(Issue issue)
        {
            NewParagraph();

            AddPart(new IssueIconTextPart(issue));
            AddText($" {issue}");
        }

        public void AddSuccessParagraph(string text)
        {
            NewParagraph();
            AddIcon(FontAwesome.Solid.Check, icon =>
            {
                icon.Colour = colours.Green;
            });
            AddText($" {text}");
        }

        private class IssueIconTextPart : TextPart
        {
            private readonly Issue issue;

            public IssueIconTextPart(Issue issue)
            {
                this.issue = issue;
            }

            protected override IEnumerable<Drawable> CreateDrawablesFor(TextFlowContainer textFlowContainer)
            {
                yield return new IssueIcon
                {
                    Size = new Vector2(font_size),
                    Issue = issue,
                };
            }
        }
    }
}
