// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.UI.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public class PracticeOverlay : SettingOverlay
    {
        protected override OverlayColourScheme OverlayColourScheme => OverlayColourScheme.Purple;

        public PracticeOverlay()
        {
            Children = new[]
            {
                new PracticeSettings
                {
                    Expanded =
                    {
                        Value = true
                    },
                    Width = 400
                }
            };
        }

        protected override SettingButton CreateButton() => new()
        {
            Name = "Toggle Practice",
            Text = "Practice",
            TooltipText = "Open/Close practice overlay",
            Action = ToggleVisibility
        };
    }
}
