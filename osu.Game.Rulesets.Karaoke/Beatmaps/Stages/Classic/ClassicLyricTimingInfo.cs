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

    public BindableList<ClassicLyricTimingPoint> Timings = new();

    [JsonIgnore]
    public List<ClassicLyricTimingPoint> SortedTimings { get; private set; } = new();

    [JsonIgnore]
    public IBindable<int> MappingVersion => mappingVersion;

    private readonly Bindable<int> mappingVersion = new();

    // todo: should be private.
    public BindableDictionary<int, int[]> Mappings = new();

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

            onPageChanged();

            void timeValueChanged(ValueChangedEvent<double> e) => onPageChanged();
        };

        void onPageChanged()
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

    public Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
    {
        // matched time should be sorted.
        var matchedTimes = SortedTimings.Select(x => x.GetLyricTime(lyric))
                                        .Where(x => x != null)
                                        .OfType<double>()
                                        .ToList();

        double? matchedStartTime = matchedTimes.Any() ? matchedTimes.Min() : default(double?);
        double? matchedEndTime = matchedTimes.Any() ? matchedTimes.Max() : default(double?);

        return new Tuple<double?, double?>(matchedStartTime, matchedEndTime);
    }
}
