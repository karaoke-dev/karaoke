// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.UI.Stages.Classic;

public class PlayfieldClassicStageApplier : PlayfieldStageApplier<ClassicStageDefinition>
{
    public PlayfieldClassicStageApplier(ClassicStageDefinition definition)
        : base(definition)
    {
    }

    protected override void UpdatePlayfieldArrangement(TransformSequence<KaraokePlayfield> transformSequence, bool displayNotePlayfield)
    {
        throw new System.NotImplementedException();
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
