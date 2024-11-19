// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Stages;

public abstract class PlayfieldCommandProvider<TStageInfo> : IPlayfieldCommandProvider
    where TStageInfo : StageInfo
{
    protected readonly TStageInfo StageInfo;

    protected readonly bool DisplayNotePlayfield;

    protected PlayfieldCommandProvider(TStageInfo stageInfo, bool displayNotePlayfield)
    {
        StageInfo = stageInfo;
        DisplayNotePlayfield = displayNotePlayfield;
    }

    public IEnumerable<IStageCommand> GetCommands(Playfield playfield) =>
        playfield switch
        {
            KaraokePlayfield karaokePlayfield => GetMainPlayfieldCommands(karaokePlayfield),
            LyricPlayfield lyricPlayfield => GetLyricPlayfieldCommands(lyricPlayfield),
            NotePlayfield notePlayfield => GetNotePlayfieldCommands(notePlayfield),
            EditorNotePlayfield => Array.Empty<IStageCommand>(),
            _ => throw new InvalidCastException(),
        };

    protected abstract IEnumerable<IStageCommand> GetMainPlayfieldCommands(KaraokePlayfield playfield);

    protected abstract IEnumerable<IStageCommand> GetLyricPlayfieldCommands(LyricPlayfield playfield);

    protected abstract IEnumerable<IStageCommand> GetNotePlayfieldCommands(NotePlayfield playfield);
}
