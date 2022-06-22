// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricSingerChangeHandler : HitObjectChangeHandler<Lyric>, ILyricSingerChangeHandler
    {
        public void Add(Singer singer)
        {
            PerformOnSelection(lyric =>
            {
                lyric.Singers.Add(singer.ID);
            });
        }

        public void Remove(Singer singer)
        {
            PerformOnSelection(lyric =>
            {
                lyric.Singers.Remove(singer.ID);
            });
        }

        public void Clear()
        {
            PerformOnSelection(lyric =>
            {
                lyric.Singers.Clear();
            });
        }
    }
}
