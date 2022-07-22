// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
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
            CheckExactlySelectedOneHitObject();

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
            CheckExactlySelectedOneHitObject();

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

        public void CreateAtPosition()
        {
            AddBelowToSelection(new Lyric
            {
                Text = "New lyric",
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

        public void AddBelowToSelection(Lyric newLyric)
        {
            AddRangeBelowToSelection(new[] { newLyric });
        }

        public void AddRangeBelowToSelection(IEnumerable<Lyric> newlyrics)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                int order = lyric.Order;

                // Shifting order that order is larger than current lyric.
                OrderUtils.ShiftingOrder(HitObjects.Where(x => x.Order > order), newlyrics.Count());

                foreach (var newlyric in newlyrics)
                {
                    newlyric.Order = ++order;
                    Add(newlyric);
                }
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

        protected override void Add<T>(T hitObject)
        {
            if (hitObject is Lyric lyric)
            {
                int index = getInsertIndex(lyric.Order);
                Insert(index, lyric);
            }
            else
            {
                base.Add(hitObject);
            }
        }

        protected override void Insert<T>(int index, T hitObject)
        {
            if (hitObject is Lyric lyric)
            {
                lyric.ID = getId();

                base.Insert(index, lyric);
            }
            else
            {
                base.Add(hitObject);
            }
        }

        private int getInsertIndex(int order)
            => HitObjects.ToList().FindIndex(x => x.Order == order - 1) + 1;

        private int getId()
            => HitObjects.Any() ? HitObjects.Max(x => x.ID) + 1 : 1;
    }
}
