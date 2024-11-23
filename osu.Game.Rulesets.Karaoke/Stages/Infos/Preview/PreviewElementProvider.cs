// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Stages.Commands;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;

public class PreviewElementProvider : StageElementProvider<PreviewStageInfo>
{
    public PreviewElementProvider(PreviewStageInfo stageInfo, bool displayNotePlayfield)
        : base(stageInfo, displayNotePlayfield)
    {
    }

    public override IEnumerable<IStageElement> GetElements()
    {
        int size = DisplayNotePlayfield ? 200 : 380;
        int x = DisplayNotePlayfield ? -360 : -270;
        int y = DisplayNotePlayfield ? 100 : 0;

        yield return new StageBeatmapCoverInfo
        {
            Commands = new IStageCommand[]
            {
                new StageWidthCommand(Easing.None, 0, 0, size, size),
                new StageHeightCommand(Easing.None, 0, 0, size, size),
                new StageAnchorCommand(Easing.None, 0, 0, Anchor.Centre, Anchor.Centre),
                new StageOriginCommand(Easing.None, 0, 0, Anchor.Centre, Anchor.Centre),
                new StageXCommand(Easing.None, 0, 0, x, x),
                new StageYCommand(Easing.None, 0, 0, y, y),
            }
        };
    }
}
