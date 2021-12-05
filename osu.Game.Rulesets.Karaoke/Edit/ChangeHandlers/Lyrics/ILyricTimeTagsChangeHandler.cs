// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public interface ILyricTimeTagsChangeHandler : IAutoGenerateChangeHandler
    {
        void SetTimeTagTime(TimeTag timeTag, double time);

        void ClearTimeTagTime(TimeTag timeTag);

        void Add(TimeTag timeTag);

        void Remove(TimeTag timeTag);

        void AddByPosition(TextIndex index);

        void RemoveByPosition(TextIndex index);
    }
}
