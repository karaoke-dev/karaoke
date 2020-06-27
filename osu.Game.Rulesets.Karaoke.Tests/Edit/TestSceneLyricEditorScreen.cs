// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Edit.LyricEditor;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneLyricEditorScreen : EditorClockTestScene
    {
        public TestSceneLyricEditorScreen()
        {
            // It's a tricky to let osu! to read karaoke testing beatmap
            KaraokeLegacyBeatmapDecoder.Register();
        }

        private static Beatmap createTestBeatmap()
        {
            using (var stream = TestResources.OpenBeatmapResource("karaoke-file-samples"))
            using (var reader = new LineBufferedReader(stream))
                return Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
        }

        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap =
            new EditorBeatmap(createTestBeatmap());

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

            // Copied from TestSceneHitObjectComposer
            var clock = new DecoupleableInterpolatingFramedClock { IsCoupled = false };
            Dependencies.CacheAs<IAdjustableClock>(clock);
            Dependencies.CacheAs<IFrameBasedClock>(clock);
            Dependencies.CacheAs(editorBeatmap);
            Dependencies.CacheAs<IBeatSnapProvider>(editorBeatmap);

            Child = new LyricEditorScreen();
        }
    }
}
