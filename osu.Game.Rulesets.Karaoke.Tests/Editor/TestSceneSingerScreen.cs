// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Cursor;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Singers;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneSingerScreen : KaraokeEditorScreenTestScene<SingerScreen>
    {
        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        protected override SingerScreen CreateEditorScreen() => new();

        protected override KaraokeBeatmap CreateBeatmap()
        {
            var karaokeBeatmap = base.CreateBeatmap();

            // todo : insert singers
            karaokeBeatmap.Singers = new List<Singer>
            {
                new(1)
                {
                    Order = 1,
                    Name = "初音ミク",
                    RomajiName = "Hatsune Miku",
                    EnglishName = "Miku",
                    Description = "International superstar vocaloid Hatsune Miku.",
                    Hue = 189 / 360f,
                },
                new(2)
                {
                    Order = 2,
                    Name = "ハク",
                    RomajiName = "haku",
                    EnglishName = "andy840119",
                    Description = "Creator of this ruleset.",
                    Hue = 46 / 360f,
                },
                new(3)
                {
                    Order = 3,
                    Name = "ゴミパソコン",
                    RomajiName = "gomi-pasokonn",
                    EnglishName = "garbage desktop",
                    Description = "My fucking slow desktop.",
                    Hue = 290 / 360f,
                }
            };

            return karaokeBeatmap;
        }

        private DialogOverlay dialogOverlay = null!;
        private LyricsProvider lyricsProvider = null!;
        private KaraokeBeatmapResourcesProvider karaokeBeatmapResourcesProvider = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            base.Content.AddRange(new Drawable[]
            {
                new OsuContextMenuContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = Content
                },
                dialogOverlay = new DialogOverlay(),
                lyricsProvider = new LyricsProvider(),
                karaokeBeatmapResourcesProvider = new KaraokeBeatmapResourcesProvider(),
            });

            var beatDivisor = new BindableBeatDivisor
            {
                Value = Beatmap.Value.BeatmapInfo.BeatDivisor
            };
            var editorClock = new EditorClock(Beatmap.Value.Beatmap, beatDivisor) { IsCoupled = false };
            Dependencies.CacheAs(editorClock);
            Dependencies.Cache(beatDivisor);
            Dependencies.CacheAs<IDialogOverlay>(dialogOverlay);
            Dependencies.CacheAs<ILyricsProvider>(lyricsProvider);
            Dependencies.CacheAs<IKaraokeBeatmapResourcesProvider>(karaokeBeatmapResourcesProvider);
        }
    }
}
