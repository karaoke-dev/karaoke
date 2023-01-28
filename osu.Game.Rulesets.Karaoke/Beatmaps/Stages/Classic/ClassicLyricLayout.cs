// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

public class ClassicLyricLayout : IStageElement, IComparable<ClassicLyricLayout>
{
    public ClassicLyricLayout(int id)
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
    /// <see cref="Lyric"/>'s alignment.
    /// </summary>
    public ClassicLyricLayoutAlignment Alignment { get; set; } = ClassicLyricLayoutAlignment.Center;

    /// <summary>
    /// <see cref="Lyric"/>'s horizontal margin.
    /// </summary>
    public int HorizontalMargin { get; set; }

    /// <summary>
    /// <see cref="Lyric"/>'s line number.
    /// <see cref="Lyric"/> will at bottom if <see cref="Line"/> is zero.
    /// </summary>
    public int Line { get; set; }

    public int CompareTo(ClassicLyricLayout? other)
    {
        return ComparableUtils.CompareByProperty(this, other,
            x => x.Line,
            x => x.Alignment,
            x => x.HorizontalMargin,
            x => x.ID);
    }
}
