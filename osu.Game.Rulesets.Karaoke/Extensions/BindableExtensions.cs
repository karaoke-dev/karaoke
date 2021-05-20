// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    public static class BindableExtensions
    {
        public static void BindArrayChanged<T>(this IBindable<T[]> bindable, Action<T[]> add, Action<T[]> remove, bool runOnceImmediately = false)
        {
            // Add time-tag into blueprint container
            bindable.BindValueChanged(e =>
            {
                // remove old item.
                var removedItems = e.OldValue?.Except(e.NewValue).ToArray() ?? new T[] {};
                remove?.Invoke(removedItems);

                // add new time-tags
                var newItems = e.NewValue ?? new T[] { };
                add?.Invoke(newItems);
            }, runOnceImmediately);
        }
    }
}
