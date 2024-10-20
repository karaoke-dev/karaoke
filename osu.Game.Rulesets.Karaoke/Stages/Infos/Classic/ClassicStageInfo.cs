// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Stages;
using osu.Game.Rulesets.Karaoke.UI.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

public class ClassicStageInfo : StageInfo
{
    #region Category

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s and <see cref="Note"/>'s style.
    /// </summary>
    public ClassicStyleCategory StyleCategory { get; set; } = new();

    /// <summary>
    /// The definition for the <see cref="Lyric"/>.<br/>
    /// Like the line height or font size.
    /// </summary>
    public ClassicStageDefinition StageDefinition { get; set; } = new();

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s layout.
    /// </summary>
    public ClassicLyricLayoutCategory LyricLayoutCategory { get; set; } = new();

    /// <summary>
    /// Timing info for saving the <see cref="Lyric"/>'s appear and disappear time.
    /// </summary>
    public ClassicLyricTimingInfo LyricTimingInfo { get; set; } = new();

    #endregion

    #region Stage element

    protected override IPlayfieldStageApplier CreatePlayfieldStageApplier()
    {
        return new PlayfieldClassicStageApplier(StageDefinition);
    }

    protected override IEnumerable<StageElement> GetLyricStageElements(Lyric lyric)
    {
        yield return StyleCategory.GetElementByItem(lyric);
        yield return LyricLayoutCategory.GetElementByItem(lyric);
    }

    protected override IEnumerable<StageElement> GetNoteStageElements(Note note)
    {
        // todo: should check the real-time mapping result.
        yield return StyleCategory.GetElementByItem(note.ReferenceLyric!);
    }

    protected override IHitObjectCommandGenerator GetLyricCommandGenerator()
        => new ClassicLyricCommandGenerator(this);

    protected override IHitObjectCommandGenerator? GetNoteCommandGenerator()
        => null;

    protected override Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
    {
        (double? startTime, double? endTime) = LyricTimingInfo.GetStartAndEndTime(lyric);
        return new Tuple<double?, double?>(startTime + getStartTimeOffset(startTime), endTime + getEndTimeOffset(endTime));
    }

    private double? getStartTimeOffset(double? lyricStartTime)
    {
        if (lyricStartTime == null)
            return null;

        bool isFirstAppearLyric = lyricStartTime.Value == LyricTimingInfo.GetStartTime();

        if (isFirstAppearLyric)
        {
            return StageDefinition.FirstLyricStartTimeOffset + StageDefinition.FadeOutTime;
        }

        // should add the previous lyric's end time offset.
        return StageDefinition.LyricEndTimeOffset + StageDefinition.FadeOutTime + StageDefinition.FadeInTime;
    }

    private double? getEndTimeOffset(double? lyricEndTime)
    {
        if (lyricEndTime == null)
            return null;

        bool isLastDisappearLyric = lyricEndTime.Value == LyricTimingInfo.GetEndTime();
        return isLastDisappearLyric ? StageDefinition.LastLyricEndTimeOffset : StageDefinition.LyricEndTimeOffset;
    }

    #endregion
}
