// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers
{
    public class SingersChangeHandler : BeatmapChangeHandler<Singer>, ISingersChangeHandler
    {
        public BindableList<Singer> Singers { get; } = new();

        [BackgroundDependencyLoader]
        private void load()
        {
            Singers.AddRange(Beatmap.Singers);

            // should write-back if singer changed.
            Singers.BindCollectionChanged((_, _) =>
            {
                Beatmap.Singers = Singers.ToList();
            });
        }

        public void ChangeOrder(Singer singer, int newIndex)
        {
            PerformObjectChanged(singer, s =>
            {
                int oldOrder = s.Order;
                int newOrder = newIndex + 1; // order is start from 1
                OrderUtils.ChangeOrder(Singers.ToArray(), oldOrder, newOrder, (switchSinger, oldOrder, newOrder) =>
                {
                    // todo : not really sure should call update?
                });
            });
        }

        public override void Add(Singer item)
        {
            PerformObjectChanged(item, singer =>
            {
                singer.Order = OrderUtils.GetMaxOrderNumber(Singers.ToArray()) + 1;
                Singers.Add(singer);
            });
        }

        public override void Remove(Singer item)
        {
            PerformObjectChanged(item, singer =>
            {
                // Shifting order that order is larger than current singer
                OrderUtils.ShiftingOrder(Singers.Where(x => x.Order > singer.Order), -1);
                Singers.Remove(singer);

                // should clear removed singer ids in singer editor.
                var lyrics = Beatmap.HitObjects.OfType<Lyric>();
                lyrics.ForEach(x =>
                {
                    LyricUtils.RemoveSinger(x, singer);
                });
            });
        }
    }
}
