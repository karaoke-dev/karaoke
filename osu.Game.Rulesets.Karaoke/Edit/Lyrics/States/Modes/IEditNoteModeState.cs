// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public interface IEditNoteModeState : IHasEditModeState<NoteEditMode>, IHasBlueprintSelection<Note>
    {
        Bindable<NoteEditModeSpecialAction> BindableSpecialAction { get; }

        Bindable<NoteEditPropertyMode> NoteEditPropertyMode { get; }
    }
}
