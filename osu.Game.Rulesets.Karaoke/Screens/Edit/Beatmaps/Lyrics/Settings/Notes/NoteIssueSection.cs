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
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Notes;

public partial class NoteIssueSection : LyricEditorIssueSection
{
    protected override LyricEditorMode EditMode => LyricEditorMode.EditNote;

    protected override LyricsIssueTable CreateLyricsIssueTable() => new NoteIssueTable();

    private partial class NoteIssueTable : LyricsIssueTable
    {
        protected override Dimension[] CreateDimensions() => new[]
        {
            new Dimension(GridSizeMode.AutoSize, minSize: 30),
            new Dimension(GridSizeMode.AutoSize, minSize: 40),
            new Dimension(),
        };

        protected override IssueTableHeaderText[] CreateHeaders() => new[]
        {
            new IssueTableHeaderText(string.Empty, Anchor.CentreLeft),
            new IssueTableHeaderText("Note", Anchor.CentreLeft),
            new IssueTableHeaderText("Message", Anchor.CentreLeft),
        };

        protected override Tuple<Drawable[], Action<Issue>> CreateContent()
        {
            IssueIcon issueIcon;
            OsuSpriteText orderSpriteText;
            TruncatingSpriteText issueSpriteText;

            return new Tuple<Drawable[], Action<Issue>>(new Drawable[]
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
                issueSpriteText = new TruncatingSpriteText
                {
                    RelativeSizeAxes = Axes.X,
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium),
                },
            }, issue =>
            {
                var note = getInvalidByIssue(issue);
                string noteIndex = note.ReferenceLyric?.Order.ToString() ?? "??";

                issueIcon.Issue = issue;
                orderSpriteText.Text = $"#{noteIndex}";
                issueSpriteText.Text = issue.ToString();
            });

            static Note getInvalidByIssue(Issue issue)
            {
                if (issue is not NoteIssue noteIssue)
                    throw new InvalidCastException();

                return noteIssue.Note;
            }
        }
    }
}
