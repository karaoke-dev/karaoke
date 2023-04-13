// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages.Classic;

public class LyricClassicStageEffectApplier : LyricStageEffectApplier
{
    public LyricClassicStageEffectApplier(IEnumerable<StageElement> elements, ClassicStageDefinition definition)
        : base(elements, definition)
    {
    }
}
