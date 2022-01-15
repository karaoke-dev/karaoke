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
    public class RubyBlueprintContainer : TextTagBlueprintContainer<RubyTag>
    {
        [UsedImplicitly]
        private readonly BindableList<RubyTag> rubyTags;

        public RubyBlueprintContainer(Lyric lyric)
            : base(lyric)
        {
            rubyTags = lyric.RubyTagsBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load(IBlueprintSelectionState blueprintSelectionState)
        {
            // Add ruby tag into blueprint container
            SelectedItems.BindTo(blueprintSelectionState.SelectedRubyTags);
            RegisterBindable(rubyTags);
        }

        protected override SelectionHandler<RubyTag> CreateSelectionHandler()
            => new RubyTagSelectionHandler();

        protected override SelectionBlueprint<RubyTag> CreateBlueprintFor(RubyTag item)
            => new RubyTagSelectionBlueprint(item);

        protected class RubyTagSelectionHandler : TextTagSelectionHandler
        {
            [Resolved]
            private ILyricRubyTagsChangeHandler rubyTagsChangeHandler { get; set; }

            [BackgroundDependencyLoader]
            private void load(IBlueprintSelectionState blueprintSelectionState)
            {
                SelectedItems.BindTo(blueprintSelectionState.SelectedRubyTags);
            }

            protected override void DeleteItems(IEnumerable<RubyTag> items)
                => rubyTagsChangeHandler.RemoveAll(items);

            protected override void SetTextTagShifting(IEnumerable<RubyTag> textTags, int offset)
                => rubyTagsChangeHandler.ShiftingIndex(textTags, offset);

            protected override void SetTextTagIndex(RubyTag textTag, int? startPosition, int? endPosition)
                => rubyTagsChangeHandler.SetIndex(textTag, startPosition, endPosition);
        }
    }
}
