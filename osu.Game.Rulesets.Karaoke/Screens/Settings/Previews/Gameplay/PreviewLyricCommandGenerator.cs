// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Preview;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Gameplay;

public class PreviewLyricCommandGenerator : HitObjectCommandGenerator<PreviewStageInfo, Lyric>
{
    public PreviewLyricCommandGenerator()
        : base(null!)
    {
    }

    protected override double GeneratePreemptTime(Lyric hitObject)
    {
        return 0;
    }

    protected override IEnumerable<IStageCommand> GenerateInitialCommands(Lyric hitObject)
    {
        yield return new StageAnchorCommand(Easing.None, 0, 0, Anchor.Centre, Anchor.Centre);
        yield return new StageOriginCommand(Easing.None, 0, 0, Anchor.Centre, Anchor.Centre);
        yield return new StageScaleCommand(Easing.None, 0, 0, 2, 2);
        yield return new StageAlphaCommand(Easing.None, 0, 100, 0, 1);
    }

    protected override IEnumerable<IStageCommand> GenerateStartTimeStateCommands(Lyric hitObject)
    {
        // there's no transformer in here.
        yield break;
    }

    protected override IEnumerable<IStageCommand> GenerateHitStateCommands(Lyric hitObject, ArmedState state)
    {
        yield return new StageAlphaCommand(Easing.None, 0, 100, 1, 0);
    }
}
