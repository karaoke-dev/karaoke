// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Globalization;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Translate;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneTranslate : EditorClockTestScene
    {
        [Cached(typeof(EditorBeatmap))]
        [Cached(typeof(IBeatSnapProvider))]
        private readonly EditorBeatmap editorBeatmap;

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        private DialogOverlay dialogOverlay;
        private LanguageSelectionDialog languageSelectionDialog;

        public TestSceneTranslate()
        {
            var beatmap = new TestKaraokeBeatmap(null);
            if (!(new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() is KaraokeBeatmap karaokeBeatmap))
                throw new ArgumentNullException(nameof(karaokeBeatmap));

            karaokeBeatmap.AvailableTranslates = new[]
            {
                new CultureInfo("zh-TW"),
                new CultureInfo("en-US"),
                new CultureInfo("ja-JP")
            };

            editorBeatmap = new EditorBeatmap(karaokeBeatmap);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

            base.Content.AddRange(new Drawable[]
            {
                Content,
                dialogOverlay = new DialogOverlay(),
                languageSelectionDialog = new LanguageSelectionDialog()
            });

            Dependencies.Cache(dialogOverlay);
            Dependencies.Cache(languageSelectionDialog);

            Child = new TranslateScreen();
        }
    }
}
