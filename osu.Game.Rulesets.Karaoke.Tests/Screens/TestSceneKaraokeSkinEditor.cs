// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public class TestSceneKaraokeSkinEditor : ScreenTestScene<KaraokeSkinEditor>
    {
        // todo: karaoke skin editor might not need editor beatmap, or at least it will be optional.
        [Cached(typeof(EditorBeatmap))]
        private readonly EditorBeatmap editorBeatmap = new(new KaraokeBeatmap());

        protected override KaraokeSkinEditor CreateEditor() => new();

        [BackgroundDependencyLoader]
        private void load()
        {
            // todo: karaoke skin editor might not need editor click eventually?
            Dependencies.Cache(new EditorClock());
        }
    }
}
