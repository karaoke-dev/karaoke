// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Rulesets.Karaoke.UI.PlayerSettings;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Users;

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

        public override Score CreateReplayScore(IBeatmap beatmap, IReadOnlyList<Mod> mods) => new()
        {
            ScoreInfo = new ScoreInfo { User = new User { Username = "practice master" } },
            Replay = new KaraokeAutoGenerator(beatmap, mods).Generate(),
        };

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

        public void ApplyToOverlay(ISettingHUDOverlay overlay)
        {
            // Add practice overlay
            overlay.AddExtraOverlay(new PracticeOverlay(beatmap));

            // Add playback group into main overlay
            overlay.AddSettingsGroup(new PlaybackSettings { Expanded = false });
        }

        private class PracticeOverlay : RightSideOverlay
        {
            public PracticeOverlay(IBeatmap beatmap)
            {
                Add(new PracticeSettings(beatmap)
                {
                    Expanded = true,
                    Width = 400
                });
            }

            public override SettingButton CreateToggleButton() => new()
            {
                Name = "Toggle Practice",
                Text = "Practice",
                TooltipText = "Open/Close practice overlay",
                Action = ToggleVisibility
            };
        }
    }
}
