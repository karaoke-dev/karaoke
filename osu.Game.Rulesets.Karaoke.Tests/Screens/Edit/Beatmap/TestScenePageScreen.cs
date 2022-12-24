// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap
{
    [TestFixture]
    public partial class TestScenePageScreen : BeatmapEditorScreenTestScene<PageScreen>
    {
        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        protected override PageScreen CreateEditorScreen() => new();

        protected override KaraokeBeatmap CreateBeatmap()
        {
            var karaokeBeatmap = base.CreateBeatmap();

            karaokeBeatmap.PageInfo.Pages.AddRange(new Page[]
            {
                new()
                {
                    Time = 1000,
                },
                new()
                {
                    Time = 2000,
                },
                new()
                {
                    Time = 3000,
                }
            });

            return karaokeBeatmap;
        }

        private DialogOverlay dialogOverlay = null!;
        private LyricsProvider lyricsProvider = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            base.Content.AddRange(new Drawable[]
            {
                Content,
                dialogOverlay = new DialogOverlay(),
                lyricsProvider = new LyricsProvider()
            });

            Dependencies.CacheAs<IDialogOverlay>(dialogOverlay);
            Dependencies.CacheAs<ILyricsProvider>(lyricsProvider);
            Dependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());
        }
    }
}
