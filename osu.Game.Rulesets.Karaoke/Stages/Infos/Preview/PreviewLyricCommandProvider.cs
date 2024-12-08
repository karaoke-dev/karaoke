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

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;

public class PreviewLyricCommandProvider : HitObjectCommandProvider<PreviewStageInfo, Lyric>
{
    public PreviewLyricCommandProvider(PreviewStageInfo stageInfo)
        : base(stageInfo)
    {
    }

    protected override double GeneratePreemptTime(Lyric hitObject)
    {
        return StageInfo.StageDefinition.FadingTime;
    }

    protected override Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
    {
        var element = StageInfo.GetStageElements(lyric).OfType<PreviewLyricLayout>().Single();
        return new Tuple<double?, double?>(element.StartTime, element.EndTime);
    }

    protected override IEnumerable<IStageCommand> GetInitialCommands(Lyric hitObject)
    {
        var elements = StageInfo.GetStageElements(hitObject);
        return elements.Select(e => e switch
        {
            PreviewLyricLayout previewLyricLayout => updateInitialTransforms(previewLyricLayout),
            PreviewStyle style => updateInitialTransforms(style),
            _ => throw new NotSupportedException(),
        }).SelectMany(x => x);
    }

    private IEnumerable<IStageCommand> updateInitialTransforms(PreviewLyricLayout layout)
    {
        var definition = StageInfo.StageDefinition;

        double duration = getFadeInDuration(definition, layout);
        float preemptPosition = getPreemptPosition(definition, layout);
        float targetPosition = getTargetPosition(definition, layout);
        float preemptAlpha = getPreemptAlpha(definition, layout);
        float targetAlpha = getTargetAlpha(definition, layout);

        yield return new StageYCommand(definition.MovingInEasing, 0, duration, preemptPosition, targetPosition);
        yield return new StageAlphaCommand(definition.FadeInEasing, 0, duration, preemptAlpha, targetAlpha);
        yield break;

        static double getFadeInDuration(PreviewStageDefinition definition, PreviewLyricLayout layout)
        {
            return isLastLyricInView(layout) ? definition.FadingTime : 0;
        }

        static float getPreemptPosition(PreviewStageDefinition definition, PreviewLyricLayout layout)
        {
            float position = getTargetPosition(definition, layout);
            return isLastLyricInView(layout) ? position + definition.FadingOffsetPosition : position;
        }

        static float getTargetPosition(PreviewStageDefinition definition, PreviewLyricLayout layout)
        {
            int line = isLastLyricInView(layout) ? definition.NumberOfLyrics - 1 : layout.Timings.Count;
            return definition.LyricHeight * line;
        }

        static float getPreemptAlpha(PreviewStageDefinition definition, PreviewLyricLayout layout)
        {
            if (isFirstLyricInView(layout))
                return 1;

            if (isLastLyricInView(layout))
                return 0;

            return definition.InactiveAlpha;
        }

        static float getTargetAlpha(PreviewStageDefinition definition, PreviewLyricLayout layout)
        {
            if (isFirstLyricInView(layout))
                return 1;

            return definition.InactiveAlpha;
        }

        static bool isFirstLyricInView(PreviewLyricLayout layout) => !layout.Timings.Any();

        static bool isLastLyricInView(PreviewLyricLayout layout) => layout.StartTime != 0;
    }

    private IEnumerable<IStageCommand> updateInitialTransforms(PreviewStyle style)
    {
        if (style.LyricStyle != null)
        {
            yield return new LyricStyleCommand(Easing.None, 0, 0, style.LyricStyle, style.LyricStyle);
        }
    }

    protected override IEnumerable<IStageCommand> GetStartTimeStateCommands(Lyric hitObject)
    {
        var elements = StageInfo.GetStageElements(hitObject);
        return elements.Select(e => e switch
        {
            PreviewLyricLayout previewLyricLayout => updateStartTimeStateTransforms(previewLyricLayout),
            PreviewStyle => Array.Empty<IStageCommand>(), // todo: implement.
            _ => throw new NotSupportedException(),
        }).SelectMany(x => x);
    }

    private IEnumerable<IStageCommand> updateStartTimeStateTransforms(PreviewLyricLayout layout)
    {
        var definition = StageInfo.StageDefinition;
        double startTime = layout.StartTime;

        foreach ((int line, double time) in layout.Timings)
        {
            double relativeTime = time - startTime;

            // move the lyric to the target position.
            float previousPosition = definition.LyricHeight * (line + 1);
            float position = definition.LyricHeight * line;
            yield return new StageYCommand(definition.LineMovingEasing, relativeTime, relativeTime + definition.LineMovingTime, previousPosition, position);

            if (line != 0)
                continue;

            // change the alpha if lyric move to the first line.
            double fadingTime = Math.Clamp(definition.ActiveTime, 0, definition.LineMovingTime);
            yield return new StageAlphaCommand(definition.ActiveEasing, relativeTime, relativeTime + fadingTime, definition.InactiveAlpha, 1);
        }
    }

    protected override IEnumerable<IStageCommand> GetHitStateCommands(Lyric hitObject, ArmedState state)
    {
        var elements = StageInfo.GetStageElements(hitObject);
        return elements.Select(e => e switch
        {
            PreviewLyricLayout layout => updateHitStateTransforms(state, layout),
            PreviewStyle => Array.Empty<IStageCommand>(), // todo: implement.
            _ => throw new NotSupportedException(),
        }).SelectMany(x => x);
    }

    private IEnumerable<IStageCommand> updateHitStateTransforms(ArmedState state, PreviewLyricLayout layout)
    {
        var definition = StageInfo.StageDefinition;
        float targetPosition = -definition.FadingOffsetPosition;

        yield return new StageAlphaCommand(definition.FadeOutEasing, 0, definition.FadingTime, 1, 0);
        yield return new StageYCommand(definition.MoveOutEasing, 0, definition.FadingTime, 0, targetPosition);
    }
}
