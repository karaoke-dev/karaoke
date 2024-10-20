// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

public class ClassicLyricCommandGenerator : HitObjectCommandGenerator<ClassicStageInfo, Lyric>
{
    public ClassicLyricCommandGenerator(ClassicStageInfo stageInfo)
        : base(stageInfo)
    {
    }

    protected override double GeneratePreemptTime(Lyric hitObject)
    {
        return StageInfo.StageDefinition.FadeInTime;
    }

    protected override IEnumerable<IStageCommand> GenerateInitialCommands(Lyric hitObject)
    {
        var elements = StageInfo.GetStageElements(hitObject);
        return elements.Select(e => e switch
        {
            ClassicLyricLayout previewLyricLayout => updateInitialTransforms(previewLyricLayout),
            ClassicStyle => Array.Empty<IStageCommand>(), // todo: implement.
            _ => throw new NotSupportedException(),
        }).SelectMany(x => x);
    }

    private IEnumerable<IStageCommand> updateInitialTransforms(ClassicLyricLayout layout)
    {
        var definition = StageInfo.StageDefinition;
        yield return new StageAnchorCommand(Easing.None, 0, 0, getLyricAnchor(layout.Alignment), getLyricAnchor(layout.Alignment));
        yield return new StageOriginCommand(Easing.None, 0, 0, getLyricAnchor(layout.Alignment), getLyricAnchor(layout.Alignment));
        yield return new StagePaddingCommand(Easing.None, 0, 0, getPosition(definition, layout), getPosition(definition, layout));
        yield return new StageScaleCommand(Easing.None, 0, 0, definition.LyricScale, definition.LyricScale);
        yield return new StageAlphaCommand(definition.FadeInEasing, 0, definition.FadeInTime, 1, 1);
        yield break;

        static Anchor getLyricAnchor(ClassicLyricLayoutAlignment alignment) =>
            alignment switch
            {
                ClassicLyricLayoutAlignment.Left => Anchor.BottomLeft,
                ClassicLyricLayoutAlignment.Center => Anchor.BottomCentre,
                ClassicLyricLayoutAlignment.Right => Anchor.BottomRight,
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null),
            };

        static MarginPadding getPosition(ClassicStageDefinition definition, ClassicLyricLayout layout)
        {
            float paddingX = definition.BorderWidth + layout.HorizontalMargin;
            float paddingY = definition.BorderHeight + definition.LineHeight * layout.Line;

            var padding = new MarginPadding
            {
                Bottom = paddingY,
            };

            switch (layout.Alignment)
            {
                case ClassicLyricLayoutAlignment.Left:
                    padding.Left = paddingX;
                    return padding;

                case ClassicLyricLayoutAlignment.Center:
                    return padding;

                case ClassicLyricLayoutAlignment.Right:
                    padding.Right = paddingX;
                    return padding;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    protected override IEnumerable<IStageCommand> GenerateStartTimeStateCommands(Lyric hitObject)
    {
        // there's no transformer in here.
        yield break;
    }

    protected override IEnumerable<IStageCommand> GenerateHitStateCommands(Lyric hitObject, ArmedState state)
    {
        var definition = StageInfo.StageDefinition;
        yield return new StageAlphaCommand(definition.FadeOutEasing, 0, definition.FadeOutTime, 1, 0);
    }
}
