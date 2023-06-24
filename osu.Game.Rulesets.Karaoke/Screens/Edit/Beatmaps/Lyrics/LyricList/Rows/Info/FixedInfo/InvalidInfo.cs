// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.LyricList.Rows.Info.FixedInfo;

public partial class InvalidInfo : SpriteIcon, IHasCustomTooltip<Issue[]>, IHasPopover
{
    private readonly IBindableList<Issue> bindableIssues = new BindableList<Issue>();
    private readonly Lyric lyric;

    public InvalidInfo(Lyric lyric)
    {
        this.lyric = lyric;

        Size = new Vector2(12);
    }

    [BackgroundDependencyLoader]
    private void load(OsuColour colours, ILyricEditorVerifier verifier)
    {
        bindableIssues.BindTo(verifier.GetBindable(lyric));
        bindableIssues.BindCollectionChanged((_, args) =>
        {
            TooltipContent = bindableIssues.ToArray();

            var issue = getDisplayIssue(bindableIssues);

            if (issue == null)
            {
                Icon = FontAwesome.Solid.CheckCircle;
                Colour = colours.Green;
                return;
            }

            var displayIssueType = issue.Template.Type;
            var targetColour = issue.Template.Colour;

            switch (displayIssueType)
            {
                case IssueType.Problem:
                    Icon = FontAwesome.Solid.TimesCircle;
                    Colour = targetColour;
                    break;

                case IssueType.Warning:
                    Icon = FontAwesome.Solid.ExclamationCircle;
                    Colour = targetColour;
                    break;

                case IssueType.Error: // it's caused by internal error.
                    Icon = FontAwesome.Solid.ExclamationTriangle;
                    Colour = targetColour;
                    break;

                case IssueType.Negligible:
                    Icon = FontAwesome.Solid.InfoCircle;
                    Colour = targetColour;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }, true);
    }

    private static Issue? getDisplayIssue(IReadOnlyList<Issue> issues)
    {
        if (!issues.Any())
            return null;

        return issues.OrderByDescending(x => x.Template.Type).First();
    }

    protected override bool OnClick(ClickEvent e)
    {
        if (bindableIssues.Any())
        {
            this.ShowPopover();
        }

        return base.OnClick(e);
    }

    public ITooltip<Issue[]> GetCustomTooltip()
        => new IssuesToolTip();

    public Issue[]? TooltipContent { get; private set; }

    public Popover GetPopover()
        => new IssueTablePopover(this, bindableIssues);

    private partial class IssueTablePopover : OsuPopover
    {
        private readonly CompositeDrawable parentDrawable;

        public IssueTablePopover(CompositeDrawable parent, IReadOnlyCollection<Issue> issues)
        {
            parentDrawable = parent;

            Child = new Container
            {
                Width = 300,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    new SingleLyricIssueTable
                    {
                        Issues = issues
                    },
                    new IconButton
                    {
                        Anchor = Anchor.TopRight,
                        Origin = Anchor.TopRight,
                        Icon = FontAwesome.Solid.Redo,
                        Scale = new Vector2(0.7f),
                        Action = () =>
                        {
                            var verifier = parentDrawable.Dependencies.Get<ILyricEditorVerifier>();
                            verifier.Refresh();

                            // should close the popover if has no issue.
                            if (!issues.Any())
                                this.HidePopover();
                        }
                    }
                }
            };
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

            dependencies.Cache(parentDrawable.Dependencies.Get<LyricEditorColourProvider>());
            dependencies.CacheAs(parentDrawable.Dependencies.Get<ILyricEditorState>());
            dependencies.CacheAs(parentDrawable.Dependencies.Get<IIssueNavigator>());

            return dependencies;
        }

        private partial class SingleLyricIssueTable : LyricEditorIssueTable
        {
            protected override TableColumn[] CreateHeaders() => new[]
            {
                new TableColumn(string.Empty, Anchor.CentreLeft, new Dimension(GridSizeMode.AutoSize, minSize: 30)),
                new TableColumn("Message", Anchor.CentreLeft),
            };

            protected override Drawable[] CreateContent(Issue issue) =>
                new Drawable[]
                {
                    new IssueIcon
                    {
                        Origin = Anchor.Centre,
                        Size = new Vector2(10),
                        Margin = new MarginPadding { Left = 10 },
                        Issue = issue
                    },
                    new TruncatingSpriteText
                    {
                        Text = issue.ToString(),
                        RelativeSizeAxes = Axes.X,
                        Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium),
                        ShowTooltip = false,
                    },
                };

            protected override void OnIssueClicked(Issue issue)
            {
                base.OnIssueClicked(issue);

                this.HidePopover();
            }
        }
    }
}
