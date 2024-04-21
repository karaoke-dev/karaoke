// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects.Stages;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Objects;

/// <summary>
/// Placing the properties that set by <see cref="KaraokeBeatmapProcessor"/> or being calculated.
/// Those properties will not be saved into the beatmap.
/// </summary>
public partial class Lyric : IHasWorkingProperty<LyricWorkingProperty>, IHasEffectApplier
{
    [JsonIgnore]
    private readonly LyricWorkingPropertyValidator workingPropertyValidator;

    public bool InvalidateWorkingProperty(LyricWorkingProperty workingProperty)
        => workingPropertyValidator.Invalidate(workingProperty);

    private void updateStateByWorkingProperty(LyricWorkingProperty workingProperty)
        => workingPropertyValidator.UpdateStateByWorkingProperty(workingProperty);

    public LyricWorkingProperty[] GetAllInvalidWorkingProperties()
        => workingPropertyValidator.GetAllInvalidFlags();

    public void ValidateWorkingProperty(KaraokeBeatmap beatmap)
    {
        foreach (var flag in GetAllInvalidWorkingProperties())
        {
            switch (flag)
            {
                case LyricWorkingProperty.StartTime:
                    StartTime = getStartTime(beatmap, this);
                    break;

                case LyricWorkingProperty.Duration:
                    Duration = getDuration(beatmap, this);
                    break;

                case LyricWorkingProperty.Timing:
                    // start time and duration should be set by other condition.
                    break;

                case LyricWorkingProperty.Singers:
                    Singers = getSingers(beatmap, SingerIds);
                    break;

                case LyricWorkingProperty.Page:
                    PageIndex = getPageIndex(beatmap, LyricStartTime);
                    break;

                case LyricWorkingProperty.ReferenceLyric:
                    ReferenceLyric = findLyricById(beatmap, ReferenceLyricId);
                    break;

                case LyricWorkingProperty.EffectApplier:
                    EffectApplier = getStageEffectApplier(beatmap, this);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static double getStartTime(KaraokeBeatmap beatmap, KaraokeHitObject lyric)
        {
            var stageInfo = beatmap.CurrentStageInfo;
            if (stageInfo == null)
                throw new InvalidCastException();

            (double? startTime, double? _) = stageInfo.GetStartAndEndTime(lyric);
            return startTime ?? 0;
        }

        static double getDuration(KaraokeBeatmap beatmap, KaraokeHitObject lyric)
        {
            var stageInfo = beatmap.CurrentStageInfo;
            if (stageInfo == null)
                throw new InvalidCastException();

            (double? startTime, double? endTime) = stageInfo.GetStartAndEndTime(lyric);
            return endTime - startTime ?? 0;
        }

        static IDictionary<Singer, SingerState[]> getSingers(KaraokeBeatmap beatmap, IEnumerable<ElementId> singerIds)
            => beatmap.SingerInfo.GetSingerByIds(singerIds.ToArray());

        static int? getPageIndex(KaraokeBeatmap beatmap, double? startTime)
            => beatmap.PageInfo.GetPageIndexAt(startTime);

        static Lyric? findLyricById(IBeatmap beatmap, ElementId? id) =>
            id == null ? null : beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == id);

        static IStageEffectApplier getStageEffectApplier(KaraokeBeatmap beatmap, KaraokeHitObject lyric)
        {
            var stageInfo = beatmap.CurrentStageInfo;
            if (stageInfo == null)
                throw new InvalidCastException();

            return stageInfo.GetStageAppliers(lyric);
        }
    }

    [JsonIgnore]
    public double? LyricStartTime { get; private set; }

    [JsonIgnore]
    public double? LyricEndTime { get; private set; }

    [JsonIgnore]
    public double? LyricDuration => LyricEndTime - LyricStartTime;

    /// <summary>
    /// Lyric's start time is created from <see cref="StageInfo"/> and should not be saved.
    /// </summary>
    [JsonIgnore]
    public override double StartTime
    {
        get => base.StartTime;
        set
        {
            base.StartTime = value;
            updateStateByWorkingProperty(LyricWorkingProperty.StartTime);
        }
    }

    [JsonIgnore]
    public readonly Bindable<double> DurationBindable = new BindableDouble();

    /// <summary>
    /// Lyric's duration is created from <see cref="StageInfo"/> and should not be saved.
    /// </summary>
    [JsonIgnore]
    public double Duration
    {
        get => DurationBindable.Value;
        set
        {
            DurationBindable.Value = value;
            updateStateByWorkingProperty(LyricWorkingProperty.Duration);
        }
    }

    /// <summary>
    /// The time at which the HitObject end.
    /// </summary>
    [JsonIgnore]
    public double EndTime => StartTime + Duration;

    [JsonIgnore]
    public readonly BindableDictionary<Singer, SingerState[]> SingersBindable = new();

    /// <summary>
    /// Singers
    /// </summary>
    [JsonIgnore]
    public IDictionary<Singer, SingerState[]> Singers
    {
        get => SingersBindable;
        set
        {
            SingersBindable.Clear();
            SingersBindable.AddRange(value);
            updateStateByWorkingProperty(LyricWorkingProperty.Singers);
        }
    }

    [JsonIgnore]
    public readonly Bindable<int?> PageIndexBindable = new();

    /// <summary>
    /// Order
    /// </summary>
    [JsonIgnore]
    public int? PageIndex
    {
        get => PageIndexBindable.Value;
        set
        {
            PageIndexBindable.Value = value;
            updateStateByWorkingProperty(LyricWorkingProperty.Page);
        }
    }

    [JsonIgnore]
    public readonly Bindable<Lyric?> ReferenceLyricBindable = new();

    /// <summary>
    /// Reference lyric.
    /// Link the same or similar lyric for reference or sync the properties.
    /// </summary>
    [JsonIgnore]
    public Lyric? ReferenceLyric
    {
        get => ReferenceLyricBindable.Value;
        set
        {
            ReferenceLyricBindable.Value = value;
            updateStateByWorkingProperty(LyricWorkingProperty.ReferenceLyric);
        }
    }

    [JsonIgnore]
    public readonly Bindable<IStageEffectApplier> EffectApplierBindable = new();

    /// <summary>
    /// Stage elements.
    /// Will save all the elements that related to the lyric.
    /// The element might include something like style or layout info.
    /// </summary>
    [JsonIgnore]
    public IStageEffectApplier EffectApplier
    {
        get => EffectApplierBindable.Value;
        set
        {
            EffectApplierBindable.Value = value;

            updateStateByWorkingProperty(LyricWorkingProperty.EffectApplier);
        }
    }
}
