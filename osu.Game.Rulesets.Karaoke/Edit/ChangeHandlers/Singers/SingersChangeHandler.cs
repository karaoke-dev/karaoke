// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers
{
    public class SingersChangeHandler : BeatmapPropertyChangeHandler<Singer>, ISingersChangeHandler
    {
        public BindableList<Singer> Singers => Items;

        public override List<Singer> GetItemsFromBeatmap(KaraokeBeatmap beatmap)
            => beatmap.Singers;

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

        protected override void OnItemAdded(Singer item)
        {
            // should give it a id.
            item.Order = OrderUtils.GetMaxOrderNumber(Singers.ToArray()) + 1;
        }

        protected override void OnItemRemoved(Singer item)
        {
            // should clear removed singer ids in singer editor.
            Lyrics.ForEach(x =>
            {
                x.Singers.Remove(item.ID);
            });
        }
    }
}
