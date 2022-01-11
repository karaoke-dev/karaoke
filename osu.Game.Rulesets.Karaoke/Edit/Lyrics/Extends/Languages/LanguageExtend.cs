// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class LanguageExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        private readonly IBindable<LanguageEditMode> bindableMode = new Bindable<LanguageEditMode>();

        public LanguageExtend()
        {
            bindableMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case LanguageEditMode.Generate:
                        Children = new Drawable[]
                        {
                            new LanguageEditModeSection(),
                            new LanguageAutoGenerateSection(),
                        };
                        break;

                    case LanguageEditMode.Verify:
                        Children = new Drawable[]
                        {
                            new LanguageEditModeSection(),
                            new LanguageMissingSection(),
                        };
                        break;

                    default:
                        return;
                }
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorExtendAreaState lyricEditorExtendAreaState)
        {
            bindableMode.BindTo(lyricEditorExtendAreaState.BindableLanguageEditMode);
        }
    }
}
