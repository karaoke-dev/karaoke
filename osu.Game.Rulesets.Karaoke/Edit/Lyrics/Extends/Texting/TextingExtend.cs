// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Texting
{
    public class TextingExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        private readonly IBindable<TextingEditMode> bindableMode = new Bindable<TextingEditMode>();

        public TextingExtend()
        {
            bindableMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case TextingEditMode.Typing:
                        Children = new[]
                        {
                            new TextingEditModeSection(),
                        };
                        break;

                    case TextingEditMode.Split:
                        Children = new Drawable[]
                        {
                            new TextingEditModeSection(),
                            new ManageSwitchSpecialActionSection()
                        };
                        break;

                    default:
                        return;
                }
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(ITextingModeState textingModeState)
        {
            bindableMode.BindTo(textingModeState.BindableEditMode);
        }
    }
}
