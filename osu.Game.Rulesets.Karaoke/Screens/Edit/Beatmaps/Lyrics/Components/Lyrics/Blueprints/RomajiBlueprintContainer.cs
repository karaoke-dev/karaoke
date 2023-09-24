// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics.Blueprints;

public partial class RomajiBlueprintContainer : TextTagBlueprintContainer<RomajiTag>
{
    public RomajiBlueprintContainer(Lyric lyric)
        : base(lyric)
    {
    }

    protected override BindableList<RomajiTag> GetProperties(Lyric lyric)
        => lyric.RomajiTagsBindable.GetBoundCopy();

    protected override SelectionHandler<RomajiTag> CreateSelectionHandler()
        => new RomajiTagSelectionHandler();

    protected override SelectionBlueprint<RomajiTag> CreateBlueprintFor(RomajiTag item)
        => new RomajiTagSelectionBlueprint(item);

    protected partial class RomajiTagSelectionHandler : TextTagSelectionHandler
    {
        [Resolved]
        private ILyricRomajiTagsChangeHandler romajiTagsChangeHandler { get; set; } = null!;

        [BackgroundDependencyLoader]
        private void load(IEditRomajiModeState editRomajiModeState)
        {
            SelectedItems.BindTo(editRomajiModeState.SelectedItems);
        }

        protected override void DeleteItems(IEnumerable<RomajiTag> items)
            => romajiTagsChangeHandler.RemoveRange(items);

        protected override void SetTextTagShifting(IEnumerable<RomajiTag> textTags, int offset)
            => romajiTagsChangeHandler.ShiftingIndex(textTags, offset);

        protected override void SetTextTagIndex(RomajiTag textTag, int? startPosition, int? endPosition)
            => romajiTagsChangeHandler.SetIndex(textTag, startPosition, endPosition);
    }
}
