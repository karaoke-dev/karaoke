// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public interface ILyricTimeTagsChangeHandler : ILyricListPropertyChangeHandler<TimeTag>
    {
        void SetTimeTagTime(TimeTag timeTag, double time);

        void ShiftingTimeTagTime(IEnumerable<TimeTag> timeTags, double offset);

        void ClearTimeTagTime(TimeTag timeTag);

        void ClearAllTimeTagTime();

        void AddByPosition(TextIndex index);

        void RemoveByPosition(TextIndex index);

        TimeTag Shifting(TimeTag timeTag, ShiftingDirection direction, ShiftingType type);
    }

    public enum ShiftingDirection
    {
        Left,

        Right
    }

    public enum ShiftingType
    {
        State,

        Index,
    }
}
