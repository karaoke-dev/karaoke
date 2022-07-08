// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Screens.Select;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps
{
    [TestFixture]
    public class TestSceneBeatmapInfoWedge : OsuTestScene
    {
        private TestBeatmapInfoWedge infoWedge = null!;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Add(infoWedge = new TestBeatmapInfoWedge
            {
                Size = new Vector2(0.5f, 245),
                RelativeSizeAxes = Axes.X,
                Margin = new MarginPadding { Top = 20 }
            });

            AddStep("show", () =>
            {
                infoWedge.Show();
                infoWedge.Beatmap = Beatmap.Value;
            });
        }

        [TestCase("karaoke-file-samples")]
        [TestCase("karaoke-file-samples-without-note")]
        [TestCase("karaoke-note-samples")]
        [TestCase("karaoke-translate-samples")]
        public void TestNullBeatmap(string fileName)
        {
            using (var resStream = TestResources.OpenBeatmapResource(fileName))
            using (var stream = new LineBufferedReader(resStream))
            {
                var decoder = Decoder.GetDecoder<Beatmap>(stream);
                var beatmap = decoder.Decode(stream);
                selectBeatmap(beatmap, fileName);
            }
        }

        private void selectBeatmap(IBeatmap b, string fileName)
        {
            BeatmapInfoWedge.WedgeInfoText infoBefore = null!;

            string title = string.IsNullOrEmpty(b.Metadata.Title) ? fileName : b.Metadata.Title;
            AddStep($"select {title} beatmap", () =>
            {
                infoBefore = infoWedge.Info;
                infoWedge.Beatmap = Beatmap.Value = CreateWorkingBeatmap(b);
            });

            AddUntilStep("wait for async load", () => infoWedge.Info != infoBefore);
        }

        private class TestBeatmapInfoWedge : BeatmapInfoWedge
        {
            public new WedgeInfoText Info => base.Info;
        }
    }
}
