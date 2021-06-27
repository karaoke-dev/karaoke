// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        private Bindable<LyricEditorMode> bindableMode;

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode = state.BindableMode.GetBoundCopy();
            bindableMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case LyricEditorMode.CreateNote:
                        Children = new Drawable[]
                        {
                            new NoteEditModeSection(),
                            new NoteAutoGenerateSection(),
                        };
                        break;

                    case LyricEditorMode.CreateNotePosition:
                        Children = new Drawable[]
                        {
                            new NoteEditModeSection(),
                        };
                        break;

                    case LyricEditorMode.AdjustNote:
                        Children = new Drawable[]
                        {
                            new NoteEditModeSection(),
                            new NoteConfigSection(),
                            new NoteIssueSection()
                        };
                        break;

                    default:
                        return;
                }
            }, true);
        }
    }
}
