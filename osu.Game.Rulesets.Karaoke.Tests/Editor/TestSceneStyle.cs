// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Style;
using osu.Game.Rulesets.Karaoke.Skinning.Legacy;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneStyle : EditorClockTestScene
    {
        private readonly KaraokeLegacySkinTransformer skin = TestResources.GetKaraokeLegacySkinTransformer("special-skin");

        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        public TestSceneStyle()
        {
            var beatmap = new TestKaraokeBeatmap(null);
            var karaokeBeatmap = new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() as KaraokeBeatmap;
            editorBeatmap = new EditorBeatmap(karaokeBeatmap);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);
            Child = new SkinProvidingContainer(skin)
            {
                RelativeSizeAxes = Axes.Both,
                Child = new StyleScreen(),
            };
        }
    }
}
