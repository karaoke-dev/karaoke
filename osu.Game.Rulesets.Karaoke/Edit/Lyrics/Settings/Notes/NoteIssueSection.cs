// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Notes
{
    public class NoteIssueSection : IssueSection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditNote;

        protected override IssueTable CreateIssueTable() => new NoteIssueTable();

        private class NoteIssueTable : IssueTable
        {
            protected override TableColumn[] CreateHeaders() => new[]
            {
                new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
                new TableColumn("Note", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 40)),
                new TableColumn("Message", Anchor.CentreLeft),
            };

            protected override Drawable[] CreateContent(Issue issue)
            {
                var note = getInvalidByIssue(issue);
                string noteIndex = note.ReferenceLyric?.Order.ToString() ?? "??";

                return new Drawable[]
                {
                    new SpriteIcon
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(10),
                        Colour = issue.Template.Colour,
                        Margin = new MarginPadding { Left = 10 },
                        Icon = FontAwesome.Solid.Globe,
                    },
                    new OsuSpriteText
                    {
                        Text = $"#{noteIndex}",
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

            private Note getInvalidByIssue(Issue issue)
                => issue.HitObjects.OfType<Note>().Single();
        }
    }
}
