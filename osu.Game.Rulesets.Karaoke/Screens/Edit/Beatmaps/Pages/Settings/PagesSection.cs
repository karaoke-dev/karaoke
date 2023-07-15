// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Settings;

public partial class PagesSection : EditorSection
{
    protected override LocalisableString Title => "Pages";

    public PagesSection()
    {
        Add(new SectionPageInfoEditor());
    }

    private partial class SectionPageInfoEditor : SectionTimingInfoItemsEditor<Page>
    {
        [BackgroundDependencyLoader]
        private void load(IPageStateProvider pageStateProvider)
        {
            Items.BindTo(pageStateProvider.PageInfo.Pages);
        }

        protected override DrawableTimingInfoItem CreateTimingInfoDrawable(Page item) => new DrawablePage(item);

        protected override EditorSectionButton CreateCreateNewItemButton() => new CreateNewPageButton();

        private partial class DrawablePage : DrawableTimingInfoItem
        {
            private readonly IBindable<int> pagesVersion = new Bindable<int>();

            [Resolved]
            private IBeatmapPagesChangeHandler beatmapPagesChangeHandler { get; set; } = null!;

            public DrawablePage(Page item)
                : base(item)
            {
            }

            protected override void RemoveItem(Page item)
            {
                beatmapPagesChangeHandler.Remove(item);
            }

            [BackgroundDependencyLoader]
            private void load(IPageStateProvider pageStateProvider)
            {
                pagesVersion.BindTo(pageStateProvider.PageInfo.PagesVersion);
                pagesVersion.BindValueChanged(_ =>
                {
                    int? order = pageStateProvider.PageInfo.GetPageOrder(Item);
                    double time = Item.Time;

                    ChangeDisplayOrder((int)time);
                    Text = $"#{order} {time.ToEditorFormattedString()}";
                }, true);
            }
        }

        private partial class CreateNewPageButton : EditorSectionButton
        {
            [Resolved]
            private IBeatmapPagesChangeHandler beatmapPagesChangeHandler { get; set; } = null!;

            [Resolved]
            private EditorClock clock { get; set; } = null!;

            public CreateNewPageButton()
            {
                Text = "Create new page";
                Action = () =>
                {
                    double currentTime = clock.CurrentTime;
                    beatmapPagesChangeHandler.Add(new Page
                    {
                        Time = currentTime,
                    });
                };
            }
        }
    }
}
