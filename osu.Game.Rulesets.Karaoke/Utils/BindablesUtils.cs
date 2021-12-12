// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class BindablesUtils
    {
        public static void Sync<T1, T2>(BindableList<T1> firstBindableList, BindableList<T2> secondBindableList)
        {
            OnyWaySync(firstBindableList, secondBindableList);
            OnyWaySync(secondBindableList, firstBindableList);
        }

        public static void OnyWaySync<T1, T2>(BindableList<T1> firstBindableList, BindableList<T2> secondBindableList)
        {
            // add objects to second list if has default value in first list.
            var defaultItems = firstBindableList.OfType<T2>().Except(secondBindableList);
            secondBindableList.AddRange(defaultItems);

            firstBindableList.CollectionChanged += (_, args) =>
            {
                var newItems = args.NewItems?.OfType<T2>().Except(secondBindableList);
                var oldItems = args.OldItems;

                if (newItems != null && newItems.Any())
                {
                    // add objects to second list if new items have been added.
                    secondBindableList.AddRange(newItems);
                }

                if (oldItems != null && oldItems.Count > 0)
                {
                    // remove objects from second list if exist items has been removed.
                    secondBindableList.RemoveAll(x => args.OldItems.Contains(x));
                }
            };
        }
    }
}
