// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Commands.Lyrics;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

public class ClassicLyricCommandProvider : HitObjectCommandProvider<ClassicStageInfo, Lyric>
{
    public ClassicLyricCommandProvider(ClassicStageInfo stageInfo)
        : base(stageInfo)
    {
    }

    protected override double GeneratePreemptTime(Lyric hitObject)
    {
        return StageInfo.StageDefinition.FadeInTime;
    }

    protected override Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
    {
        (double? startTime, double? endTime) = StageInfo.LyricTimingInfo.GetStartAndEndTime(lyric);
        return new Tuple<double?, double?>(startTime + getStartTimeOffset(StageInfo, startTime), endTime + getEndTimeOffset(StageInfo, endTime));

        static double? getStartTimeOffset(ClassicStageInfo stageInfo, double? lyricStartTime)
        {
            if (lyricStartTime == null)
                return null;

            bool isFirstAppearLyric = lyricStartTime.Value == stageInfo.LyricTimingInfo.GetStartTime();
            var stageDefinition = stageInfo.StageDefinition;

            if (isFirstAppearLyric)
            {
                return stageDefinition.FirstLyricStartTimeOffset + stageDefinition.FadeOutTime;
            }

            // should add the previous lyric's end time offset.
            return stageDefinition.LyricEndTimeOffset + stageDefinition.FadeOutTime + stageDefinition.FadeInTime;
        }

        static double? getEndTimeOffset(ClassicStageInfo stageInfo, double? lyricEndTime)
        {
            if (lyricEndTime == null)
                return null;

            bool isLastDisappearLyric = lyricEndTime.Value == stageInfo.LyricTimingInfo.GetEndTime();
            var stageDefinition = stageInfo.StageDefinition;

            return isLastDisappearLyric ? stageDefinition.LastLyricEndTimeOffset : stageDefinition.LyricEndTimeOffset;
        }
    }

    protected override IEnumerable<IStageCommand> GetInitialCommands(Lyric hitObject)
    {
        var elements = StageInfo.GetStageElements(hitObject);
        return elements.Select(e => e switch
        {
            ClassicLyricLayout layout => updateInitialTransforms(layout),
            ClassicStyle style => updateInitialTransforms(style),
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

    private IEnumerable<IStageCommand> updateInitialTransforms(ClassicStyle style)
    {
        if (style.LyricStyle != null)
        {
            yield return new LyricStyleCommand(Easing.None, 0, 0, style.LyricStyle, style.LyricStyle);
        }
    }

    protected override IEnumerable<IStageCommand> GetStartTimeStateCommands(Lyric hitObject)
    {
        // there's no transformer in here.
        yield break;
    }

    protected override IEnumerable<IStageCommand> GetHitStateCommands(Lyric hitObject, ArmedState state)
    {
        var definition = StageInfo.StageDefinition;
        yield return new StageAlphaCommand(definition.FadeOutEasing, 0, definition.FadeOutTime, 1, 0);
    }
}
