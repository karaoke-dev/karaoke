// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.ControlPanel;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModPractice : Mod, IApplicableToDrawableRuleset<KaraokeHitObject>, IApplicableToHUD
    {
        public override string Name => "Practice";
        public override string Acronym => "Practice";
        public override double ScoreMultiplier => 0.0f;
        public override IconUsage Icon => FontAwesome.Solid.Adjust;

        private KaraokePlayfield playfield;

        public void ApplyToDrawableRuleset(DrawableRuleset<KaraokeHitObject> drawableRuleset)
        {
            playfield = drawableRuleset.Playfield as KaraokePlayfield;
        }

        public void ApplyToHUD(HUDOverlay overlay)
        {
            // Create overpay
            var karaokePanelOverlay = new KaraokePanelOverlay(playfield)
            {
                Clock = new FramedClock(new StopwatchClock(true)),
                RelativeSizeAxes = Axes.Both
            };
            overlay.Add(karaokePanelOverlay);

            // Trigger overlay
            playfield.Pressed = pressedAction => { karaokePanelOverlay.OnPressed(pressedAction); };
        }
    }
}
