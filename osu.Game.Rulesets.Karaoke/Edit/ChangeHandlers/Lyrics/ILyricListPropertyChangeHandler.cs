// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public interface ILyricListPropertyChangeHandler<in TItem>
    {
        void Add(TItem item);

        void AddRange(IEnumerable<TItem> items);

        void Remove(TItem item);

        void RemoveRange(IEnumerable<TItem> items);
    }
}
