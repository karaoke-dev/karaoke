// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Cursor;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.UI.Overlays.Settings
{
    public class SettingButton : TriangleButton, IHasTooltip
    {
        public string TooltipText { get; set; }

        public SettingButton()
        {
            Width = 90;
            Height = 45;
        }
    }
}
