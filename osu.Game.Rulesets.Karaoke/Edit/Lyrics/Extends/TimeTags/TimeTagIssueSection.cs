// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagIssueSection : Section
    {
        protected override string Title => "Invalid time-tag";

        private BindableDictionary<Lyric, Issue[]> bindableReports;

        private TimeTagIssueTable table;

        [BackgroundDependencyLoader]
        private void load(OsuColour colour, LyricCheckerManager lyricCheckerManager)
        {
            Children = new[]
            {
                // todo : should all invalid tag number
                table = new TimeTagIssueTable(),
            };

            bindableReports = lyricCheckerManager.BindableReports.GetBoundCopy();
            bindableReports.BindCollectionChanged((a, b) =>
            {
                // todo : might have filter in here.
                var issues = bindableReports.Values.SelectMany(x => x);
                table.Issues = issues.OfType<TimeTagIssue>();
            }, true);
        }

        public class TimeTagIssueTable : EditorTable
        {
            [Resolved]
            private EditorClock clock { get; set; }

            private Bindable<TimeTagIssue> selectedIssue;

            public IEnumerable<TimeTagIssue> Issues
            {
                set
                {
                    Content = null;
                    BackgroundFlow.Clear();

                    if (value == null)
                        return;

                    foreach (var issue in value)
                    {
                        BackgroundFlow.Add(new RowBackground(issue)
                        {
                            Action = () =>
                            {
                                selectedIssue.Value = issue;

                                if (issue.Time != null)
                                {
                                    // seek to target time-tag time if time-tag has time.
                                    clock.Seek(issue.Time.Value);
                                }

                                // todo : select target time-tag.
                            },
                        });
                    }

                    Columns = createHeaders();
                    Content = value.Select((g, i) => createContent(i, g)).ToArray().ToRectangular();
                }
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                // todo : might get from outside.
                // selectedIssue = verify.SelectedIssue.GetBoundCopy();

                selectedIssue = new Bindable<TimeTagIssue>();
                selectedIssue.BindValueChanged(issue =>
                {
                    foreach (var b in BackgroundFlow) b.Selected = b.Item == issue.NewValue;
                }, true);
            }

            private TableColumn[] createHeaders()
            {
                var columns = new List<TableColumn>
                {
                    new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize)),
                    new TableColumn("Lyric", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 60)),
                    new TableColumn("Position", Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 60)),
                    new TableColumn("Message", Anchor.CentreLeft),
                };

                return columns.ToArray();
            }

            private Drawable[] createContent(int index, Issue issue) => new Drawable[]
            {
                new OsuSpriteText
                {
                    Text = $"#{index + 1}",
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium),
                    Margin = new MarginPadding { Right = 10 }
                },
                new OsuSpriteText
                {
                    Text = issue.Template.Type.ToString(),
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                    Margin = new MarginPadding { Right = 10 },
                    Colour = issue.Template.Colour
                },
                new OsuSpriteText
                {
                    Text = issue.GetEditorTimestamp(),
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                    Margin = new MarginPadding { Right = 10 },
                },
                new OsuSpriteText
                {
                    Text = issue.ToString(),
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium)
                },
                new OsuSpriteText
                {
                    Text = issue.Check.Metadata.Category.ToString(),
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Bold),
                    Margin = new MarginPadding(10)
                }
            };
        }
    }
}
