// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Stages;

public interface IPlayfieldCommandProvider
{
    /// <summary>
    /// Get the list of <see cref="IStageCommand"/> that apply to the  <see cref="Playfield"/>.
    /// </summary>
    /// <param name="playfield"></param>
    IEnumerable<IStageCommand> GetCommands(Playfield playfield);
}
