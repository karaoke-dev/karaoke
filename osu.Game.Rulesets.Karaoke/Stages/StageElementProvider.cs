// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Stages;

public abstract class StageElementProvider<TStageInfo> : IStageElementProvider
{
    protected readonly TStageInfo StageInfo;

    protected readonly bool DisplayNotePlayfield;

    protected StageElementProvider(TStageInfo stageInfo, bool displayNotePlayfield)
    {
        StageInfo = stageInfo;
        DisplayNotePlayfield = displayNotePlayfield;
    }

    public abstract IEnumerable<IStageElement> GetElements();
}
