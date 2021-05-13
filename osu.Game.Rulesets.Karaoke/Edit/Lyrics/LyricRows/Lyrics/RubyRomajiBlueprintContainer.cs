// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints.Romajies;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Blueprints.Rubies;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics
{
    public class RubyRomajiBlueprintContainer : ExtendBlueprintContainer<ITextTag>
    {
        [UsedImplicitly]
        private readonly Bindable<RubyTag[]> rubyTags;

        [UsedImplicitly]
        private readonly Bindable<RomajiTag[]> romajiTags;

        protected readonly Lyric Lyric;

        public RubyRomajiBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
            rubyTags = lyric.RubyTagsBindable.GetBoundCopy();
            romajiTags = lyric.RomajiTagsBindable.GetUnboundCopy();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            // Add ruby and romaji tag into blueprint container
            RegistBindable(rubyTags);
            RegistBindable(romajiTags);
        }

        protected override SelectionHandler<ITextTag> CreateSelectionHandler()
            => new RubyRomajiSelectionHandler();

        protected override SelectionBlueprint<ITextTag> CreateBlueprintFor(ITextTag item)
        {
            switch (item)
            {
                case RubyTag rubyTag:
                    return new RubySelectionBlueprint(rubyTag);

                case RomajiTag romajiTag:
                    return new RomajiSelectionBlueprint(romajiTag);

                default:
                    throw new ArgumentOutOfRangeException(nameof(item));
            }
        }

        protected class RubyRomajiSelectionHandler : SelectionHandler<ITextTag>
        {
            [Resolved]
            private LyricManager lyricManager { get; set; }

            // for now we always allow movement. snapping is provided by the Timeline's "distance" snap implementation
            public override bool HandleMovement(MoveSelectionEvent<ITextTag> moveEvent) => true;

            protected override void DeleteItems(IEnumerable<ITextTag> items)
            {
                // todo : delete ruby or romaji
            }
        }
    }
}
