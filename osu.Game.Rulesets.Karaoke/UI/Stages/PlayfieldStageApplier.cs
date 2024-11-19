// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Stages.Drawables;
using osu.Game.Rulesets.Karaoke.Stages.Infos;

namespace osu.Game.Rulesets.Karaoke.UI.Stages;

/// <summary>
/// TODO: This class will be removed once <see cref="DrawableStage"/> support create custom drawable elements.
/// </summary>
/// <typeparam name="TStageDefinition"></typeparam>
public abstract class PlayfieldStageApplier<TStageDefinition> : IPlayfieldStageApplier
    where TStageDefinition : StageDefinition
{
    protected readonly TStageDefinition Definition;

    protected PlayfieldStageApplier(TStageDefinition definition)
    {
        Definition = definition;
    }

    public void UpdatePlayfieldArrangement(KaraokePlayfield playfield, bool displayNotePlayfield)
    {
        playfield.ClearTransforms();

        // Note that we should handle the fade-in effect in here.
        var playfieldTransformSequence = playfield.FadeOut().Then();

        UpdatePlayfieldArrangement(playfieldTransformSequence, displayNotePlayfield);
    }

    protected abstract void UpdatePlayfieldArrangement(TransformSequence<KaraokePlayfield> transformSequence, bool displayNotePlayfield);
}
