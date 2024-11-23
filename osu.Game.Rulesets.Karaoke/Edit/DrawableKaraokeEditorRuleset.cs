// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Edit;

public partial class DrawableKaraokeEditorRuleset : DrawableKaraokeRuleset
{
    public new IScrollingInfo ScrollingInfo => base.ScrollingInfo;

    protected override bool DisplayNotePlayfield => true;

    public DrawableKaraokeEditorRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod>? mods)
        : base(ruleset, beatmap, mods)
    {
    }

    protected override Playfield CreatePlayfield() => new KaraokeEditorPlayfield();

    public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer()
    {
        bool isCalledByComposer = StackTraceUtils.IsStackTraceContains("DrawableEditorRulesetWrapper");
        if (isCalledByComposer)
            return new PlayfieldAdjustmentContainer();

        return base.CreatePlayfieldAdjustmentContainer();
    }
}
