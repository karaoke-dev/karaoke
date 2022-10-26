// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.ComponentModel;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Cursor
{
    public class InvalidLyricToolTip : BackgroundToolTip<Issue[]>
    {
        private readonly MessageContainer invalidMessage;

        public InvalidLyricToolTip()
        {
            Child = invalidMessage = new MessageContainer(s => s.Font = s.Font.With(size: 14))
            {
                Width = 300,
                AutoSizeAxes = Axes.Y,
                Colour = Color4.White.Opacity(0.75f),
                Spacing = new Vector2(0, 5),
                Name = "Invalid message",
            };
        }

        private Issue[] lastIssues;

        public override void SetContent(Issue[] issues)
        {
            if (issues == lastIssues)
                return;

            lastIssues = issues;

            // clear exist warning.
            invalidMessage.Clear();

            foreach (var issue in issues)
            {
                switch (issue)
                {
                    // print normal message
                    case Issue:
                        invalidMessage.AddAlertParagraph(issue.Template.GetMessage());
                        break;

                    // Should throw exception because every issue message should be printed.
                    default:
                        throw new InvalidEnumArgumentException(nameof(issue));
                }
            }

            // show no problem message
            if (issues.Length == 0)
                invalidMessage.AddSuccessParagraph("Seems no issue in this lyric.");
        }
    }
}
