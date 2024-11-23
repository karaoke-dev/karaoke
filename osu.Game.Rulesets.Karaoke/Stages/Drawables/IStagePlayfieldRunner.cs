// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.UI;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public interface IStagePlayfieldRunner
{
    /// <summary>
    /// Apply transforms to the main playfield and child playfield.
    /// </summary>
    /// <param name="playfield"></param>
    void UpdatePlayfieldTransforms(KaraokePlayfield playfield);
}
