// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

[Cached(typeof(IPageStateProvider))]
public partial class PageScreen : BeatmapEditorRoundedScreen, IPageStateProvider
{
    [Cached(typeof(IBeatmapPagesChangeHandler))]
    private readonly BeatmapPagesChangeHandler beatmapPagesChangeHandler;

    public IBindable<PageEditorEditMode> BindableEditMode => bindableEditMode;

    private readonly Bindable<PageEditorEditMode> bindableEditMode = new();

    public PageScreen()
        : base(KaraokeBeatmapEditorScreenMode.Page)
    {
        AddInternal(beatmapPagesChangeHandler = new BeatmapPagesChangeHandler());
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Add(new FixedSectionsContainer<Drawable>
        {
            FixedHeader = new PageScreenHeader(),
            RelativeSizeAxes = Axes.Both,
            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(),
                    new Dimension(GridSizeMode.Absolute, 250),
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        new PageEditor
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        new PageSettings(),
                    },
                }
            }
        });
    }

    public void ChangeEditMode(PageEditorEditMode mode)
    {
        bindableEditMode.Value = mode;
    }

    private partial class FixedSectionsContainer<T> : SectionsContainer<T> where T : Drawable
    {
        private readonly Container<T> content;

        // todo: check what this shit doing.
        protected override Container<T> Content => content;

        public FixedSectionsContainer()
        {
            AddInternal(content = new Container<T>
            {
                Masking = true,
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Top = 55 }
            });
        }
    }

    private partial class PageScreenHeader : OverlayHeader
    {
        protected override OverlayTitle CreateTitle() => new PageScreenTitle();

        private partial class PageScreenTitle : OverlayTitle
        {
            public PageScreenTitle()
            {
                Title = "page";
                Description = "create page of your beatmap";
                IconTexture = "Icons/Hexacons/social";
            }
        }
    }
}
