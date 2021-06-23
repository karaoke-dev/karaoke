// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Blueprints;
using osu.Game.Rulesets.Karaoke.Objects;

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
        private void load()
        {
            // Add ruby tag into blueprint container
            RegistBindable(rubyTags);
        }

        protected override SelectionBlueprint<RubyTag> CreateBlueprintFor(RubyTag item)
            => new RubyTagSelectionBlueprint(item);
    }
}
