// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.UI.Stages;

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
        var lyricPlayfield = playfield.LyricPlayfield;
        var notePlayfield = playfield.NotePlayfield;

        playfield.ClearTransforms();
        lyricPlayfield.ClearTransforms();
        notePlayfield.ClearTransforms();

        // Note that we should handle the fade-in effect in here.
        var playfieldTransformSequence = playfield.FadeOut().Then();
        var lyricPlayfieldTransformSequence = lyricPlayfield.FadeOut().Then();
        var notePlayfieldTransformSequence = notePlayfield.FadeOut().Then();

        UpdatePlayfieldArrangement(playfieldTransformSequence, displayNotePlayfield);
        UpdateLyricPlayfieldArrangement(lyricPlayfieldTransformSequence, displayNotePlayfield);

        if (displayNotePlayfield)
        {
            UpdateNotePlayfieldArrangement(notePlayfieldTransformSequence);
        }
    }

    protected abstract void UpdatePlayfieldArrangement(TransformSequence<KaraokePlayfield> transformSequence, bool displayNotePlayfield);

    protected abstract void UpdateLyricPlayfieldArrangement(TransformSequence<LyricPlayfield> transformSequence, bool displayNotePlayfield);

    protected abstract void UpdateNotePlayfieldArrangement(TransformSequence<ScrollingNotePlayfield> transformSequence);
}
