// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings.Notes
{
    public class NoteEditPropertyModeSection : LyricEditorSection
    {
        protected override LocalisableString Title => "Edit property";

        [BackgroundDependencyLoader]
        private void load(IEditNoteModeState editNoteModeState)
        {
            Children = new Drawable[]
            {
                new LabelledEnumDropdown<NoteEditPropertyMode>
                {
                    Label = "Edit property",
                    Description = "Batch edit text, ruby(alternative) text or display from notes",
                    Current = editNoteModeState.NoteEditPropertyMode,
                },
            };
        }
    }
}
