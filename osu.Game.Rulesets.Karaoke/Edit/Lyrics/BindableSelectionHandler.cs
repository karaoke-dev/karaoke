// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Graphics;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public abstract class BindableSelectionHandler<T> : SelectionHandler<T>
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
