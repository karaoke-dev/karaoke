// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricSingerChangeHandler : HitObjectChangeHandler<Lyric>, ILyricSingerChangeHandler
    {
        public void Add(Singer singer)
        {
            PerformOnSelection(lyric =>
            {
                LyricUtils.AddSinger(lyric, singer);
            });
        }

        public void Remove(Singer singer)
        {
            PerformOnSelection(lyric =>
            {
                LyricUtils.RemoveSinger(lyric, singer);
            });
        }

        public void Clear()
        {
            PerformOnSelection(lyric =>
            {
                LyricUtils.ClearSinger(lyric);
            });
        }
    }
}
