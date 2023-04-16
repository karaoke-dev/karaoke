// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.UI.Stages.Preview;

public class PlayfieldPreviewStageApplier : PlayfieldStageApplier<PreviewStageDefinition>
{
    public PlayfieldPreviewStageApplier(PreviewStageDefinition definition)
        : base(definition)
    {
    }

    protected override void UpdateLyricPlayfieldArrangement(TransformSequence<LyricPlayfield> transformSequence, bool displayNotePlayfield)
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateNotePlayfieldArrangement(TransformSequence<ScrollingNotePlayfield> transformSequence)
    {
        throw new System.NotImplementedException();
    }
}
