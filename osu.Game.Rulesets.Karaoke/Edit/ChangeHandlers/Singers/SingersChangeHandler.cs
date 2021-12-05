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
    public class SingersChangeHandler : BeatmapChangeHandler<Singer>
    {
        private readonly BindableList<Singer> singers = new();

        [BackgroundDependencyLoader]
        private void load()
        {
            singers.AddRange(Beatmap.Singers);

            // should write-back if singer changed.
            singers.BindCollectionChanged((_, _) =>
            {
                Beatmap.Singers = singers.ToArray();
            });
        }

        public void ChangeOrder(Singer singer, int newIndex)
        {
            PerformObjectChanged(singer, s =>
            {
                var oldOrder = s.Order;
                var newOrder = newIndex + 1; // order is start from 1
                OrderUtils.ChangeOrder(singers.ToArray(), oldOrder, newOrder, (switchSinger, oldOrder, newOrder) =>
                {
                    // todo : not really sure should call update?
                });
            });
        }

        public override void Add(Singer item)
        {
            PerformObjectChanged(item, singer =>
            {
                singer.Order = OrderUtils.GetMaxOrderNumber(singers.ToArray()) + 1;
                singers.Add(singer);
            });
        }

        public override void Remove(Singer item)
        {
            PerformObjectChanged(item, singer =>
            {
                // Shifting order that order is larger than current singer
                OrderUtils.ShiftingOrder(singers.Where(x => x.Order > singer.Order).ToArray(), -1);
                singers.Remove(singer);

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
