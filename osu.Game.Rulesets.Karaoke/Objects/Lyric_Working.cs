// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects.Workings;

namespace osu.Game.Rulesets.Karaoke.Objects;

/// <summary>
/// Placing the properties that set by <see cref="KaraokeBeatmapProcessor"/> or being calculated.
/// Those properties will not be saved into the beatmap.
/// </summary>
public partial class Lyric
{
    [JsonIgnore]
    public HitObjectWorkingPropertyValidator<LyricWorkingProperty> Validator { get; } = new();

    [JsonIgnore]
    public double LyricStartTime { get; private set; }

    [JsonIgnore]
    public double LyricEndTime { get; private set; }

    [JsonIgnore]
    public double LyricDuration => LyricEndTime - LyricStartTime;

    /// <summary>
    /// Lyric's start time is created from <see cref="KaraokeBeatmapProcessor"/> and should not be saved.
    /// </summary>
    [JsonIgnore]
    public override double StartTime
    {
        get => base.StartTime;
        set => base.StartTime = value;
    }

    /// <summary>
    /// Lyric's duration is created from <see cref="KaraokeBeatmapProcessor"/> and should not be saved.
    /// </summary>
    [JsonIgnore]
    public double Duration { get; set; }

    /// <summary>
    /// The time at which the HitObject end.
    /// </summary>
    [JsonIgnore]
    public double EndTime => StartTime + Duration;

    [JsonIgnore]
    public readonly Bindable<int?> PageIndexBindable = new();

    /// <summary>
    /// Order
    /// </summary>
    [JsonIgnore]
    public int? PageIndex
    {
        get => PageIndexBindable.Value;
        set => PageIndexBindable.Value = value;
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

            if (value?.ID != ReferenceLyricId)
            {
                throw new InvalidWorkingPropertyAssignException();
            }
        }
    }
}
