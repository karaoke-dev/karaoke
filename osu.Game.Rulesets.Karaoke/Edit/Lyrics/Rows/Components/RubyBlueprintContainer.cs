// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Blueprints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components
{
    public class RubyBlueprintContainer : TextTagBlueprintContainer<RubyTag>
    {
        [UsedImplicitly]
        private readonly Bindable<RubyTag[]> rubyTags;

        public RubyBlueprintContainer(Lyric lyric)
            : base(lyric)
        {
            rubyTags = lyric.RubyTagsBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            // Add ruby tag into blueprint container
            SelectedItems.BindTo(state.SelectedRubyTags);
            RegistBindable(rubyTags);
        }

        protected override SelectionHandler<RubyTag> CreateSelectionHandler()
            => new RubyTagSelectionHandler();

        protected override SelectionBlueprint<RubyTag> CreateBlueprintFor(RubyTag item)
            => new RubyTagSelectionBlueprint(item);

        protected class RubyTagSelectionHandler : TextTagSelectionHandler
        {
            [BackgroundDependencyLoader]
            private void load(ILyricEditorState state)
            {
                SelectedItems.BindTo(state.SelectedRubyTags);
            }
        }
    }
}
