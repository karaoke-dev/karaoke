// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Cursor;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Singers;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneSinger : OsuManualInputManagerTestScene
    {
        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        public TestSceneSinger()
        {
            var beatmap = new TestKaraokeBeatmap(null);
            var karaokeBeatmap = new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() as KaraokeBeatmap;
            // todo : insert singers
            karaokeBeatmap.SingerMetadata.CreateSinger(singer =>
            {
                singer.Name = "初音ミク";
                singer.RomajiName = "Hatsune Miku";
                singer.EnglishName = "Miku";
                singer.Description = "International superstar vocaloid Hatsune Miku.";
                singer.Color = Colour4.AliceBlue;
            });
            karaokeBeatmap.SingerMetadata.CreateSinger(singer =>
            {
                singer.Name = "ハク";
                singer.RomajiName = "haku";
                singer.EnglishName = "andy840119";
                singer.Description = "Creator of this ruleset.";
                singer.Color = Colour4.Yellow;
            });
            karaokeBeatmap.SingerMetadata.CreateSinger(singer =>
            {
                singer.Name = "ゴミパソコン";
                singer.RomajiName = "gomi-pasokonn";
                singer.EnglishName = "garbage desktop";
                singer.Description = "My fucking slow desktop.";
                singer.Color = Colour4.Brown;
            });

            editorBeatmap = new EditorBeatmap(karaokeBeatmap);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

            var beatDivisor = new BindableBeatDivisor
            {
                Value = Beatmap.Value.BeatmapInfo.BeatDivisor
            };
            var editorClock = new EditorClock(Beatmap.Value, beatDivisor) { IsCoupled = false };
            Dependencies.CacheAs(editorClock);
            Dependencies.Cache(beatDivisor);

            base.Content.Add(new OsuContextMenuContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = Content
            });
        }

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = new SingerScreen();
        });

        [Test]
        public void HoverToSingerArea()
        {
            // todo : add this step because the description is not showing.
            AddStep("Move mouse to singer area", () => InputManager.MoveMouseTo(Child, new Vector2(-450, -90)));
        }
    }
}
