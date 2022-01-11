// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public class LyricEditorExtendAreaState : Component, ILyricEditorExtendAreaState
    {
        private readonly Bindable<LanguageEditMode> bindableLanguageEditMode = new();
        private readonly Bindable<TextTagEditMode> bindableRubyTagEditMode = new(TextTagEditMode.Edit);
        private readonly Bindable<TextTagEditMode> bindableRomajiTagEditMode = new(TextTagEditMode.Edit);
        private readonly Bindable<NoteEditMode> bindableNoteEditMode = new();

        public IBindable<LanguageEditMode> BindableLanguageEditMode => bindableLanguageEditMode;
        public IBindable<TextTagEditMode> BindableRubyTagEditMode => bindableRubyTagEditMode;
        public IBindable<TextTagEditMode> BindableRomajiTagEditMode => bindableRomajiTagEditMode;
        public IBindable<NoteEditMode> BindableNoteEditMode => bindableNoteEditMode;

        public LanguageEditMode LanguageEditMode => bindableLanguageEditMode.Value;

        public TextTagEditMode RubyTagEditMode => bindableRubyTagEditMode.Value;

        public TextTagEditMode RomajiTagEditMode => bindableRomajiTagEditMode.Value;

        public NoteEditMode NoteEditMode => bindableNoteEditMode.Value;

        public void ChangeLanguageEditMode(LanguageEditMode mode)
        {
            bindableLanguageEditMode.Value = mode;
        }

        public void ChangeRubyTagEditMode(TextTagEditMode mode)
        {
            bindableRubyTagEditMode.Value = mode;
        }

        public void ChangeRomajiTagEditMode(TextTagEditMode mode)
        {
            bindableRomajiTagEditMode.Value = mode;
        }

        public void ChangeNoteEditMode(NoteEditMode mode)
        {
            bindableNoteEditMode.Value = mode;
        }
    }
}
