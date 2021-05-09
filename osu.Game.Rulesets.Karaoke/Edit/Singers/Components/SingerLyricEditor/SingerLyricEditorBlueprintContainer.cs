// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Components.Timeline;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.SingerLyricEditor
{
    public class LyricBlueprintContainer : TimelineBlueprintContainer
    {
        private readonly Singer singer;

        public LyricBlueprintContainer(HitObjectComposer composer, Singer singer)
            : base(composer)
        {
            this.singer = singer;
        }

        protected override SelectionHandler<HitObject> CreateSelectionHandler()
            => new LyricTimelineSelectionHandler();

        protected override SelectionBlueprint<HitObject> CreateBlueprintFor(HitObject hitObject)
        {
            if (!(hitObject is Lyric))
                return null;

            return new LyricTimelineHitObjectBlueprint(hitObject, singer);
        }
    }
}
