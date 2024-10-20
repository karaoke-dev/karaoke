// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

public class ClassicStyle : StageElement, IComparable<ClassicStyle>
{
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
