// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteEditPropertyModeSection : Section
    {
        protected override string Title => "Edit property";

        [BackgroundDependencyLoader]
        private void load(Bindable<NoteEditPropertyMode> bindableNoteEditPropertyMode)
        {
            Children = new Drawable[]
            {
                new LabelledEnumDropdown<NoteEditPropertyMode>
                {
                    Label = "Edit property",
                    Description = "Batch edit text, ruby(alternative) text or display from notes",
                    Current = bindableNoteEditPropertyMode,
                },
            };
        }
    }
}
