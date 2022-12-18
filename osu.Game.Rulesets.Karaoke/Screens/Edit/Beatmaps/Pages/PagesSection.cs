// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Components.UserInterface;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

public partial class PagesSection : EditorSection
{
    protected override LocalisableString Title => "Pages";

    private readonly IBindableList<Page> bindablePages = new BindableList<Page>();

    private FillFlowContainer? fillFlowContainer => Content as FillFlowContainer;

    public PagesSection()
    {
        if (fillFlowContainer != null)
        {
            fillFlowContainer.LayoutDuration = 100;
            fillFlowContainer.LayoutEasing = Easing.Out;
        }

        bindablePages.BindCollectionChanged((_, args) =>
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var obj in args.NewItems.OfType<Page>())
                    {
                        Add(new LabelledPage(obj)
                        {
                            DepthChanged = updatePagePosition
                        });
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var obj in args.OldItems.OfType<Page>())
                    {
                        var drawable = Children.OfType<LabelledPage>().FirstOrDefault(x => x.Page == obj);
                        if (drawable == null)
                            return;

                        Remove(drawable, true);
                    }

                    break;
            }
        });

        addCreateButton();
    }

    private void addCreateButton()
    {
        fillFlowContainer?.Insert(int.MaxValue, new CreateNewPageButton());
    }

    private void updatePagePosition(Drawable drawable, float newPosition)
    {
        fillFlowContainer?.SetLayoutPosition(drawable, newPosition);
    }

    [BackgroundDependencyLoader]
    private void load(IPageStateProvider pageStateProvider)
    {
        bindablePages.BindTo(pageStateProvider.PageInfo.Pages);
    }

    private partial class LabelledPage : CompositeDrawable
    {
        public readonly Page Page;

        public Action<LabelledPage, float>? DepthChanged;

        private readonly IBindable<int> pagesVersion = new Bindable<int>();

        [Resolved, AllowNull]
        private IBeatmapPagesChangeHandler beatmapPagesChangeHandler { get; set; }

        private readonly Box background;
        private readonly OsuSpriteText spriteText;
        private readonly DeleteIconButton deleteIconButton;

        public LabelledPage(Page page)
        {
            Page = page;

            Masking = true;
            CornerRadius = 5;
            RelativeSizeAxes = Axes.X;
            Height = 28;
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                spriteText = new OsuSpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Padding = new MarginPadding
                    {
                        Horizontal = 5,
                    },
                },
                deleteIconButton = new DeleteIconButton
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    X = -5,
                    Size = new Vector2(20),
                    Action = () =>
                    {
                        beatmapPagesChangeHandler.Remove(page);
                    },
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colour, IPageStateProvider pageStateProvider)
        {
            background.Colour = colour.YellowLight;
            spriteText.Colour = colour.YellowDarker;
            deleteIconButton.IconColour = colour.YellowDarker;

            pagesVersion.BindTo(pageStateProvider.PageInfo.PagesVersion);
            pagesVersion.BindValueChanged(_ =>
            {
                int? order = pageStateProvider.PageInfo.GetPageOrder(Page);
                double time = Page.Time;

                DepthChanged?.Invoke(this, (float)time);
                spriteText.Text = $"#{order} {time.ToEditorFormattedString()}";
            }, true);
        }
    }

    private partial class CreateNewPageButton : OsuButton
    {
        [Resolved, AllowNull]
        private IBeatmapPagesChangeHandler beatmapPagesChangeHandler { get; set; }

        [Resolved, AllowNull]
        private EditorClock clock { get; set; }

        public CreateNewPageButton()
        {
            RelativeSizeAxes = Axes.X;
            Content.CornerRadius = 15;
            Text = "Create new page";
            Action = () =>
            {
                double currentTime = clock.CurrentTime;
                beatmapPagesChangeHandler.Add(new Page
                {
                    Time = currentTime
                });
            };
        }
    }
}
