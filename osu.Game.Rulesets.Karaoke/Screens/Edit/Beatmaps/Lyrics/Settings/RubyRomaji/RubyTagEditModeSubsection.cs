// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji;

public partial class RubyTagEditModeSubsection : EditModeSwitchSubsection<RubyTagEditMode>
{
    protected override LocalisableString GetButtonTitle(RubyTagEditMode mode)
        => mode switch
        {
            RubyTagEditMode.Create => "Create",
            RubyTagEditMode.Modify => "Modify",
            _ => throw new InvalidOperationException(nameof(mode)),
        };

    protected override Color4 GetButtonColour(OsuColour colours, RubyTagEditMode mode, bool active)
        => mode switch
        {
            RubyTagEditMode.Create => active ? colours.Green : colours.GreenDarker,
            RubyTagEditMode.Modify => active ? colours.Pink : colours.PinkDarker,
            _ => throw new InvalidOperationException(nameof(mode)),
        };

    protected override DescriptionFormat GetDescription(RubyTagEditMode mode) =>
        mode switch
        {
            RubyTagEditMode.Create => "Use mouse to select range of the lyric text to create the ruby tag.",
            RubyTagEditMode.Modify => "Select ruby to change the start/end position or delete it.",
            _ => throw new InvalidOperationException(nameof(mode)),
        };
}
