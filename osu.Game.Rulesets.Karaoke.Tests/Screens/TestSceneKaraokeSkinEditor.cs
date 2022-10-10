// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Game.Database;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public class TestSceneKaraokeSkinEditor : ScreenTestScene<KaraokeSkinEditor>
    {
        // todo: karaoke skin editor might not need editor beatmap, or at least it will be optional.
        [Cached(typeof(EditorBeatmap))]
        private readonly EditorBeatmap editorBeatmap = new(new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
        });

        private KaraokeSkin? karaokeSkin;

        [BackgroundDependencyLoader]
        private void load(SkinManager skinManager)
        {
            skinManager.CurrentSkinInfo.Value = TestingSkin.CreateInfo().ToLiveUnmanaged();

            karaokeSkin = skinManager.CurrentSkin.Value as KaraokeSkin;
        }

        protected override KaraokeSkinEditor CreateScreen() => new(karaokeSkin);

        /// <summary>
        /// todo: it's a tricky way to create ruleset's own skin class.
        /// should use generic skin like <see cref="LegacySkin"/> eventually.
        /// </summary>
        public class TestingSkin : KaraokeSkin
        {
            internal static readonly Guid DEFAULT_SKIN = new("FEC5A291-5709-11EC-9F10-0800200C9A66");

            public static SkinInfo CreateInfo() => new()
            {
                ID = DEFAULT_SKIN,
                Name = "karaoke! (default skin)",
                Creator = "team karaoke!",
                Protected = true,
                InstantiationInfo = typeof(TestingSkin).GetInvariantInstantiationInfo()
            };

            public TestingSkin(IStorageResourceProvider? resources)
                : this(CreateInfo(), resources)
            {
            }

            [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
            public TestingSkin(SkinInfo skin, IStorageResourceProvider? resources)
                : base(skin, resources)
            {
                DefaultElement[ElementType.LyricConfig] = LyricConfig.CreateDefault();
                DefaultElement[ElementType.LyricStyle] = LyricStyle.CreateDefault();
                DefaultElement[ElementType.NoteStyle] = NoteStyle.CreateDefault();
            }
        }
    }
}
