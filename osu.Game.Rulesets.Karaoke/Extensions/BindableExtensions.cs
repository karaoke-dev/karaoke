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
                var newValue = e.NewValue ?? new T[] { };
                var oldValue = e.OldValue ?? new T[] { };

                var newItems = newValue.Except(oldValue).ToArray();
                var removedItems = oldValue.Except(newValue).ToArray();

                // remove old item.
                remove?.Invoke(removedItems);

                // add new time-tags
                add?.Invoke(newItems);
            });

            if (runOnceImmediately)
            {
                add?.Invoke(bindable.Value);
            }
        }
    }
}
