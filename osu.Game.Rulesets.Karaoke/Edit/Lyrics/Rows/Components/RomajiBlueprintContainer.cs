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
    public class RomajiBlueprintContainer : TextTagBlueprintContainer<RomajiTag>
    {
        [UsedImplicitly]
        private readonly Bindable<RomajiTag[]> romajiTags;

        public RomajiBlueprintContainer(Lyric lyric)
            : base(lyric)
        {
            romajiTags = lyric.RomajiTagsBindable.GetBoundCopy();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            // Add romaji tag into blueprint container
            RegistBindable(romajiTags);
        }

        protected override SelectionBlueprint<RomajiTag> CreateBlueprintFor(RomajiTag item)
            => new RomajiTagSelectionBlueprint(item);
    }
}
