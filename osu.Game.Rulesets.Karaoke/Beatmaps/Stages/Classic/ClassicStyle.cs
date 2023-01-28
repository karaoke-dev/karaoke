// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

public class ClassicStyle : IStageElement, IComparable<ClassicStyle>
{
    public ClassicStyle(int id)
    {
        ID = id;
    }

    /// <summary>
    /// Index of the element.
    /// </summary>
    public int ID { get; protected set; }

    /// <summary>
    /// Name of the element.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// <see cref="Lyric"/>'s skin lookup index.
    /// </summary>
    public int? LyricStyleIndex { get; set; }

    /// <summary>
    /// <see cref="Note"/>'s skin lookup index.
    /// </summary>
    public int? NoteStyleIndex { get; set; }

    public int CompareTo(ClassicStyle? other)
    {
        return ComparableUtils.CompareByProperty(this, other,
            x => x.Name,
            x => x.ID);
    }
}
