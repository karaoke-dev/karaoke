// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public partial class LyricSingerChangeHandler : LyricPropertyChangeHandler, ILyricSingerChangeHandler
    {
        public void Add(ISinger singer)
        {
            PerformOnSelection(lyric =>
            {
                lyric.Singers.Add(singer.ID);
            });
        }

        public void AddRange(IEnumerable<ISinger> singers)
        {
            PerformOnSelection(lyric =>
            {
                // should convert to array because enumerable might change while deleting.
                foreach (var singer in singers.ToArray())
                {
                    lyric.Singers.Add(singer.ID);
                }
            });
        }

        public void Remove(ISinger singer)
        {
            PerformOnSelection(lyric =>
            {
                lyric.Singers.Remove(singer.ID);
            });
        }

        public void RemoveRange(IEnumerable<ISinger> singers)
        {
            PerformOnSelection(lyric =>
            {
                // should convert to array because enumerable might change while deleting.
                foreach (var singer in singers.ToArray())
                {
                    lyric.Singers.Remove(singer.ID);
                }
            });
        }

        public void Clear()
        {
            PerformOnSelection(lyric =>
            {
                lyric.Singers.Clear();
            });
        }

        protected override bool IsWritePropertyLocked(Lyric lyric)
            => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.Singers));
    }
}
