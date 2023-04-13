// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages.Preview;

public class NotePreviewStageEffectApplier : NoteStageEffectApplier
{
    public NotePreviewStageEffectApplier(IEnumerable<StageElement> elements, PreviewStageDefinition definition)
        : base(elements, definition)
    {
    }
}
