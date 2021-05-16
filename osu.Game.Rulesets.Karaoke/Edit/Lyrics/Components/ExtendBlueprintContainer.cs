// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components
{
    public abstract class ExtendBlueprintContainer<T> : BlueprintContainer<T> where T : class
    {
        protected void RegistBindable<TItem>(Bindable<TItem[]> bindable) where TItem : T
        {
            // Add time-tag into blueprint container
            bindable.BindValueChanged(e =>
            {
                // remove old item.
                var removedItems = e.OldValue?.Except(e.NewValue).ToList();

                if (removedItems != null)
                {
                    foreach (var obj in removedItems)
                        RemoveBlueprintFor(obj);
                }

                // add new time-tags
                if (e.NewValue != null)
                {
                    foreach (var obj in e.NewValue)
                        AddBlueprintFor(obj);
                }
            }, true);
        }
    }
}
