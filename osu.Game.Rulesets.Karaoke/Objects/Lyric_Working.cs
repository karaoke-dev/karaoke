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
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Objects;

/// <summary>
/// Placing the properties that set by <see cref="KaraokeBeatmapProcessor"/> or being calculated.
/// Those properties will not be saved into the beatmap.
/// </summary>
public partial class Lyric : IHasWorkingProperty<LyricWorkingProperty>, IHasCommandGenerator
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
                    PageIndex = LyricTimingInfo != null ? getPageIndex(beatmap, LyricTimingInfo.StartTime) : null;
                    break;

                case LyricWorkingProperty.ReferenceLyric:
                    ReferenceLyric = findLyricById(beatmap, ReferenceLyricId);
                    break;

                case LyricWorkingProperty.CommandGenerator:
                    CommandGenerator = getCommandGenerator(beatmap, this);
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

        static int? getPageIndex(KaraokeBeatmap beatmap, double startTime)
            => beatmap.PageInfo.GetPageIndexAt(startTime);

        static Lyric? findLyricById(IBeatmap beatmap, ElementId? id) =>
            id == null ? null : beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == id);

        static IHitObjectCommandGenerator getCommandGenerator(KaraokeBeatmap beatmap, KaraokeHitObject lyric)
        {
            var stageInfo = beatmap.CurrentStageInfo;
            if (stageInfo == null)
                throw new InvalidCastException();

            return stageInfo.GetHitObjectCommandGenerator(lyric) ?? throw new InvalidOperationException();
        }
    }

    [JsonIgnore]
    public LyricTimingInfo? LyricTimingInfo { get; private set; }

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
    public readonly Bindable<IHitObjectCommandGenerator> EffectApplierBindable = new();

    /// <summary>
    /// Stage elements.
    /// Will save all the elements that related to the lyric.
    /// The element might include something like style or layout info.
    /// </summary>
    [JsonIgnore]
    public IHitObjectCommandGenerator CommandGenerator
    {
        get => EffectApplierBindable.Value;
        set
        {
            EffectApplierBindable.Value = value;

            updateStateByWorkingProperty(LyricWorkingProperty.CommandGenerator);
        }
    }
}

public class LyricTimingInfo : IEquatable<LyricTimingInfo>
{
    public LyricTimingInfo(double startTime, double endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public double StartTime { get; }

    public double EndTime { get; }

    public double Duration => EndTime - StartTime;

    public bool Equals(LyricTimingInfo? other)
    {
        if (ReferenceEquals(null, other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        return obj.GetType() == GetType() && Equals((LyricTimingInfo)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, EndTime);
    }
}
