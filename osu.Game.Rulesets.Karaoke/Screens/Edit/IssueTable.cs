// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Screens.Edit;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

[Cached]
public abstract partial class IssueTable : CompositeDrawable
{
    public IBindableList<Issue> Issues { get; } = new BindableList<Issue>();

    public const float ROW_HEIGHT = 25;
    public const int TEXT_SIZE = 14;

    protected abstract Dimension[] CreateDimensions();

    protected abstract IssueTableHeaderText[] CreateHeaders();

    protected abstract Tuple<Drawable[], Action<Issue>> CreateContent();

    protected abstract Color4 GetBackgroundColour(bool selected);

    protected abstract void OnIssueClicked(Issue issue);

    [BackgroundDependencyLoader]
    private void load()
    {
        InternalChildren = new Drawable[]
        {
            new GridContainer
            {
                RelativeSizeAxes = Axes.X,
                Height = ROW_HEIGHT,
                RowDimensions = CreateDimensions(),
                Content = new[]
                {
                    CreateHeaders(),
                },
            },
            new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Top = ROW_HEIGHT, },
                Child = new IssueRowList
                {
                    RelativeSizeAxes = Axes.Both,
                    RowData = { BindTarget = Issues },
                },
            },
        };
    }

    private bool showHeader;

    public bool ShowHeaders
    {
        get => showHeader;
        set
        {
            showHeader = value;

            // todo: show/hide the header.
        }
    }

    protected partial class IssueTableHeaderText : TableHeaderText
    {
        public IssueTableHeaderText(LocalisableString text, Anchor anchor)
            : base(text)
        {
            Anchor = anchor;
            Origin = anchor;
        }
    }

    private sealed partial class IssueRowList : VirtualisedListContainer<Issue, DrawableIssue>
    {
        public IssueRowList()
            : base(ROW_HEIGHT, 50)
        {
        }

        protected sealed override ScrollContainer<Drawable> CreateScrollContainer() => new OsuScrollContainer();
    }

    private partial class DrawableIssue : PoolableDrawable, IHasCurrentValue<Issue>
    {
        private readonly BindableWithCurrent<Issue> current = new();

        public Action<Issue>? Selected;

        private Box background = null!;

        public Bindable<Issue> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        [Resolved]
        private IssueTable issueTable { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            Height = ROW_HEIGHT;

            var (drawables, action) = issueTable.CreateContent();
            Selected = action;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Horizontal = 20, },
                    RowDimensions = issueTable.CreateDimensions(),
                    Content = new[]
                    {
                        drawables,
                    },
                },
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Current.BindValueChanged(_ => updateState(), true);
            FinishTransforms(true);
        }

        protected override bool OnHover(HoverEvent e)
        {
            updateState();
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            updateState();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            Selected?.Invoke(current.Value);
            updateState();

            return true;
        }

        private void updateState()
        {
            // todo: implement
            bool isSelected = true;

            if (IsHovered || isSelected)
                background.FadeIn(100, Easing.OutQuint);
            else
                background.FadeOut(100, Easing.OutQuint);

            background.Colour = issueTable.GetBackgroundColour(isSelected);
        }
    }
}
