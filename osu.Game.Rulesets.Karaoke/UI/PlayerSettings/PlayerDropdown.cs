// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings;

public partial class PlayerEnumDropdown<T> : SettingsDropdown<T> where T : struct, Enum
{
    protected override OsuDropdown<T> CreateDropdown() => new EnumDropdown();

    protected partial class EnumDropdown : OsuEnumDropdown<T>
    {
        public EnumDropdown()
        {
            RelativeSizeAxes = Axes.X;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            // todo: adjust the style.
        }
    }
}
