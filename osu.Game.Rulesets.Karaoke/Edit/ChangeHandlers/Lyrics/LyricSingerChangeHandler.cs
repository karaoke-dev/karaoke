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
                lyric.SingerIds.Add(singer.ID);

                TriggerHitObjectUpdate(lyric);
            });
        }

        public void AddRange(IEnumerable<ISinger> singers)
        {
            PerformOnSelection(lyric =>
            {
                // should convert to array because enumerable might change while deleting.
                foreach (var singer in singers.ToArray())
                {
                    lyric.SingerIds.Add(singer.ID);
                }

                TriggerHitObjectUpdate(lyric);
            });
        }

        public void Remove(ISinger singer)
        {
            PerformOnSelection(lyric =>
            {
                lyric.SingerIds.Remove(singer.ID);

                TriggerHitObjectUpdate(lyric);
            });
        }

        public void RemoveRange(IEnumerable<ISinger> singers)
        {
            PerformOnSelection(lyric =>
            {
                // should convert to array because enumerable might change while deleting.
                foreach (var singer in singers.ToArray())
                {
                    lyric.SingerIds.Remove(singer.ID);
                }

                TriggerHitObjectUpdate(lyric);
            });
        }

        public void Clear()
        {
            PerformOnSelection(lyric =>
            {
                lyric.SingerIds.Clear();

                TriggerHitObjectUpdate(lyric);
            });
        }

        protected override bool IsWritePropertyLocked(Lyric lyric)
            => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.SingerIds));
    }
}
