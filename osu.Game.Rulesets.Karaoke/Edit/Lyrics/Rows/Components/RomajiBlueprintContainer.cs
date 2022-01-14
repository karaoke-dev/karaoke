// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Blueprints;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components
{
    public class RomajiBlueprintContainer : TextTagBlueprintContainer<RomajiTag>
    {
        [Resolved]
        private ILyricRomajiTagsChangeHandler romajiTagsChangeHandler { get; set; }

        [UsedImplicitly]
        private readonly Bindable<RomajiTag[]> romajiTags;

        public RomajiBlueprintContainer(Lyric lyric)
            : base(lyric)
        {
            romajiTags = lyric.RomajiTagsBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load(IBlueprintSelectionState blueprintSelectionState)
        {
            // Add romaji tag into blueprint container
            SelectedItems.BindTo(blueprintSelectionState.SelectedRomajiTags);
            RegisterBindable(romajiTags);
        }

        protected override SelectionHandler<RomajiTag> CreateSelectionHandler()
            => new RomajiTagSelectionHandler();

        protected override SelectionBlueprint<RomajiTag> CreateBlueprintFor(RomajiTag item)
            => new RomajiTagSelectionBlueprint(item);

        protected override void SetTextTagPosition(RomajiTag textTag, int startPosition, int endPosition)
            => romajiTagsChangeHandler.SetIndex(textTag, startPosition, endPosition);

        protected class RomajiTagSelectionHandler : TextTagSelectionHandler
        {
            [Resolved]
            private ILyricRomajiTagsChangeHandler romajiTagsChangeHandler { get; set; }

            [BackgroundDependencyLoader]
            private void load(IBlueprintSelectionState blueprintSelectionState)
            {
                SelectedItems.BindTo(blueprintSelectionState.SelectedRomajiTags);
            }

            protected override void DeleteItems(IEnumerable<RomajiTag> items)
                => romajiTagsChangeHandler.RemoveAll(items);

            protected override void SetTextTagPosition(RomajiTag textTag, int? startPosition, int? endPosition)
                => romajiTagsChangeHandler.SetIndex(textTag, startPosition, endPosition);
        }
    }
}
