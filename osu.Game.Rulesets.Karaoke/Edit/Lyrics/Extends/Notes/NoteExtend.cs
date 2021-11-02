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

        [Cached]
        private Bindable<NoteEditMode> bindableMode = new();

        [Cached]
        private readonly Bindable<NoteEditPropertyMode> noteEditPropertyMode = new();

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case NoteEditMode.Generate:
                        Children = new Drawable[]
                        {
                            new NoteEditModeSection(),
                            new NoteConfigSection(),
                            new NoteAutoGenerateSection(),
                        };
                        break;

                    case NoteEditMode.Edit:
                        Children = new Drawable[]
                        {
                            new NoteEditModeSection(),
                            new NoteEditPropertyModeSection(),
                            new NoteEditPropertySection(),
                        };
                        break;

                    case NoteEditMode.Verify:
                        Children = new Drawable[]
                        {
                            new NoteEditModeSection(),
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
