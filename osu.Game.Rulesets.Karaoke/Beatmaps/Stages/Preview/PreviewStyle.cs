// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;

public class PreviewStyle : StageElement, IComparable<PreviewStyle>
{
    /// <summary>
    /// <see cref="Lyric"/>'s skin lookup index.
    /// </summary>
    public int? LyricStyleIndex { get; set; }

    /// <summary>
    /// <see cref="Note"/>'s skin lookup index.
    /// </summary>
    public int? NoteStyleIndex { get; set; }

    public int CompareTo(PreviewStyle? other)
    {
        return ComparableUtils.CompareByProperty(this, other,
            x => x.Name,
            x => x.ID);
    }
}
