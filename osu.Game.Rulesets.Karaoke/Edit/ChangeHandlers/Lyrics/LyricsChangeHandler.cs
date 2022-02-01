// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricsChangeHandler : HitObjectChangeHandler<Lyric>, ILyricsChangeHandler
    {
        public void Split(int index)
        {
            PerformOnSelection(lyric =>
            {
                // Shifting order that order is larger than current lyric.
                int lyricOrder = lyric.Order;
                OrderUtils.ShiftingOrder(HitObjects.Where(x => x.Order > lyricOrder), 1);

                // Split lyric
                var (firstLyric, secondLyric) = LyricsUtils.SplitLyric(lyric, index);
                firstLyric.Order = lyric.Order;
                firstLyric.ID = getId();
                secondLyric.Order = lyric.Order + 1;
                secondLyric.ID = getId() + 1;

                // Add those tho lyric and remove old one.
                Add(secondLyric);
                Add(firstLyric);
                Remove(lyric);
            });
        }

        public void Combine()
        {
            PerformOnSelection(lyric =>
            {
                var previousLyric = HitObjects.GetPrevious(lyric);
                if (previousLyric == null)
                    throw new ArgumentNullException(nameof(previousLyric));

                // Shifting order that order is larger than current lyric.
                int lyricOrder = previousLyric.Order;
                OrderUtils.ShiftingOrder(HitObjects.Where(x => x.Order > lyricOrder), -1);

                var newLyric = LyricsUtils.CombineLyric(previousLyric, lyric);
                newLyric.Order = lyricOrder;

                // Add created lyric and remove old two.
                Add(newLyric);
                Remove(previousLyric);
                Remove(lyric);
            });
        }

        public void CreateAtPosition(int? nextToOrder = null)
        {
            PerformOnSelection(lyric =>
            {
                int order = lyric.Order;

                // Shifting order that order is larger than current lyric.
                OrderUtils.ShiftingOrder(HitObjects.Where(x => x.Order > order), 1);

                // Add new lyric to target order.
                var createLyric = new Lyric
                {
                    Text = "New lyric",
                    Order = order + 1,
                };
                Add(createLyric);
            });
        }

        public void CreateAtLast()
        {
            int order = OrderUtils.GetMaxOrderNumber(HitObjects.ToArray());

            // Add new lyric to target order.
            Add(new Lyric
            {
                Text = "New lyric",
                Order = order + 1,
            });
        }

        public void Remove()
        {
            PerformOnSelection(lyric =>
            {
                // Shifting order that order is larger than current lyric.
                OrderUtils.ShiftingOrder(HitObjects.Where(x => x.Order > lyric.Order), -1);
                Remove(lyric);
            });
        }

        public void ChangeOrder(int newOrder)
        {
            PerformOnSelection(lyric =>
            {
                int oldOrder = lyric.Order;
                OrderUtils.ChangeOrder(HitObjects.ToArray(), oldOrder, newOrder + 1, (switchSinger, oldOrder, newOrder) =>
                {
                    // todo : not really sure should call update?
                });
            });
        }

        protected override void Add(Lyric hitObject)
        {
            hitObject.ID = getId();

            int index = HitObjects.ToList().FindIndex(x => x.Order == hitObject.Order - 1) + 1;
            Insert(index, hitObject);
        }

        private int getId()
            => HitObjects.Max(x => x.ID) + 1;
    }
}
