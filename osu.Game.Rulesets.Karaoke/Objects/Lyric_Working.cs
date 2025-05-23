﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
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

namespace osu.Game.Rulesets.Karaoke.Objects;

/// <summary>
/// Placing the properties that set by <see cref="KaraokeBeatmapProcessor"/> or being calculated.
/// Those properties will not be saved into the beatmap.
/// </summary>
public partial class Lyric : IHasWorkingProperty<LyricWorkingProperty>
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
                case LyricWorkingProperty.Singers:
                    Singers = getSingers(beatmap, SingerIds);
                    break;

                case LyricWorkingProperty.Page:
                    PageIndex = getPageIndex(beatmap, StartTime);
                    break;

                case LyricWorkingProperty.ReferenceLyric:
                    ReferenceLyric = findLyricById(beatmap, ReferenceLyricId);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static IDictionary<Singer, SingerState[]> getSingers(KaraokeBeatmap beatmap, IEnumerable<ElementId> singerIds)
            => beatmap.SingerInfo.GetSingerByIds(singerIds.ToArray());

        static int? getPageIndex(KaraokeBeatmap beatmap, double startTime)
            => beatmap.PageInfo.GetPageIndexAt(startTime);

        static Lyric? findLyricById(IBeatmap beatmap, ElementId? id) =>
            id == null ? null : beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == id);
    }

    /// <summary>
    /// Min value of <see cref="TimeTags"/> time.
    /// </summary>
    [JsonIgnore]
    public override double StartTime
    {
        get => base.StartTime;
        set => base.StartTime = value;
    }

    [JsonIgnore]
    public readonly Bindable<double> DurationBindable = new BindableDouble();

    /// <summary>
    /// Duration of <see cref="TimeTags"/>
    /// </summary>
    [JsonIgnore]
    public double Duration
    {
        get => DurationBindable.Value;
        set => DurationBindable.Value = value;
    }

    /// <summary>
    /// Max value of <see cref="TimeTags"/> time.
    /// </summary>
    [JsonIgnore]
    public double EndTime => StartTime + Duration;

    public bool TimeValid { get; private set; }

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
}
