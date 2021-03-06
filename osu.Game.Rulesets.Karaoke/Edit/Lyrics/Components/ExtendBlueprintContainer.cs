﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components
{
    public abstract class ExtendBlueprintContainer<T> : BlueprintContainer<T> where T : class
    {
        protected void RegisterBindable<TItem>(Bindable<TItem[]> bindable) where TItem : T
        {
            // Add time-tag into blueprint container
            bindable.BindArrayChanged(addItems =>
            {
                foreach (var obj in addItems)
                    AddBlueprintFor(obj);
            }, removedItems =>
            {
                foreach (var obj in removedItems)
                    RemoveBlueprintFor(obj);
            }, true);
        }

        protected override bool OnDragStart(DragStartEvent e)
        {
            if (!base.OnDragStart(e))
                return false;

            // should clear all selected text-tag if start selecting.
            if (containsSelectionFromOtherBlueprintContainer())
                DeselectAll();

            return true;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (!base.OnClick(e))
                return false;

            // should clear all selected text-tag if start selecting.
            if (containsSelectionFromOtherBlueprintContainer())
                DeselectAll();

            return true;
        }

        /// <summary>
        /// This function will de-select all selection in relative blueprint container
        /// </summary>
        protected virtual void DeselectAll()
        {
        }

        private bool containsSelectionFromOtherBlueprintContainer()
        {
            var items = SelectionBlueprints.Select(x => x.Item);

            // check any selected items that is not in current blueprint container.
            return SelectedItems.Any(x => !items.Contains(x));
        }
    }
}
