// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.UI.PlayerSettings;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public class ControlOverlay : RightSideOverlay
    {
        public ControlOverlay(IBeatmap beatmap)
        {
            // Add common group
            Add(new VisualSettings { Expanded = false });
            Add(new PitchSettings { Expanded = false });
            Add(new RubyRomajiSettings { Expanded = false });

            // Add translate group if this beatmap has translate
            if (beatmap.AnyTranslate())
                Add(new TranslateSettings(beatmap.AvailableTranslates()) { Expanded = false });
        }

        public override SettingButton CreateToggleButton() => new()
        {
            Name = "Toggle setting button",
            Text = "Settings",
            TooltipText = "Open/Close setting",
            Action = ToggleVisibility
        };
    }
}
