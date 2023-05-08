// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
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
    [Resolved]
    private IBeatmapPagesChangeHandler beatmapPagesChangeHandler { get; set; } = null!;

    [BackgroundDependencyLoader]
    private void load(IPageStateProvider pageStateProvider)
    {
        Items.BindTo(pageStateProvider.PageInfo.Pages);
    }

    protected override bool ApplyOffsetResult(Page[] items, double time)
    {
        double offset = time - items.First().Time;
        beatmapPagesChangeHandler.ShiftingPageTime(items, offset);
        return true;
    }

    protected override IEnumerable<SelectionBlueprint<Page>> SortForMovement(IReadOnlyList<SelectionBlueprint<Page>> blueprints)
        => blueprints.OrderBy(b => b.Item.Time);

    protected override SelectionHandler<Page> CreateSelectionHandler()
        => new PageSelectionHandler();

    protected override SelectionBlueprint<Page> CreateBlueprintFor(Page item)
        => new PageSelectionBlueprint(item);

    protected partial class PageSelectionHandler : EditableTimelineSelectionHandler
    {
        [Resolved]
        private IBeatmapPagesChangeHandler beatmapPagesChangeHandler { get; set; } = null!;

        [Resolved]
        private IPageStateProvider pageStateProvider { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(pageStateProvider.SelectedItems);
        }

        // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
        public override bool HandleMovement(MoveSelectionEvent<Page> moveEvent) => true;

        protected override void DeleteItems(IEnumerable<Page> items)
        {
            beatmapPagesChangeHandler.RemoveRange(items);
        }
    }
}
