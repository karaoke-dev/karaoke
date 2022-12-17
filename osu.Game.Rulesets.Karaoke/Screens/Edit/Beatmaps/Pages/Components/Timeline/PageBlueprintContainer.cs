// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Components.Timeline;

public partial class PageBlueprintContainer : EditableTimelineBlueprintContainer<Page>
{
    [BackgroundDependencyLoader]
    private void load(IPageStateProvider pageStateProvider)
    {
        Items.BindTo(pageStateProvider.PageInfo.Pages);
    }

    protected override IEnumerable<SelectionBlueprint<Page>> SortForMovement(IReadOnlyList<SelectionBlueprint<Page>> blueprints)
        => blueprints.OrderBy(b => b.Item.Time);

    protected override SelectionHandler<Page> CreateSelectionHandler()
        => new PageSelectionHandler();

    protected override SelectionBlueprint<Page> CreateBlueprintFor(Page item)
        => new PageSelectionBlueprint(item);

    protected partial class PageSelectionHandler : EditableTimelineSelectionHandler
    {
        [Resolved, AllowNull]
        private IBeatmapPagesChangeHandler beatmapPagesChangeHandler { get; set; }

        [Resolved, AllowNull]
        private IPageStateProvider pageStateProvider { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(pageStateProvider.SelectedItems);
        }

        protected override void DeleteItems(IEnumerable<Page> items)
        {
            beatmapPagesChangeHandler.RemoveRange(items);
        }
    }
}
