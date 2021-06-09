// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Resources.Fonts;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Overlays;
using osu.Game.Rulesets.Karaoke.UI.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.UI.Overlays.Settings.PlayerSettings;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModPractice : ModAutoplay, IApplicableToDrawableRuleset<KaraokeHitObject>, IApplicableToSettingHUDOverlay, IApplicableToBeatmap
    {
        public override string Name => "Practice";
        public override string Acronym => "Practice";
        public override double ScoreMultiplier => 0.0f;
        public override IconUsage? Icon => KaraokeIcon.ModPractice;
        public override ModType Type => ModType.Fun;

        private KaraokeBeatmap beatmap;

        public void ApplyToBeatmap(IBeatmap beatmap) => this.beatmap = beatmap as KaraokeBeatmap;

        public void ApplyToDrawableRuleset(DrawableRuleset<KaraokeHitObject> drawableRuleset)
        {
            beatmap = drawableRuleset.Beatmap as KaraokeBeatmap;

            if (drawableRuleset.Playfield is KaraokePlayfield karaokePlayfield)
            {
                karaokePlayfield.DisplayCursor = new BindableBool
                {
                    Default = true,
                    Value = true
                };
            }
        }

        public void ApplyToOverlay(SettingHUDOverlay overlay)
        {
            // Add practice overlay
            overlay.AddExtraOverlay(new PracticeOverlay(beatmap));

            // Add playback group into main overlay
            overlay.AddSettingsGroup(new PlaybackSettings { Expanded = false });
        }

        public class PracticeOverlay : RightSideOverlay
        {
            public PracticeOverlay(IBeatmap beatmap)
            {
                Add(new PracticeSettings(beatmap)
                {
                    Expanded = true,
                    Width = 400
                });
            }

            public override SettingButton CreateToggleButton() => new SettingButton
            {
                Name = "Toggle Practice",
                Text = "Practice",
                TooltipText = "Open/Close practice overlay",
                Action = ToggleVisibility
            };
        }
    }
}
