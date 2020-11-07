// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Components.Timeline;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components.Timeline
{
    public class LyricBlueprintContainer : TimelineBlueprintContainer
    {
        protected override void AddBlueprintFor(HitObject hitObject)
        {
            if (!(hitObject is Lyric))
                return;

            // todo : check lyric is at the same singer.

            base.AddBlueprintFor(hitObject);
        }

        protected override SelectionHandler CreateSelectionHandler()
            => new LyricTimelineSelectionHandler();

        protected override SelectionBlueprint CreateBlueprintFor(HitObject hitObject)
            => new LyricTimelineHitObjectBlueprint(hitObject);

        internal class LyricTimelineSelectionHandler : TimelineSelectionHandler
        {
            // does not allow movement
            public override bool HandleMovement(MoveSelectionEvent moveEvent) => false;
        }
    }
}
