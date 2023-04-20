// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;
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
            Size = new Vector2(displayNotePlayfield ? 200 : 300),
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            X = -200,
            Y = displayNotePlayfield ? 100 : 0,
        }).FadeIn(300);
    }

    protected override void UpdateLyricPlayfieldArrangement(TransformSequence<LyricPlayfield> transformSequence, bool displayNotePlayfield)
    {
        transformSequence.TransformTo(nameof(LyricPlayfield.Anchor), Anchor.BottomRight)
                         .TransformTo(nameof(LyricPlayfield.Origin), Anchor.BottomRight)
                         .TransformTo(nameof(LyricPlayfield.Size), new Vector2(0.5f))
                         .Then()
                         .FadeIn(100);
    }

    protected override void UpdateNotePlayfieldArrangement(TransformSequence<ScrollingNotePlayfield> transformSequence)
    {
        transformSequence.TransformTo(nameof(LyricPlayfield.Padding), new MarginPadding(50))
                         .Then()
                         .FadeIn(100);
    }
}
