// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages
{
    public class LanguageExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        [Cached]
        private readonly Bindable<LanguageEditMode> editMode = new();

        public LanguageExtend()
        {
            editMode.BindValueChanged(e =>
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
    }
}
