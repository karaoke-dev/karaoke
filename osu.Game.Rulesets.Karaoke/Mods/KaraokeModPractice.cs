// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Resources.Fonts;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Rulesets.Karaoke.UI.PlayerSettings;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModPractice : ModAutoplay<KaraokeHitObject>, IApplicableToKaraokeHUD, IApplicableToBeatmap
    {
        public override string Name => "Practice";
        public override string Acronym => "Practice";
        public override double ScoreMultiplier => 0.0f;
        public override IconUsage? Icon => KaraokeIcon.ModPractice;
        public override ModType Type => ModType.Fun;

        private DrawableKaraokeRuleset drawableRuleset;
        private RulesetInfo rulesetInfo;
        private KaraokeBeatmap beatmap;

        public void ApplyToBeatmap(IBeatmap beatmap) => this.beatmap = beatmap as KaraokeBeatmap;

        public override void ApplyToDrawableRuleset(DrawableRuleset<KaraokeHitObject> drawableRuleset)
        {
            base.ApplyToDrawableRuleset(drawableRuleset);

            this.drawableRuleset = drawableRuleset as DrawableKaraokeRuleset;
            beatmap = drawableRuleset.Beatmap as KaraokeBeatmap;
            rulesetInfo = drawableRuleset.Ruleset.RulesetInfo;

            if (drawableRuleset.Playfield is KaraokePlayfield karaokePlayfield)
            {
                karaokePlayfield.DisplayCursor = new BindableBool
                {
                    Default = true,
                    Value = true
                };
            }
        }

        public void ApplyToKaraokeHUD(KaraokeHUDOverlay overlay)
        {
            var adjustmentOverlay = new GameplaySettingsOverlay
            {
                RelativeSizeAxes = Axes.Y,
                Anchor = Anchor.CentreRight,
                Origin = Anchor.CentreRight,
            };

            var triggerButton = new ControlLayer.TriggerButton
            {
                Name = "Toggle Practice",
                Text = "Practice",
                TooltipText = "Open/Close practice overlay",
                Action = () => adjustmentOverlay.ToggleVisibility()
            };

            overlay.controlLayer.AddExtraOverlay(triggerButton, adjustmentOverlay);

            // Add practice group into overlay
            adjustmentOverlay.Add(new PracticeSettings(beatmap)
            {
                Expanded = true,
                Width = 400
            });

            // Add playback group into main overlay
            overlay.controlLayer.AddSettingsGroup(new PlaybackSettings { Expanded = false });
        } 
    }
}
