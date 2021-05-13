// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics
{
    public class RubyRomajiBlueprintContainer : BlueprintContainer<ITextTag>
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

        protected override SelectionHandler<ITextTag> CreateSelectionHandler()
        {
            throw new System.NotImplementedException();
        }
    }
}
