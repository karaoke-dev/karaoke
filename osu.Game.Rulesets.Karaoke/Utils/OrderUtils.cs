// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class OrderUtils
    {
        /// <summary>
        /// Check objects contains duplicated ids.
        /// </summary>
        /// <typeparam name="T">IHasOrder</typeparam>
        /// <param name="objects">objects</param>
        /// <returns>contain duplicated id or not</returns>
        public static bool ContainDuplicatedId<T>(T[] objects) where T : IHasOrder
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));

            return objects.Length != objects.Select(x => x.Order).Distinct().Count();
        }

        /// <summary>
        /// Get min order number
        /// </summary>
        /// <typeparam name="T">IHasOrder</typeparam>
        /// <param name="objects">objects</param>
        /// <returns>min order number.</returns>
        public static int GetMinOrderNumber<T>(T[] objects) where T : IHasOrder
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));

            return objects.OrderBy(x => x.Order).FirstOrDefault()?.Order ?? 0;
        }

        /// <summary>
        /// Get max order number
        /// </summary>
        /// <typeparam name="T">IHasOrder</typeparam>
        /// <param name="objects">objects</param>
        /// <returns>max order number.</returns>
        public static int GetMaxOrderNumber<T>(T[] objects) where T : IHasOrder
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));

            return objects.OrderByDescending(x => x.Order).FirstOrDefault()?.Order ?? 0;
        }

        /// <summary>
        /// Get sorted objects
        /// </summary>
        /// <typeparam name="T">IHasOrder</typeparam>
        /// <param name="objects">objects</param>
        /// <returns>sorted result</returns>
        public static T[] Sorted<T>(IEnumerable<T> objects) where T : IHasOrder
        {
            return objects?.OrderBy(x => x.Order).ToArray();
        }

        /// <summary>
        /// Shifting order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="shifting"></param>
        public static void ShiftingOrder<T>(T[] objects, int shifting) where T : class, IHasOrder
        {
            foreach (var processObject in objects)
            {
                processObject.Order += shifting;
            }
        }

        /// <summary>
        /// Re-generate order number if has gap between two order number
        /// </summary>
        /// <example>
        /// Valid: 1, 2, 3, 4
        /// Should be generated: 1, 3, 4, 5, 7
        /// </example>
        /// <typeparam name="T">IHasOrder</typeparam>
        /// <param name="objects">objects</param>
        /// <param name="startFrom">start order should from</param>
        /// <param name="changeOrderAction">has call-back if order has been changed.</param>
        public static void ResortOrder<T>(T[] objects, int startFrom = 1, Action<T, int, int> changeOrderAction = null) where T : IHasOrder
        {
            int minOrderNumber = GetMinOrderNumber(objects.ToArray());
            int maxOrderNumber = GetMaxOrderNumber(objects.ToArray());

            // todo : should deal with the case if new start order is between min and max order number
            bool orderByAsc = startFrom <= minOrderNumber;
            var processObjects = orderByAsc ? objects.OrderBy(x => x.Order) : objects.OrderByDescending(x => x.Order);

            int targetOrder = orderByAsc ? startFrom : startFrom - minOrderNumber + objects.Length;

            foreach (var processObject in processObjects)
            {
                if (processObject.Order != targetOrder)
                    changeOrder(processObject, targetOrder);

                targetOrder = orderByAsc ? targetOrder + 1 : targetOrder - 1;
            }

            void changeOrder(T obj, int newOrder)
            {
                int oldOrder = obj.Order;
                obj.Order = newOrder;

                // call invoke for outside updating.
                changeOrderAction?.Invoke(obj, oldOrder, newOrder);
            }
        }

        /// <summary>
        /// Change order.
        /// </summary>
        /// <typeparam name="T">IHasOrder</typeparam>
        /// <param name="objects">objects</param>
        /// <param name="oldOrder">old order</param>
        /// <param name="newOrder">new oder</param>
        /// <param name="changeOrderAction">has call-back if order has been changed.</param>
        public static void ChangeOrder<T>(T[] objects, int oldOrder, int newOrder, Action<T, int, int> changeOrderAction = null) where T : IHasOrder
        {
            if (oldOrder == newOrder)
                return;

            // check old order number should be in the exist list
            if (!objects.Select(x => x.Order).Contains(oldOrder))
                throw new ArgumentOutOfRangeException(nameof(oldOrder), $"new order number {oldOrder} is not in the range of {nameof(objects)}.");

            // check new order number should be in the exist list
            if (!objects.Select(x => x.Order).Contains(newOrder))
                throw new ArgumentOutOfRangeException(nameof(newOrder), $"new order number {newOrder} is not in the range of {nameof(objects)}.");

            // get objects that will need to change order
            int minAffectOrder = Math.Min(oldOrder, newOrder);
            int maxAffectOrder = Math.Max(oldOrder, newOrder);
            var affectObjects = objects.Where(x => x.Order >= minAffectOrder && x.Order <= maxAffectOrder);

            // get shifting order
            int shiftingOrder = newOrder > oldOrder ? -1 : 1;

            // get order order object info
            const int old_order_temp_id = -1;
            var oldOrderObject = objects.FirstOrDefault(x => x.Order == oldOrder);

            // set old order to -1 for order duplicated issue
            changeOrder(oldOrderObject, old_order_temp_id);

            // switching order
            affectObjects = shiftingOrder > 0 ? affectObjects.OrderByDescending(x => x.Order) : affectObjects.OrderBy(x => x.Order);

            foreach (var affectObject in affectObjects)
            {
                if (affectObject.Order == old_order_temp_id)
                    continue;

                int affectObjectNewOrder = affectObject.Order + shiftingOrder;
                changeOrder(affectObject, affectObjectNewOrder);
            }

            // set old order to new order
            changeOrder(oldOrderObject, newOrder);

            // post check should not have duplicated ids.
            ContainDuplicatedId(objects.ToArray());

            void changeOrder(T obj, int n)
            {
                int o = obj.Order;
                obj.Order = n;

                // call invoke for outside updating.
                changeOrderAction?.Invoke(obj, o, n);
            }
        }
    }
}
