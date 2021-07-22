// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components
{
    public abstract class ExtendSelectionHandler<T> : SelectionHandler<T>
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            // because it will still show selection box even all selected items is not in current handler.
            // so should hide the selection box by hand.
            // todo : this is the temp way, might remove after official fix that.
            SelectedItems.CollectionChanged += (sender, args) =>
            {
                Scheduler.AddOnce(updateVisibility);
            };
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
