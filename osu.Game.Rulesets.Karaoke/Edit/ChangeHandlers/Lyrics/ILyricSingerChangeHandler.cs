// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public interface ILyricSingerChangeHandler
    {
        void AddRange(IEnumerable<Singer> singers);

        void RemoveRange(IEnumerable<Singer> singers);

        void Clear();
    }
}
