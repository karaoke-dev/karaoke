// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class IHasOrdersUtils
    {
        public static bool ContainDuplicatedId<T>(T[] objects) where T : IHasOrder
        {
            return objects.Length != objects.Select(x => x.Order).Distinct().Count();
        }

        public static T[] GetOrder<T>(IEnumerable<T> objects) where T : IHasOrder
        {
            return objects.OrderBy(x => x.Order).ToArray();
        }

        public static void ChangeOrder()
        {
            // todo : change order
        }

        public static void Insert()
        {
            // todo : insert
        }

        public static void Delete()
        {
            // todo : delete
        }
    }
}
