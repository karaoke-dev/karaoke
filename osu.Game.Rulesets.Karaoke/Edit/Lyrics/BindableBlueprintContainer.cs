// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Specialized;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public abstract class BindableBlueprintContainer<T> : BlueprintContainer<T> where T : class
    {
        private readonly BindableList<T> bindableList = new();

        protected BindableBlueprintContainer()
        {
            bindableList.BindCollectionChanged((_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var obj in args.NewItems.OfType<T>())
                            AddBlueprintFor(obj);

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (var obj in args.OldItems.OfType<T>())
                            RemoveBlueprintFor(obj);

                        break;
                }
            });
        }

        protected void RegisterBindable(BindableList<T> bindable)
        {
            bindableList.UnbindBindings();
            bindableList.BindTo(bindable);
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

        protected override void SelectAll()
        {
            SelectedItems.AddRange(bindableList);
        }

        private bool containsSelectionFromOtherBlueprintContainer()
        {
            var items = SelectionBlueprints.Select(x => x.Item);

            // check any selected items that is not in current blueprint container.
            return SelectedItems.Any(x => !items.Contains(x));
        }

        public abstract class BindableSelectionHandler : SelectionHandler<T>
        {
            protected override void OnSelectionChanged()
            {
                base.OnSelectionChanged();

                updateVisibility();
            }

            /// <summary>
            /// Updates whether this <see cref="SelectionHandler{T}"/> is visible.
            /// </summary>
            private void updateVisibility()
            {
                bool visible = containsSelectionInCurrentBlueprintContainer();
                SelectionBox.FadeTo(visible ? 1f : 0.0f);
            }

            private bool containsSelectionInCurrentBlueprintContainer()
            {
                var items = SelectedBlueprints.Select(x => x.Item);

                // check any selected items that is in current blueprint container.
                return SelectedItems.Any(x => items.Contains(x));
            }
        }
    }
}
