// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
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
        transformSequence.TransformAddStageComponent(new BeatmapCoverInfo
        {
            Size = new Vector2(displayNotePlayfield ? 200 : 380),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            X = displayNotePlayfield ? -360 : -270,
            Y = displayNotePlayfield ? 100 : 0,
        }).FadeIn(300);
    }

    protected override void UpdateLyricPlayfieldArrangement(TransformSequence<LyricPlayfield> transformSequence, bool displayNotePlayfield)
    {
        transformSequence.TransformTo(nameof(LyricPlayfield.Anchor), Anchor.Centre)
                         .TransformTo(nameof(LyricPlayfield.Origin), Anchor.TopLeft)
                         .TransformTo(nameof(LyricPlayfield.Size), new Vector2(0.5f))
                         .MoveToX(displayNotePlayfield ? -190 : 0) // lyric and the beatmap cover should be closer if has note playfield.
                         .MoveToY(displayNotePlayfield ? 0 : -32) // lyric playfield should be upper if there's no note playfield.
                         .Then()
                         .FadeIn(100);
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
