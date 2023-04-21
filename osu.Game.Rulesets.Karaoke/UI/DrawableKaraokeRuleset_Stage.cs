// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;

namespace osu.Game.Rulesets.Karaoke.UI;

public partial class DrawableKaraokeRuleset
{
    private readonly IBindable<StageInfo> currentStageInfo = new Bindable<StageInfo>();
    private readonly IBindable<bool> scorable = new Bindable<bool>();

    private void updatePlayfieldArrangement(StageInfo stageInfo)
    {
        var applier = stageInfo.GetPlayfieldStageApplier();
        applier.UpdatePlayfieldArrangement(Playfield, DisplayNotePlayfield);
    }
}
