// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.UI;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

public class ClassicPlayfieldCommandProvider : PlayfieldCommandProvider<ClassicStageInfo>
{
    public ClassicPlayfieldCommandProvider(ClassicStageInfo stageInfo, bool displayNotePlayfield)
        : base(stageInfo, displayNotePlayfield)
    {
    }

    protected override IEnumerable<IStageCommand> GetMainPlayfieldCommands(KaraokePlayfield playfield)
    {
        yield break;
    }

    protected override IEnumerable<IStageCommand> GetLyricPlayfieldCommands(LyricPlayfield playfield)
    {
        yield return new StageAlphaCommand(Easing.In, 0, 100, 0, 1);
    }

    protected override IEnumerable<IStageCommand> GetNotePlayfieldCommands(NotePlayfield playfield)
    {
        yield return new StageAnchorCommand(Easing.None, 0, 0, Anchor.Centre, Anchor.Centre);
        yield return new StageOriginCommand(Easing.None, 0, 0, Anchor.Centre, Anchor.Centre);
        yield return new StageYCommand(Easing.None, 0, 0, 0, -200);
        yield return new StagePaddingCommand(Easing.None, 0, 0, new MarginPadding(50), new MarginPadding(50));
        yield return new StageAlphaCommand(Easing.In, 0, 200, 0, 1);
    }
}
