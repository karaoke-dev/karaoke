// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Issues;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Panels;

public partial class IssueSection : PanelSection
{
    protected override LocalisableString Title => "Issues";

    private readonly IBindableList<Issue> bindableIssues = new BindableList<Issue>();

    [Resolved]
    private ILyricEditorVerifier verifier { get; set; } = null!;

    public IssueSection()
    {
        EmptyIssue emptyIssue;

        IconButton reloadButton;
        LyricEditorIssueTable issueTable;

        Children = new Drawable[]
        {
            emptyIssue = new EmptyIssue
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Padding = new MarginPadding(10),
            },
            issueTable = new SingleLyricIssueTable(),
        };

        AddInternal(reloadButton = new IconButton
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Icon = FontAwesome.Solid.Redo,
            Scale = new Vector2(0.7f),
            Action = () =>
            {
                if (Lyric == null)
                    throw new ArgumentNullException(nameof(Lyric));

                verifier.RefreshByHitObject(Lyric);
            },
        });

        bindableIssues.BindCollectionChanged((_, _) =>
        {
            bool hasIssue = bindableIssues.Any();

            emptyIssue.Alpha = hasIssue ? 0 : 1;

            reloadButton.Alpha = hasIssue ? 1 : 0;
            issueTable.Alpha = hasIssue ? 1 : 0;
            issueTable.Issues = bindableIssues.Take(100);
        }, true);
    }

    protected override void OnLyricChanged(Lyric? lyric)
    {
        bindableIssues.UnbindBindings();
        if (lyric == null)
            return;

        bindableIssues.BindTo(verifier.GetBindable(lyric));
    }

    // todo: change the style.
    private partial class EmptyIssue : ClickableContainer
    {
        [BackgroundDependencyLoader]
        private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state, ILyricEditorVerifier verifier, OsuColour colours)
        {
            Action = verifier.Refresh;

            InternalChild = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Padding = new MarginPadding(20),
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new SpriteIcon
                    {
                        Icon = FontAwesome.Solid.CheckCircle,
                        Colour = colours.Green,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Size = new Vector2(36),
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "No issue here!",
                        Colour = colourProvider.Colour1(state.Mode),
                        Font = OsuFont.GetFont(size: 24),
                    },
                    new OsuSpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Text = "Click this area to re-check again.",
                        Font = OsuFont.GetFont(size: 14),
                    },
                },
            };

            AddInternal(new HoverClickSounds(HoverSampleSet.Button));
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Content.ScaleTo(0.9f, 4000, Easing.OutQuint);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            Content.ScaleTo(1, 1000, Easing.OutElastic);
            base.OnMouseUp(e);
        }
    }

    private partial class SingleLyricIssueTable : LyricEditorIssueTable
    {
        public SingleLyricIssueTable()
        {
            ShowHeaders = false;
        }

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
                    Issue = issue,
                },
                new TruncatingSpriteText
                {
                    Text = issue.ToString(),
                    RelativeSizeAxes = Axes.X,
                    Font = OsuFont.GetFont(size: TEXT_SIZE, weight: FontWeight.Medium),
                },
            };
    }
}
