// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Difficulty;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Resources.Fonts;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke
{
    public class KaraokeRuleset : Ruleset
    {
        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableKaraokeRuleset(this, beatmap, mods);
        public override ScoreProcessor CreateScoreProcessor() => new KaraokeScoreProcessor();
        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new KaraokeBeatmapConverter(beatmap, this);
        public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap) => new KaraokeBeatmapProcessor(beatmap);

        public const string SHORT_NAME = "karaoke";

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0)
        {
            switch (variant)
            {
                case 0:
                    return new[]
                    {
                        // Basic control
                        new KeyBinding(InputKey.Number1, KaraokeAction.FirstLyric),
                        new KeyBinding(InputKey.Left, KaraokeAction.PreviousLyric),
                        new KeyBinding(InputKey.Right, KaraokeAction.NextLyric),
                        new KeyBinding(InputKey.Space, KaraokeAction.PlayAndPause),

                        // Panel
                        new KeyBinding(InputKey.P, KaraokeAction.OpenPanel),

                        // Advance control
                        new KeyBinding(InputKey.Q, KaraokeAction.IncreaseTempo),
                        new KeyBinding(InputKey.A, KaraokeAction.DecreaseTempo),
                        new KeyBinding(InputKey.Z, KaraokeAction.ResetTempo),
                        new KeyBinding(InputKey.W, KaraokeAction.IncreasePitch),
                        new KeyBinding(InputKey.S, KaraokeAction.DecreasePitch),
                        new KeyBinding(InputKey.X, KaraokeAction.ResetPitch),
                        new KeyBinding(InputKey.E, KaraokeAction.IncreaseVocalPitch),
                        new KeyBinding(InputKey.D, KaraokeAction.DecreaseVocalPitch),
                        new KeyBinding(InputKey.C, KaraokeAction.ResetVocalPitch),
                        new KeyBinding(InputKey.R, KaraokeAction.IncreaseSaitenPitch),
                        new KeyBinding(InputKey.F, KaraokeAction.DecreaseSaitenPitch),
                        new KeyBinding(InputKey.V, KaraokeAction.ResetSaitenPitch),
                    };

                case 1:
                    //Vocal
                    return Array.Empty<KeyBinding>();

                default:
                    return Array.Empty<KeyBinding>();
            }
        }

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new KaraokeModNoFail(),
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new KaraokeModHiddenNote(),
                        new KaraokeModFlashlight(),
                        new MultiMod(new KaraokeModSuddenDeath(), new KaraokeModPerfect(), new KaraokeModWindowsUpdate()),
                    };

                case ModType.Automation:
                    return new Mod[]
                    {
                        new KaraokeModAutoplay(),
                    };

                case ModType.Fun:
                    return new Mod[]
                    {
                        new KaraokeModPractice(),
                        new KaraokeModDisableNote(),
                        new KaraokeModSnow(),
                    };

                default:
                    return Array.Empty<Mod>();
            }
        }

        public override Drawable CreateIcon() => new SpriteIcon { Icon = KaraokeIcon.RulesetKaraoke };

        public override DifficultyCalculator CreateDifficultyCalculator(WorkingBeatmap beatmap) => new KaraokeDifficultyCalculator(this, beatmap);

        public override HitObjectComposer CreateHitObjectComposer() => new KaraokeHitObjectComposer(this);

        public override string Description => "karaoke!";

        public override string ShortName => "osu!karaoke";

        public override string PlayingVerb => "Singing karaoke";

        public override ISkin CreateLegacySkinProvider(ISkinSource source) => new KaraokeLegacySkinTransformer(source);

        public override IRulesetConfigManager CreateConfig(SettingsStore settings) => new KaraokeRulesetConfigManager(settings, RulesetInfo);

        public override RulesetSettingsSubsection CreateSettings() => new KaraokeSettingsSubsection(this);

        public KaraokeRuleset()
        {
            // It's a tricky to let osu! to read karaoke testing beatmap
            KaraokeLegacyBeatmapDecoder.Register();
        }
    }
}
