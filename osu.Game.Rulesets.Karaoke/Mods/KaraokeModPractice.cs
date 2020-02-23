// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Rulesets.Karaoke.UI.PlayerSettings;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Karaoke.Resources.Textures;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModPractice : ModAutoplay<KaraokeHitObject>, IApplicableToDrawableRuleset<KaraokeHitObject>, IApplicableToHUD, IApplicableToTrack, IApplicableToBeatmap
    {
        public override string Name => "Practice";
        public override string Acronym => "Practice";
        public override double ScoreMultiplier => 0.0f;
        public override IconUsage? Icon => KaraokeIcon.ModPractice;
        public override ModType Type => ModType.Fun;

        private DrawableKaraokeRuleset drawableRuleset;
        private RulesetInfo rulesetInfo;
        private KaraokeBeatmap beatmap;

        private HUDOverlay overlay;

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

        public void ApplyToTrack(Track track)
        {
            // Create overlay
            overlay?.Add(new KaraokeActionContainer(rulesetInfo, drawableRuleset)
            {
                RelativeSizeAxes = Axes.Both,
                Child = new KaraokePracticeContainer(beatmap, track)
                {
                    Clock = new FramedClock(new StopwatchClock(true)),
                    RelativeSizeAxes = Axes.Both
                }
            });
        }

        public void ApplyToHUD(HUDOverlay overlay)
        {
            this.overlay = overlay;
        }

        public class KaraokeActionContainer : DatabasedKeyBindingContainer<KaraokeAction>
        {
            private readonly DrawableKaraokeRuleset drawableRuleset;

            protected IRulesetConfigManager Config;

            public KaraokeActionContainer(RulesetInfo ruleset, DrawableKaraokeRuleset drawableRuleset)
                : base(ruleset, 0, SimultaneousBindingMode.Unique)
            {
                this.drawableRuleset = drawableRuleset;
            }

            protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            {
                var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
                dependencies.Cache(drawableRuleset.Config);
                dependencies.Cache(drawableRuleset.Session);
                return dependencies;
            }
        }

        public class KaraokePracticeContainer : ControlOverlay
        {
            private readonly GameplaySettingsOverlay adjustmentOverlay;

            public KaraokePracticeContainer(KaraokeBeatmap beatmap, Track track)
            {
                AddExtraOverlay(new TriggerButton
                {
                    Name = "Toggle Practice",
                    Text = "Practice",
                    TooltipText = "Open/Close practice overlay",
                    Action = () => adjustmentOverlay.ToggleVisibility()
                },
                adjustmentOverlay = new GameplaySettingsOverlay
                {
                    RelativeSizeAxes = Axes.Y,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                });

                // Add practice group into overlay
                adjustmentOverlay.Add(new PracticeSettings(beatmap)
                {
                    Expanded = true,
                    Width = 400
                });

                // Add playback group into main overlay
                AddSettingsGroup(new PlaybackSettings { Expanded = false });

                // Add translate group if this beatmap has translate
                var translateDictionary = beatmap.HitObjects.OfType<TranslateDictionary>().FirstOrDefault();
                if (translateDictionary != null && translateDictionary.Translates.Any())
                    AddSettingsGroup(new TranslateSettings(translateDictionary) { Expanded = false });
            }
        }
    }
}
