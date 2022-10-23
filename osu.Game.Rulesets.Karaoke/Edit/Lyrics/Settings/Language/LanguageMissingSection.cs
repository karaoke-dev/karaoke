// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Language
{
    public class LanguageMissingSection : LyricEditorSection
    {
        protected override LocalisableString Title => "Invalid lyric (missing language)";

        private BindableDictionary<Lyric, Issue[]> bindableReports;

        private LyricLanguageIssueTable table;

        [BackgroundDependencyLoader]
        private void load(LyricCheckerManager lyricCheckerManager)
        {
            Children = new[]
            {
                table = new LyricLanguageIssueTable(),
            };

            bindableReports = lyricCheckerManager.BindableReports.GetBoundCopy();
            bindableReports.BindCollectionChanged((_, _) =>
            {
                var issues = bindableReports.Values.SelectMany(x => x);
                table.Issues = issues.Where(x => x.Template is CheckLyricLanguage.IssueTemplateNotFillLanguage);
            }, true);
        }

        public class LyricLanguageIssueTable : IssueTableContainer
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

                    Content = value.Select((g, _) =>
                    {
                        var lyric = g.HitObjects.FirstOrDefault() as Lyric;
                        return createContent(lyric);
                    }).ToArray().ToRectangular();

                    BackgroundFlow.Children = value.Select((g, _) =>
                    {
                        var lyric = g.HitObjects.FirstOrDefault() as Lyric;
                        return new LyricLanguageRowBackground(lyric);
                    }).ToArray();
                }
            }

            protected override TableColumn[] CreateHeaders() => new[]
            {
                new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
                new TableColumn("Lyric", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 40)),
                new TableColumn("Message", Anchor.CentreLeft),
            };

            private Drawable[] createContent(Lyric lyric) => new Drawable[]
            {
                new SpriteIcon
                {
                    Origin = Anchor.Centre,
                    Size = new Vector2(10),
                    Colour = colours.Red,
                    Margin = new MarginPadding { Left = 10 },
                    Icon = FontAwesome.Solid.Globe,
                },
                new OsuSpriteText
                {
                    Text = $"#{lyric.Order}",
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                    Margin = new MarginPadding { Right = 10 },
                },
                new OsuSpriteText
                {
                    Text = "This lyric is missing language.",
                    Truncate = true,
                    RelativeSizeAxes = Axes.X,
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium)
                },
            };

            private class LyricLanguageRowBackground : RowBackground
            {
                private readonly Lyric lyric;

                public LyricLanguageRowBackground(Lyric lyric)
                {
                    this.lyric = lyric;
                }
            }
        }
    }
}
