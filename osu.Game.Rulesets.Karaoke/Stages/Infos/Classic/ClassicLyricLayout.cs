// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

public class ClassicLyricLayout : StageElement
{
    [JsonIgnore]
    public readonly Bindable<ClassicLyricLayoutAlignment> AlignmentBindable = new(ClassicLyricLayoutAlignment.Center);

    /// <summary>
    /// <see cref="Lyric"/>'s alignment.
    /// </summary>
    public ClassicLyricLayoutAlignment Alignment
    {
        get => AlignmentBindable.Value;
        set => AlignmentBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<float> HorizontalMarginBindable = new();

    /// <summary>
    /// <see cref="Lyric"/>'s horizontal margin.
    /// </summary>
    public float HorizontalMargin
    {
        get => HorizontalMarginBindable.Value;
        set => HorizontalMarginBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<int> LineBindable = new();

    /// <summary>
    /// <see cref="Lyric"/>'s line number.<br/>
    /// <see cref="Lyric"/> will at bottom if <see cref="Line"/> is zero.
    /// </summary>
    public int Line
    {
        get => LineBindable.Value;
        set => LineBindable.Value = value;
    }
}
