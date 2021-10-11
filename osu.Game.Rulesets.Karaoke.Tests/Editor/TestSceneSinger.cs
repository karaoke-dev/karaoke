// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Cursor;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Singers;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneSinger : OsuManualInputManagerTestScene
    {
        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        private DialogOverlay dialogOverlay;

        public TestSceneSinger()
        {
            var beatmap = new TestKaraokeBeatmap(null);
            if (new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() is not KaraokeBeatmap karaokeBeatmap)
                throw new ArgumentNullException(nameof(karaokeBeatmap));

            // todo : insert singers
            karaokeBeatmap.Singers = new[]
            {
                new Singer(1)
                {
                    Order = 1,
                    Name = "初音ミク",
                    RomajiName = "Hatsune Miku",
                    EnglishName = "Miku",
                    Description = "International superstar vocaloid Hatsune Miku.",
                    Color = Colour4.AliceBlue
                },
                new Singer(2)
                {
                    Order = 2,
                    Name = "ハク",
                    RomajiName = "haku",
                    EnglishName = "andy840119",
                    Description = "Creator of this ruleset.",
                    Color = Colour4.Yellow
                },
                new Singer(3)
                {
                    Order = 3,
                    Name = "ゴミパソコン",
                    RomajiName = "gomi-pasokonn",
                    EnglishName = "garbage desktop",
                    Description = "My fucking slow desktop.",
                    Color = Colour4.Brown
                }
            };

            editorBeatmap = new EditorBeatmap(karaokeBeatmap);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

            base.Content.AddRange(new Drawable[]
            {
                new OsuContextMenuContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = Content
                },
                dialogOverlay = new DialogOverlay()
            });

            var beatDivisor = new BindableBeatDivisor
            {
                Value = Beatmap.Value.BeatmapInfo.BeatDivisor
            };
            var editorClock = new EditorClock(Beatmap.Value.Beatmap, beatDivisor) { IsCoupled = false };
            Dependencies.CacheAs(editorClock);
            Dependencies.Cache(beatDivisor);
            Dependencies.Cache(dialogOverlay);
        }

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = new SingerScreen();
        });

        [Test]
        public void HoverToSingerArea()
        {
            // todo : add this step because description is not showing.
            AddStep("Move mouse to singer area", () => InputManager.MoveMouseTo(Child, new Vector2(-450, -90)));
        }
    }
}
