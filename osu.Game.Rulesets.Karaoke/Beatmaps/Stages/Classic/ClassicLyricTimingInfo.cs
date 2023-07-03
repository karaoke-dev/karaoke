// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

public class ClassicLyricTimingInfo
{
    [JsonIgnore]
    public IBindable<int> TimingVersion => timingVersion;

    private readonly Bindable<int> timingVersion = new();

    // todo: should be readonly.
    public BindableList<ClassicLyricTimingPoint> Timings = new();

    [JsonIgnore]
    public List<ClassicLyricTimingPoint> SortedTimings { get; private set; } = new();

    [JsonIgnore]
    public IBindable<int> MappingVersion => mappingVersion;

    private readonly Bindable<int> mappingVersion = new();

    // todo: should be private.
    public BindableDictionary<ElementId, int[]> Mappings = new();

    public ClassicLyricTimingInfo()
    {
        Timings.CollectionChanged += (_, args) =>
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Debug.Assert(args.NewItems != null);

                    foreach (var c in args.NewItems.Cast<ClassicLyricTimingPoint>())
                        c.TimeBindable.ValueChanged += timeValueChanged;
                    break;

                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Remove:
                    Debug.Assert(args.OldItems != null);

                    foreach (var c in args.OldItems.Cast<ClassicLyricTimingPoint>())
                        c.TimeBindable.ValueChanged -= timeValueChanged;
                    break;
            }

            onTimingChanged();

            void timeValueChanged(ValueChangedEvent<double> e) => onTimingChanged();
        };

        void onTimingChanged()
        {
            SortedTimings = Timings.OrderBy(x => x.Time).ToList();
            timingVersion.Value++;
        }

        Mappings.CollectionChanged += (_, args) =>
        {
            switch (args.Action)
            {
                case NotifyDictionaryChangedAction.Add:
                case NotifyDictionaryChangedAction.Replace:
                case NotifyDictionaryChangedAction.Remove:
                    onMappingChanged();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        };

        void onMappingChanged() => mappingVersion.Value++;
    }

    #region Edit

    public ClassicLyricTimingPoint AddTimingPoint(Action<ClassicLyricTimingPoint>? action = null)
    {
        int id = getNewTimingPointId();
        var timingPoint = new ClassicLyricTimingPoint(id);

        action?.Invoke(timingPoint);
        Timings.Add(timingPoint);

        return timingPoint;

        int getNewTimingPointId()
        {
            if (Timings.Count == 0)
                return 1;

            return Timings.Max(x => x.ID) + 1;
        }
    }

    public void RemoveTimingPoint(ClassicLyricTimingPoint point)
    {
        ClearTimingPointFromMapping(point);

        Timings.Remove(point);
    }

    public void AddToMapping(ClassicLyricTimingPoint point, Lyric lyric)
    {
        var key = lyric.ID;
        int value = point.ID;

        if (!Timings.Contains(point))
            throw new InvalidOperationException($"{nameof(point)} does ont in the {nameof(point)}.");

        if (Mappings.TryGetValue(key, out int[]? timingIds))
        {
            Mappings[key] = timingIds.Concat(new[] { value }).ToArray();
        }
        else
        {
            Mappings.Add(key, new[] { value });
        }
    }

    public void RemoveFromMapping(ClassicLyricTimingPoint point, Lyric lyric)
    {
        var key = lyric.ID;
        int value = point.ID;

        if (!Timings.Contains(point))
            throw new InvalidOperationException($"{nameof(point)} does ont in the {nameof(point)}.");

        if (!Mappings.TryGetValue(key, out int[]? values))
            return;

        if (values.All(x => x == point.ID))
        {
            ClearLyricFromMapping(lyric);
        }
        else
        {
            Mappings[key] = values.Where(x => x != value).ToArray();
        }
    }

    public void ClearTimingPointFromMapping(ClassicLyricTimingPoint point)
    {
        int value = point.ID;

        foreach ((var key, int[]? values) in Mappings)
        {
            if (values.All(x => x == point.ID))
            {
                Mappings.Remove(key);
            }
            else
            {
                Mappings[key] = values.Where(x => x != value).ToArray();
            }
        }
    }

    public void ClearLyricFromMapping(Lyric lyric)
    {
        Mappings.Remove(lyric.ID);
    }

    #endregion

    #region Query

    public int? GetTimingPointOrder(ClassicLyricTimingPoint point)
    {
        int index = SortedTimings.IndexOf(point);
        return index == -1 ? null : index + 1;
    }

    public IEnumerable<ClassicLyricTimingPoint> GetLyricTimingPoints(Lyric lyric)
    {
        if (!Mappings.TryGetValue(lyric.ID, out int[]? ids))
            return Array.Empty<ClassicLyricTimingPoint>();

        return SortedTimings.Where(x => ids.Contains(x.ID));
    }

    public Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
    {
        // already sorted.
        double[] matchedTimings = GetLyricTimingPoints(lyric).Select(x => x.Time).ToArray();

        double? matchedStartTime = matchedTimings.Any() ? matchedTimings.Min() : default(double?);
        double? matchedEndTime = matchedTimings.Any() ? matchedTimings.Max() : default(double?);

        return new Tuple<double?, double?>(matchedStartTime, matchedEndTime);
    }

    public double? GetStartTime()
    {
        return Timings.Any() ? Timings.Min(x => x.Time) : default(double?);
    }

    public double? GetEndTime()
    {
        return Timings.Any() ? Timings.Max(x => x.Time) : default(double?);
    }

    public IEnumerable<ElementId> GetMatchedLyricIds(ClassicLyricTimingPoint point)
    {
        return Mappings.Where(x => x.Value.Contains(point.ID)).Select(x => x.Key);
    }

    #endregion
}
