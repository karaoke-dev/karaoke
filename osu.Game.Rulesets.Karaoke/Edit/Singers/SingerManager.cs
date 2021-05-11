// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    public class SingerManager : Component
    {
        public readonly BindableFloat BindableZoom = new BindableFloat();

        public readonly BindableFloat BindableCurrent = new BindableFloat();

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
                Singers.AddRange(karaokeBeatmap.Singers);
            }
        }

        public void ChangeOrder(Singer singer, int newIndex)
        {
            var oldOrder = singer.Order;
            var newOrder = newIndex + 1; // order is start from 1
            OrderUtils.ChangeOrder(Singers.ToArray(), oldOrder, newOrder, (switchSinger, oldOrder, newOrder) =>
            {
                // todo : not really sure should call update?
            });
        }

        public void CreateSinger(Singer singer)
        {
            singer.Order = OrderUtils.GetMaxOrderNumber(Singers.ToArray()) + 1;
            Singers.Add(singer);
        }

        public void DeleteSinger(Singer singer)
        {
            // Shifting order that order is larger than current singer
            OrderUtils.ShiftingOrder(Singers.Where(x => x.Order > singer.Order).ToArray(), -1);
            Singers.Remove(singer);
        }
    }
}
