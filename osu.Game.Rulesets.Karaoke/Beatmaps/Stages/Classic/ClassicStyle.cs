// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

public class ClassicStyle : IStageElement, IComparable<ClassicStyle>
{
    private readonly Bindable<int> orderVersion = new();

    public IBindable<int> OrderVersion => orderVersion;

    public ClassicStyle(int id)
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
    public readonly Bindable<int?> LyricStyleIndexBindable = new();

    /// <summary>
    /// <see cref="Lyric"/>'s skin lookup index.
    /// </summary>
    public int? LyricStyleIndex
    {
        get => LyricStyleIndexBindable.Value;
        set => LyricStyleIndexBindable.Value = value;
    }

    [JsonIgnore]
    public readonly Bindable<int?> NoteStyleIndexBindable = new();

    /// <summary>
    /// <see cref="Note"/>'s skin lookup index.
    /// </summary>
    public int? NoteStyleIndex
    {
        get => NoteStyleIndexBindable.Value;
        set => NoteStyleIndexBindable.Value = value;
    }

    public int CompareTo(ClassicStyle? other)
    {
        return ComparableUtils.CompareByProperty(this, other,
            x => x.Name,
            x => x.ID);
    }
}
