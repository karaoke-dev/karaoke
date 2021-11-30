// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
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
        private readonly EditorBeatmap editorBeatmap = new(new KaraokeBeatmap());

        private KaraokeSkin karaokeSkin;

        [BackgroundDependencyLoader]
        private void load(SkinManager skinManager)
        {
            // todo: karaoke skin editor might not need editor click eventually?
            Dependencies.Cache(new EditorClock());

            // because skin is compared by id, so should change id to let skin manager info knows that skin has been changed.
            var defaultSkin = DefaultKaraokeSkin.Default;
            defaultSkin.ID = 100;
            skinManager.CurrentSkinInfo.Value = defaultSkin;

            karaokeSkin = skinManager.CurrentSkin.Value as KaraokeSkin;
        }

        protected override KaraokeSkinEditor CreateScreen() => new(karaokeSkin);
    }
}
