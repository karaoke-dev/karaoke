// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji;

public partial class RubyTagConfigToolSection : EditorSection
{
    protected override LocalisableString Title => "Config Tool";

    [BackgroundDependencyLoader]
    private void load(IEditRubyModeState editRubyModeState)
    {
        Children = new Drawable[]
        {
            new RubyTagEditModeSubsection()
            {
                Current = editRubyModeState.BindableRubyTagEditMode,
            },
        };
    }
}
