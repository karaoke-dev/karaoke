// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Mods;

public partial class LanguageSettingsControl : SettingsItem<CultureInfo?>
{
    protected override Drawable CreateControl() => new LanguageSelector
    {
        RelativeSizeAxes = Axes.X,
        Height = 300,
    };
}
