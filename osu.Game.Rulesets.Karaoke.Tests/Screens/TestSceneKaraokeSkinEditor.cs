// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Database;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Rulesets.Karaoke.Skinning;
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
            skinManager.CurrentSkinInfo.Value = DefaultKaraokeSkin.CreateInfo().ToLiveUnmanaged();

            karaokeSkin = skinManager.CurrentSkin.Value as KaraokeSkin;
        }

        protected override KaraokeSkinEditor CreateScreen() => new(karaokeSkin);
    }
}
