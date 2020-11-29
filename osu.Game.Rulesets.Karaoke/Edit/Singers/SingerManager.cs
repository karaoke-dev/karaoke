// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerManager : Component
    {
        public readonly BindableFloat BindableZoom = new BindableFloat();

        public readonly BindableList<Singer> Singers = new BindableList<Singer>();

        [Resolved(CanBeNull = true)]
        private IEditorChangeHandler changeHandler { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (beatmap?.PlayableBeatmap is KaraokeBeatmap karaokeBeatmap)
            {
                Singers.AddRange(karaokeBeatmap.SingerMetadata.Singers);
            }
        }

        public IEnumerable<MenuItem> CreateSingerContextMenu(List<Lyric> lyrics)
        {
            return Singers.Select(singer => new OsuMenuItem(singer.Name, anySingerInLyric(singer) ? MenuItemType.Highlighted : MenuItemType.Standard, () =>
            {
                // if only one lyric
                if (allSingerInLyric(singer))
                {
                    lyrics.ForEach(lyric => RemoveSingerToLyric(singer, lyric));
                }
                else
                {
                    lyrics.ForEach(lyric => AddSingerToLyric(singer, lyric));
                }
            }));

            bool anySingerInLyric(Singer singer) => lyrics.Any(lyric => SingerInLyric(singer, lyric));

            bool allSingerInLyric(Singer singer) => lyrics.All(lyric => SingerInLyric(singer, lyric));
        }

        public void AddSingerToLyric(Singer singer, Lyric lyric) => AddSingersToLyrics(new List<Singer> { singer }, new List<Lyric> { lyric });

        public void AddSingersToLyrics(List<Singer> singers, List<Lyric> lyrics)
        {
            if (!(singers?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(singers)} cannot be numm or empty.");

            if (!(lyrics?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(lyrics)} cannot be numm or empty.");

            changeHandler?.BeginChange();

            foreach (var lyric in lyrics)
            {
                foreach (var singer in singers)
                {
                    addSingerToLyric(singer, lyric);
                }
            }

            changeHandler?.EndChange();

            void addSingerToLyric(Singer singer, Lyric lyric)
            {
                if (SingerInLyric(singer, lyric))
                    return;

                var existSingerList = lyric.Singers?.ToList() ?? new List<int>();
                existSingerList.Add(singer.ID);
                lyric.Singers = existSingerList.ToArray();
            }
        }

        public void RemoveSingerToLyric(Singer singer, Lyric lyric) => RemoveSingersToLyrics(new List<Singer> { singer }, new List<Lyric> { lyric });

        public void RemoveSingersToLyrics(List<Singer> singers, List<Lyric> lyrics)
        {
            if (!(singers?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(singers)} cannot be numm or empty.");

            if (!(lyrics?.Any() ?? false))
                throw new ArgumentNullException($"{nameof(lyrics)} cannot be numm or empty.");

            changeHandler?.BeginChange();

            foreach (var lyric in lyrics)
            {
                foreach (var singer in singers)
                {
                    removeSingerToLyric(singer, lyric);
                }
            }

            changeHandler?.EndChange();

            void removeSingerToLyric(Singer singer, Lyric lyric)
            {
                if (!SingerInLyric(singer, lyric))
                    return;

                lyric.Singers = lyric.Singers?.Where(x => x != singer.ID).ToArray();
            }
        }

        public bool SingerInLyric(Singer singer, Lyric lyric)
        {
            // lyric belongs to default singer if no singer specified by lyric.
            if (lyric.Singers == null || !lyric.Singers.Any())
                return singer.ID == 0;

            return (bool)lyric.Singers?.Contains(singer.ID);
        }
    }
}
