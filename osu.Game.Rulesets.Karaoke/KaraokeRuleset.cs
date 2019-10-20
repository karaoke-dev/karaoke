// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Difficulty;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke
{
    public class KaraokeRuleset : Ruleset
    {
        public override DrawableRuleset CreateDrawableRulesetWith(IWorkingBeatmap beatmap, IReadOnlyList<Mod> mods) => new DrawableKaraokeRuleset(this, beatmap, mods);
        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new KaraokeBeatmapConverter(beatmap);
        public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap) => new KaraokeBeatmapProcessor(beatmap);

        public const string SHORT_NAME = "karaoke";

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
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
            new KeyBinding(InputKey.E, KaraokeAction.IncreaseLyricAppearTime),
            new KeyBinding(InputKey.D, KaraokeAction.DecreaseLyricAppearTime),
            new KeyBinding(InputKey.C, KaraokeAction.ResetLyricAppearTime),
        };

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new KaraokeModPractice(),
                    };
                case ModType.Fun:
                    return new Mod[]
                    {
                        new KaraokeModSnow(),
                    };
                default:
                    return new Mod[] { };
            }
        }

        public override DifficultyCalculator CreateDifficultyCalculator(WorkingBeatmap beatmap) => new KaraokeDifficultyCalculator(this, beatmap);

        public override HitObjectComposer CreateHitObjectComposer() => new KaraokeHitObjectComposer(this);

        public override string Description => "karaoke!";

        public override string ShortName => "osu!karaoke";

        public override ISkin CreateLegacySkinProvider(ISkinSource source) => new KaraokeLegacySkinTransformer(source);

        public override int? LegacyID => 111;

        public KaraokeRuleset(RulesetInfo rulesetInfo = null)
            : base(rulesetInfo)
        {
            // It's a tricky to let lazer to read karaoke testing beatmap
            KaroakeLegacyBeatmapDecoder.Register();
        }
    }
}
