// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

public class ClassicLyricLayout : IStageElement, IComparable<ClassicLyricLayout>
{
    private readonly Bindable<int> orderVersion = new();

    public IBindable<int> OrderVersion => orderVersion;

    public ClassicLyricLayout(int id)
    {
        ID = id;
    }

    /// <summary>
    /// Index of the element.
    /// </summary>
    public int ID { get; protected set; }

    [JsonIgnore]
    public readonly Bindable<string> NameBindable = new();

    /// <summary>
    /// Name of the element.
    /// </summary>
    public string Name
    {
        get => NameBindable.Value;
        set => NameBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<ClassicLyricLayoutAlignment> AlignmentBindable = new(ClassicLyricLayoutAlignment.Center);

    /// <summary>
    /// <see cref="Lyric"/>'s alignment.
    /// </summary>
    public ClassicLyricLayoutAlignment Alignment
    {
        get => AlignmentBindable.Value;
        set
        {
            AlignmentBindable.Value = value;
            orderVersion.Value++;
        }
    }

    [JsonIgnore]
    public readonly Bindable<float> HorizontalMarginBindable = new();

    /// <summary>
    /// <see cref="Lyric"/>'s horizontal margin.
    /// </summary>
    public float HorizontalMargin
    {
        get => HorizontalMarginBindable.Value;
        set
        {
            HorizontalMarginBindable.Value = value;
            orderVersion.Value++;
        }
    }

    [JsonIgnore]
    public readonly Bindable<int> LineBindable = new();

    /// <summary>
    /// <see cref="Lyric"/>'s line number.
    /// <see cref="Lyric"/> will at bottom if <see cref="Line"/> is zero.
    /// </summary>
    public int Line
    {
        get => LineBindable.Value;
        set
        {
            LineBindable.Value = value;
            orderVersion.Value++;
        }
    }

    public int CompareTo(ClassicLyricLayout? other)
    {
        return ComparableUtils.CompareByProperty(this, other,
            x => x.Line,
            x => x.Alignment,
            x => x.HorizontalMargin,
            x => x.ID);
    }
}
