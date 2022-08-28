// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricSingerChangeHandler : LyricPropertyChangeHandler, ILyricSingerChangeHandler
    {
        public void Add(Singer singer)
        {
            PerformOnSelection(lyric =>
            {
                lyric.Singers.Add(singer.ID);
            });
        }

        public void AddRange(IEnumerable<Singer> singers)
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

        public void Remove(Singer singer)
        {
            PerformOnSelection(lyric =>
            {
                lyric.Singers.Remove(singer.ID);
            });
        }

        public void RemoveRange(IEnumerable<Singer> singers)
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

        protected override bool AllowToEditIfHasReferenceLyric(IReferenceLyricPropertyConfig? config)
        {
            if (config is SyncLyricConfig syncLyricConfig && !syncLyricConfig.SyncSingerProperty)
                return true;

            return base.AllowToEditIfHasReferenceLyric(config);
        }
    }
}
