// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.UI;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;

public class PreviewPlayfieldCommandProvider : PlayfieldCommandProvider<PreviewStageInfo>
{
    public PreviewPlayfieldCommandProvider(PreviewStageInfo stageInfo, bool displayNotePlayfield)
        : base(stageInfo, displayNotePlayfield)
    {
    }

    protected override IEnumerable<IStageCommand> GetMainPlayfieldCommands(KaraokePlayfield playfield)
    {
        yield break;
    }

    protected override IEnumerable<IStageCommand> GetLyricPlayfieldCommands(LyricPlayfield playfield)
    {
        int xPosition = DisplayNotePlayfield ? -190 : 0;
        int yPosition = DisplayNotePlayfield ? 0 : -32;

        yield return new StageAnchorCommand(Easing.None, 0, 0, Anchor.Centre, Anchor.Centre);
        yield return new StageOriginCommand(Easing.None, 0, 0, Anchor.TopLeft, Anchor.TopLeft);
        yield return new StageXCommand(Easing.None, 0, 0, xPosition, xPosition);
        yield return new StageYCommand(Easing.None, 0, 0, yPosition, yPosition);
        yield return new StageAlphaCommand(Easing.In, 0, 100, 0, 1);
    }

    protected override IEnumerable<IStageCommand> GetNotePlayfieldCommands(NotePlayfield playfield)
    {
        yield return new StageAnchorCommand(Easing.None, 0, 0, Anchor.Centre, Anchor.Centre);
        yield return new StageOriginCommand(Easing.None, 0, 0, Anchor.Centre, Anchor.Centre);
        yield return new StageYCommand(Easing.None, 0, 0, -200, -200);
        yield return new StagePaddingCommand(Easing.None, 0, 0, new MarginPadding(50), new MarginPadding(50));
        yield return new StageAlphaCommand(Easing.In, 0, 100, 0, 1);
    }
}
