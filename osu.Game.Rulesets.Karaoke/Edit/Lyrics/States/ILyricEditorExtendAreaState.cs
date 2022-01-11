// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public interface ILyricEditorExtendAreaState
    {
        IBindable<LanguageEditMode> BindableLanguageEditMode { get; }
        IBindable<TextTagEditMode> BindableRubyTagEditMode { get; }
        IBindable<TextTagEditMode> BindableRomajiTagEditMode { get; }
        IBindable<NoteEditMode> BindableNoteEditMode { get; }

        LanguageEditMode LanguageEditMode { get; }

        TextTagEditMode RubyTagEditMode { get; }

        TextTagEditMode RomajiTagEditMode { get; }

        NoteEditMode NoteEditMode { get; }

        void ChangeLanguageEditMode(LanguageEditMode mode);
        void ChangeRubyTagEditMode(TextTagEditMode mode);
        void ChangeRomajiTagEditMode(TextTagEditMode mode);
        void ChangeNoteEditMode(NoteEditMode mode);
    }
}
