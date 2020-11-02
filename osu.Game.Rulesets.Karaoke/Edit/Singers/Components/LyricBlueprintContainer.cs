// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Components.Timeline;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components
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
    }
}
