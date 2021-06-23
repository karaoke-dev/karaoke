// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class TextTagExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 350;

        private Bindable<LyricEditorMode> bindableMode;

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode = state.BindableMode.GetBoundCopy();
            bindableMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case LyricEditorMode.EditRuby:
                        Children = new Drawable[]
                        {
                            new RubyTagEditSection(),
                        };
                        break;

                    case LyricEditorMode.EditRomaji:
                        Children = new Drawable[]
                        {
                            new RomajiTagEditSection(),
                        };
                        break;

                    default:
                        return;
                }
            }, true);
        }
    }
}
