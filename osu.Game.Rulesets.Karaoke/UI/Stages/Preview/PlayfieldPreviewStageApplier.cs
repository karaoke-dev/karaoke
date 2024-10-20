// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.Stages.Preview;

public class PlayfieldPreviewStageApplier : PlayfieldStageApplier<PreviewStageDefinition>
{
    public PlayfieldPreviewStageApplier(PreviewStageDefinition definition)
        : base(definition)
    {
    }

    protected override void UpdatePlayfieldArrangement(TransformSequence<KaraokePlayfield> transformSequence, bool displayNotePlayfield)
    {
    }
}
