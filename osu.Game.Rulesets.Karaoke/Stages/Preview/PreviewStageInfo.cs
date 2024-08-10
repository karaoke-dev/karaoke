// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Stages;
using osu.Game.Rulesets.Karaoke.Objects.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Stages.Types;
using osu.Game.Rulesets.Karaoke.UI.Stages;
using osu.Game.Rulesets.Karaoke.UI.Stages.Preview;

namespace osu.Game.Rulesets.Karaoke.Stages.Preview;

public class PreviewStageInfo : StageInfo, IHasCalculatedProperty
{
    #region Category

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s and <see cref="Note"/>'s style.
    /// </summary>
    [JsonIgnore]
    private PreviewStyleCategory styleCategory { get; set; } = new();

    /// <summary>
    /// The definition for the <see cref="Lyric"/>.
    /// Like how many lyrics can in the playfield at the same time.
    /// </summary>
    public PreviewStageDefinition StageDefinition { get; set; } = new();

    /// <summary>
    /// Category to save the <see cref="Lyric"/>'s layout.
    /// </summary>
    [JsonIgnore]
    private PreviewLyricLayoutCategory layoutCategory { get; set; } = new();

    #endregion

    #region Validation

    private bool calculatedPropertyIsUpdated;

    /// <summary>
    /// Mark the stage info's calculated property as invalidate.
    /// </summary>
    /// <returns></returns>
    public void TriggerRecalculate()
    {
        calculatedPropertyIsUpdated = false;
    }

    /// <summary>
    /// Check if the stage info's calculated property is calculated and the value is the latest.
    /// </summary>
    /// <returns></returns>
    public bool IsUpdated() => calculatedPropertyIsUpdated;

    /// <summary>
    /// If the calculated property is not updated, then re-calculate the property inside the stage info in the <see cref="KaraokeBeatmapProcessor"/>
    /// </summary>
    /// <param name="beatmap"></param>
    public void ValidateCalculatedProperty(IBeatmap beatmap)
    {
        if (IsUpdated())
            return;

        var calculator = new PreviewStageTimingCalculator(beatmap, StageDefinition);

        // also, clear all mapping in the layout and re-create one.
        layoutCategory.ClearElements();

        // Note: only deal with those lyrics has time.
        var matchedLyrics = beatmap.HitObjects.OfType<Lyric>().Where(x => x.LyricTimingInfo != null).OrderBy(x => x.LyricTimingInfo!.StartTime).ToArray();

        foreach (var lyric in matchedLyrics)
        {
            var element = layoutCategory.AddElement(x =>
            {
                x.Name = $"Auto-generated layout with lyric {lyric.ID}";
                x.StartTime = calculator.CalculateStartTime(lyric);
                x.EndTime = calculator.CalculateEndTime(lyric);
                x.Timings = calculator.CalculateTimings(lyric);
            });
            layoutCategory.AddToMapping(element, lyric);

            // Need to invalidate the working property in the lyric to let the property re-fill in the beatmap processor.
            lyric.InvalidateWorkingProperty(LyricStageWorkingProperty.Timing);
            lyric.InvalidateWorkingProperty(LyricStageWorkingProperty.EffectApplier);
        }

        calculatedPropertyIsUpdated = true;
    }

    #endregion

    #region Stage element

    protected override IPlayfieldStageApplier CreatePlayfieldStageApplier()
    {
        return new PlayfieldPreviewStageApplier(StageDefinition);
    }

    protected override IEnumerable<StageElement> GetLyricStageElements(Lyric lyric)
    {
        yield return styleCategory.GetElementByItem(lyric);
        yield return layoutCategory.GetElementByItem(lyric);
    }

    protected override IEnumerable<StageElement> GetNoteStageElements(Note note)
    {
        // todo: should check the real-time mapping result.
        yield return styleCategory.GetElementByItem(note.ReferenceLyric!);
    }

    protected override IStageEffectApplier ConvertToLyricStageAppliers(IEnumerable<StageElement> elements)
    {
        return new LyricPreviewStageEffectApplier(elements, StageDefinition);
    }

    protected override IStageEffectApplier ConvertToNoteStageAppliers(IEnumerable<StageElement> elements)
    {
        return new NotePreviewStageEffectApplier(elements, StageDefinition);
    }

    protected override Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
    {
        var element = layoutCategory.GetElementByItem(lyric);
        return new Tuple<double?, double?>(element.StartTime, element.EndTime);
    }

    #endregion
}
