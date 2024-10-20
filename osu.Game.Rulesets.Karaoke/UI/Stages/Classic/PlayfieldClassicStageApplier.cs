// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
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
        transformSequence.FadeIn(300);
    }

    protected override void UpdateLyricPlayfieldArrangement(TransformSequence<LyricPlayfield> transformSequence, bool displayNotePlayfield)
    {
        // todo: adjust the lyric playfield size if contains note playfield.
        transformSequence.FadeIn(100);
    }

    protected override void UpdateNotePlayfieldArrangement(TransformSequence<ScrollingNotePlayfield> transformSequence)
    {
        transformSequence.TransformTo(nameof(LyricPlayfield.Anchor), Anchor.Centre)
                         .TransformTo(nameof(LyricPlayfield.Origin), Anchor.Centre)
                         .MoveToY(-200)
                         .TransformTo(nameof(LyricPlayfield.Padding), new MarginPadding(50))
                         .Then()
                         .FadeIn(100);
    }
}
