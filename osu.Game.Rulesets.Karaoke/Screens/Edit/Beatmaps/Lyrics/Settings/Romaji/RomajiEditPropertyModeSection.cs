// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romaji;

public partial class RomajiEditPropertyModeSection : EditorSection
{
    protected override LocalisableString Title => "Edit property";

    [BackgroundDependencyLoader]
    private void load(IEditRomanisationModeState editRomanisationModeState)
    {
        Children = new Drawable[]
        {
            new LabelledEnumDropdown<RomajiEditPropertyMode>
            {
                Label = "Edit property",
                Description = "Batch edit text or other romaji-related properties from time-tag",
                Current = editRomanisationModeState.BindableRomajiEditPropertyMode,
            },
        };
    }
}
