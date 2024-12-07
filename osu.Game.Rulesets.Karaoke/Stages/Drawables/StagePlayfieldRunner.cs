// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public class StagePlayfieldRunner : StageRunner, IStagePlayfieldRunner
{
    private IPlayfieldCommandProvider? commandProvider;
    private IList<IApplicableToStagePlayfieldCommand>? stageMods;
    private KaraokePlayfield? karaokePlayfield;

    public override void OnStageInfoChanged(StageInfo stageInfo, bool scorable, IReadOnlyList<Mod> mods)
    {
        commandProvider = stageInfo.CreatePlayfieldCommandProvider(scorable);
        stageMods = mods.OfType<IApplicableToStagePlayfieldCommand>().Where(x => x.CanApply(stageInfo)).ToList();
        applyTransforms();
    }

    public override void TriggerUpdateCommand()
    {
        applyTransforms();
    }

    public void UpdatePlayfieldTransforms(KaraokePlayfield playfield)
    {
        karaokePlayfield = playfield;

        applyTransforms();
    }

    private void applyTransforms()
    {
        if (karaokePlayfield == null)
            return;

        var lyricPlayfield = karaokePlayfield.LyricPlayfield;
        var notePlayfield = karaokePlayfield.NotePlayfield;

        applyTransforms(karaokePlayfield, getCommand(karaokePlayfield));
        applyTransforms(lyricPlayfield, getCommand(lyricPlayfield));
        applyTransforms(notePlayfield, getCommand(notePlayfield));
    }

    private IEnumerable<IStageCommand> getCommand(
        Playfield playfield)
    {
        if (commandProvider == null)
            return Array.Empty<IStageCommand>();

        var commands = commandProvider.GetCommands(playfield);

        if (stageMods == null)
            return commands;

        return stageMods.Aggregate(commands, (current, mod) => mod.PostProcessCommands(playfield, current));
    }

    private static void applyTransforms<TDrawable>(TDrawable drawable, IEnumerable<IStageCommand> commands)
        where TDrawable : Playfield
    {
        drawable.ClearTransforms();

        var appliedProperties = new HashSet<string>();

        foreach (var command in commands.OrderBy(c => c.StartTime))
        {
            if (appliedProperties.Add(command.PropertyName))
                command.ApplyInitialValue(drawable);

            using (drawable.BeginAbsoluteSequence(command.StartTime))
                command.ApplyTransforms(drawable);
        }
    }
}
