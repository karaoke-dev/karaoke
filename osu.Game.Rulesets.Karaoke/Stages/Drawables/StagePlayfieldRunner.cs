// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public class StagePlayfieldRunner : IStagePlayfieldRunner
{
    private IPlayfieldCommandProvider? commandProvider;
    private KaraokePlayfield? karaokePlayfield;

    public void UpdateCommandGenerator(IPlayfieldCommandProvider provider)
    {
        commandProvider = provider;
        applyTransforms();
    }

    public void UpdatePlayfieldTransforms(KaraokePlayfield playfield)
    {
        this.karaokePlayfield = playfield;

        applyTransforms();
    }

    private void applyTransforms()
    {
        if (commandProvider == null || karaokePlayfield == null)
            return;

        var lyricPlayfield = karaokePlayfield.LyricPlayfield;
        var notePlayfield = karaokePlayfield.NotePlayfield;

        applyTransforms(karaokePlayfield, commandProvider.GetCommands(karaokePlayfield));
        applyTransforms(lyricPlayfield, commandProvider.GetCommands(lyricPlayfield));
        applyTransforms(notePlayfield, commandProvider.GetCommands(notePlayfield));
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
