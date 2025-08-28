// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.IO.Stores;
using osu.Framework.Localisation;
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
using osu.Game.Rulesets.Karaoke.Edit.Setup;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.Resources;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Karaoke.Skinning.Argon;
using osu.Game.Rulesets.Karaoke.Skinning.Legacy;
using osu.Game.Rulesets.Karaoke.Skinning.Triangles;
using osu.Game.Rulesets.Karaoke.Statistics;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Replays.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Screens.Edit.Setup;
using osu.Game.Screens.Ranking.Statistics;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke;

public partial class KaraokeRuleset : Ruleset
{
    public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod>? mods = null) => new DrawableKaraokeRuleset(this, beatmap, mods);
    public override ScoreProcessor CreateScoreProcessor() => new KaraokeScoreProcessor();
    public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new KaraokeBeatmapConverter(beatmap, this);
    public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap) => new KaraokeBeatmapProcessor(beatmap);

    public override PerformanceCalculator CreatePerformanceCalculator() => new KaraokePerformanceCalculator();

    public const string SHORT_NAME = "karaoke";

    public const int GAMEPLAY_INPUT_VARIANT = 1;

    public const int EDIT_INPUT_VARIANT = 2;

    public override IEnumerable<int> AvailableVariants => new[] { GAMEPLAY_INPUT_VARIANT, EDIT_INPUT_VARIANT };

    public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) =>
        variant switch
        {
            0 =>
                // Vocal
                Array.Empty<KeyBinding>(),
            GAMEPLAY_INPUT_VARIANT => new[]
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
                new KeyBinding(InputKey.R, KaraokeAction.IncreaseScoringPitch),
                new KeyBinding(InputKey.F, KaraokeAction.DecreaseScoringPitch),
                new KeyBinding(InputKey.V, KaraokeAction.ResetScoringPitch),
            },
            EDIT_INPUT_VARIANT => new[]
            {
                // moving
                new KeyBinding(InputKey.Up, KaraokeEditAction.MoveToPreviousLyric),
                new KeyBinding(InputKey.Down, KaraokeEditAction.MoveToNextLyric),
                new KeyBinding(InputKey.PageUp, KaraokeEditAction.MoveToFirstLyric),
                new KeyBinding(InputKey.PageDown, KaraokeEditAction.MoveToLastLyric),
                new KeyBinding(InputKey.Left, KaraokeEditAction.MoveToPreviousIndex),
                new KeyBinding(InputKey.Right, KaraokeEditAction.MoveToNextIndex),
                new KeyBinding(new[] { InputKey.Alt, InputKey.Left }, KaraokeEditAction.MoveToFirstIndex),
                new KeyBinding(new[] { InputKey.Alt, InputKey.Right }, KaraokeEditAction.MoveToLastIndex),

                new KeyBinding(new[] { InputKey.Alt, InputKey.BracketLeft }, KaraokeEditAction.PreviousEditMode),
                new KeyBinding(new[] { InputKey.Alt, InputKey.BracketRight }, KaraokeEditAction.NextEditMode),

                // Edit Ruby tag.
                new KeyBinding(new[] { InputKey.Z, InputKey.Left }, KaraokeEditAction.EditRubyTagReduceStartIndex),
                new KeyBinding(new[] { InputKey.Z, InputKey.Right }, KaraokeEditAction.EditRubyTagIncreaseStartIndex),
                new KeyBinding(new[] { InputKey.X, InputKey.Left }, KaraokeEditAction.EditRubyTagReduceEndIndex),
                new KeyBinding(new[] { InputKey.X, InputKey.Right }, KaraokeEditAction.EditRubyTagIncreaseEndIndex),

                // edit time-tag.
                new KeyBinding(InputKey.Q, KaraokeEditAction.CreateStartTimeTag),
                new KeyBinding(InputKey.W, KaraokeEditAction.CreateEndTimeTag),
                new KeyBinding(InputKey.A, KaraokeEditAction.RemoveStartTimeTag),
                new KeyBinding(InputKey.S, KaraokeEditAction.RemoveEndTimeTag),
                new KeyBinding(InputKey.Enter, KaraokeEditAction.SetTime),
                new KeyBinding(InputKey.BackSpace, KaraokeEditAction.ClearTime),

                // Action for compose mode.
                new KeyBinding(new[] { InputKey.Control, InputKey.Plus }, KaraokeEditAction.IncreasePreviewFontSize),
                new KeyBinding(new[] { InputKey.Control, InputKey.Minus }, KaraokeEditAction.DecreasePreviewFontSize),
            },
            _ => Array.Empty<KeyBinding>(),
        };

    public override LocalisableString GetVariantName(int variant)
        => variant switch
        {
            GAMEPLAY_INPUT_VARIANT => "Gameplay",
            EDIT_INPUT_VARIANT => "Composer",
            _ => throw new ArgumentNullException(nameof(variant)),
        };

    public override IEnumerable<Mod> GetModsFor(ModType type) =>
        type switch
        {
            ModType.DifficultyReduction => new Mod[]
            {
                new KaraokeModNoFail(),
                new KaraokeModLyricConfiguration(),
                new KaraokeModTranslation(),
            },
            ModType.DifficultyIncrease => new Mod[]
            {
                new KaraokeModHiddenNote(),
                new KaraokeModFlashlight(),
                new MultiMod(new KaraokeModSuddenDeath(), new KaraokeModPerfect(), new KaraokeModWindowsUpdate()),
            },
            ModType.Conversion => new Mod[]
            {
                new MultiMod(new KaraokeModPreviewStage(), new KaraokeModClassicStage()),
            },

            ModType.Automation => new Mod[]
            {
                new MultiMod(new KaraokeModAutoplay(), new KaraokeModAutoplayBySinger()),
            },
            ModType.Fun => new Mod[]
            {
                new KaraokeModPractice(),
                new KaraokeModDisableNote(),
                new KaraokeModSnow(),
            },
            _ => Array.Empty<Mod>(),
        };

    public override Drawable CreateIcon() => new KaraokeIcon(this);

    public override IResourceStore<byte[]> CreateResourceStore()
    {
        var store = new ResourceStore<byte[]>();

        // add resource in the current dll.
        store.AddStore(base.CreateResourceStore());

        // add resource dll, it only works in the local because the resource will be packed into main dll in the resource build.
        store.AddStore(new DllResourceStore(KaraokeResources.ResourceAssembly));

        // add shader resource from font package.
        store.AddStore(new NamespacedResourceStore<byte[]>(new ShaderResourceStore(), "Resources"));

        return store;
    }

    public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new KaraokeDifficultyCalculator(RulesetInfo, beatmap);

    public override HitObjectComposer CreateHitObjectComposer() => new KaraokeHitObjectComposer(this);

    public override IBeatmapVerifier CreateBeatmapVerifier() => new KaraokeBeatmapVerifier();

    public override string Description => "karaoke!";

    public override string ShortName => "karaoke!";

    public override string PlayingVerb => "Singing karaoke";

    public override ISkin CreateSkinTransformer(ISkin skin, IBeatmap beatmap)
    {
        return skin switch
        {
            TrianglesSkin => new KaraokeTrianglesSkinTransformer(skin, beatmap),
            ArgonSkin => new KaraokeArgonSkinTransformer(skin, beatmap),
            DefaultLegacySkin => new KaraokeClassicSkinTransformer(skin, beatmap),
            LegacySkin => new KaraokeLegacySkinTransformer(skin, beatmap),
            _ => throw new InvalidOperationException(),
        };
    }

    public override IConvertibleReplayFrame CreateConvertibleReplayFrame() => new KaraokeReplayFrame();

    public override IRulesetConfigManager CreateConfig(SettingsStore? settings) => new KaraokeRulesetConfigManager(settings, RulesetInfo);

    public override RulesetSettingsSubsection CreateSettings() => new KaraokeSettingsSubsection(this);

    protected override IEnumerable<HitResult> GetValidHitResults()
    {
        return new[]
        {
            HitResult.Great,
            HitResult.Ok,
            HitResult.Meh,
        };
    }

    public override LocalisableString GetDisplayNameForHitResult(HitResult result)
    {
        return result switch
        {
            HitResult.Great => "Great",
            HitResult.Ok => "OK",
            HitResult.Meh => "Meh",
            _ => base.GetDisplayNameForHitResult(result),
        };
    }

    public override StatisticItem[] CreateStatisticsForScore(ScoreInfo score, IBeatmap playableBeatmap)
    {
        const int fix_height = 560;
        const int text_size = 14;
        const int spacing = 15;
        const int info_height = 200;

        // Always display song info
        var statistic = new List<StatisticItem>
        {
            new("Info", () => new BeatmapInfoGraph(playableBeatmap)
            {
                RelativeSizeAxes = Axes.X,
                Height = info_height,
            }),
            new("Metadata", () => new BeatmapMetadataGraph(playableBeatmap)
            {
                RelativeSizeAxes = Axes.X,
                Height = info_height,
            }),
        };

        // Set component to remain height
        const int remain_height = fix_height - text_size - spacing - info_height;

        if (playableBeatmap.IsScorable())
        {
            statistic.Add(new StatisticItem("Scoring Result", () => new ScoringResultGraph(score, playableBeatmap)
            {
                RelativeSizeAxes = Axes.X,
                Height = remain_height - text_size - spacing,
            }));
        }
        else
        {
            statistic.Add(new StatisticItem("Result", () => new NotScorableGraph
            {
                RelativeSizeAxes = Axes.X,
                Height = remain_height - text_size - spacing,
            }));
        }

        return statistic.ToArray();
    }

    public override IEnumerable<Drawable> CreateEditorSetupSections() => new Drawable[]
    {
        new MetadataSection(),
        new ResourcesSection(),
        new KaraokeSingerSection(),
        new KaraokeTranslationSection(),
        new KaraokeNoteSection(),
    };

    public KaraokeRuleset()
    {
        // It's a tricky way to let lazer to read karaoke testing beatmap
        KaraokeLegacyBeatmapDecoder.Register();
        KaraokeJsonBeatmapDecoder.Register();

        // it's a tricky way for loading customized karaoke beatmap.
        RulesetInfo.OnlineID = 111;
    }

    private partial class KaraokeIcon : CompositeDrawable
    {
        private readonly KaraokeRuleset ruleset;

        public KaraokeIcon(KaraokeRuleset ruleset)
        {
            this.ruleset = ruleset;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            // Set a fixed size to make Song Select V2 happy
            Size = new Vector2(32);
        }

        [BackgroundDependencyLoader]
        private void load(IRenderer renderer)
        {
            InternalChildren = new Drawable[]
            {
                new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    Scale = new Vector2(0.9f),
                    Texture = new TextureStore(renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false).Get("Textures/logo"),
                },
                new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Icon = FontAwesome.Regular.Circle,
                },
            };
        }
    }
}
