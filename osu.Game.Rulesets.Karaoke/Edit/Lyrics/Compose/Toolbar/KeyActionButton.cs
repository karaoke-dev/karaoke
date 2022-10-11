// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar
{
    public abstract class KeyActionButton : ToolbarButton, IKeyBindingHandler<KaraokeEditAction>, IHasIKeyBindingHandlerOrder
    {
        protected abstract KaraokeEditAction EditAction { get; }

        public int KeyBindingHandlerOrder => int.MinValue;

        public bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e)
        {
            if (e.Action == EditAction)
                ToggleClickEffect();

            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
        {
        }
    }
}
