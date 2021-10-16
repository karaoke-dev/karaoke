// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Cursor;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public class SettingButton : TriangleButton, IHasTooltip
    {
        public LocalisableString TooltipText { get; set; }

        public SettingButton()
        {
            Width = 90;
            Height = 45;
        }
    }
}
