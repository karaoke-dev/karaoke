// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class FontSelectionDialog : TitleFocusedOverlayContainer, IHasCurrentValue<FontUsage>
    {
        protected override string Title => "Select font";

        private readonly BindableWithCurrent<FontUsage> current = new BindableWithCurrent<FontUsage>();

        public Bindable<FontUsage> Current
        {
            get => current.Current;
            set => current.Current = value;
        }
    }
}
