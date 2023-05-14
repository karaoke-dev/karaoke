// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.ContextMenu;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Timeline;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Rows.Components.Timeline;

public partial class SingerLyricEditorBlueprintContainer : EditableTimelineBlueprintContainer<Lyric>
{
    [BackgroundDependencyLoader]
    private void load(ILyricsProvider lyricsProvider)
    {
        Items.BindTo(lyricsProvider.BindableLyrics);
    }

    protected override IEnumerable<SelectionBlueprint<Lyric>> SortForMovement(IReadOnlyList<SelectionBlueprint<Lyric>> blueprints)
        => blueprints.OrderBy(b => b.Item.LyricStartTime);

    protected override SelectionHandler<Lyric> CreateSelectionHandler()
        => new SingerLyricSelectionHandler();

    protected override SelectionBlueprint<Lyric> CreateBlueprintFor(Lyric item)
        => new LyricTimelineSelectionBlueprint(item);

    protected partial class SingerLyricSelectionHandler : EditableTimelineSelectionHandler
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; } = null!;

        [Resolved]
        private ILyricSingerChangeHandler lyricSingerChangeHandler { get; set; } = null!;

        [Resolved]
        private BindableList<Lyric> selectedLyrics { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectedItems.BindTo(selectedLyrics);
        }

        protected override IEnumerable<MenuItem> GetContextMenuItemsForSelection(IEnumerable<SelectionBlueprint<Lyric>> selection)
        {
            var contextMenu = new SingerContextMenu(beatmap, lyricSingerChangeHandler, string.Empty, () =>
            {
                selectedLyrics.Clear();
            });
            return contextMenu.Items;
        }

        protected override void DeleteItems(IEnumerable<Lyric> items)
        {
            // todo : remove all in the same time.
            foreach (var item in items)
            {
                lyricSingerChangeHandler.Clear();
            }

            selectedLyrics.Clear();
        }
    }
}
