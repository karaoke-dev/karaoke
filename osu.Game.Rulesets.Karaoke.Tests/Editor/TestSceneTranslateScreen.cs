// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Translate;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [TestFixture]
    public class TestSceneTranslateScreen : KaraokeEditorScreenTestScene<TranslateScreen>
    {
        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        protected override TranslateScreen CreateEditorScreen() => new();

        protected override KaraokeBeatmap CreateBeatmap()
        {
            var karaokeBeatmap = base.CreateBeatmap();

            karaokeBeatmap.AvailableTranslates = new List<CultureInfo>
            {
                new("zh-TW"),
                new("en-US"),
                new("ja-JP")
            };

            return karaokeBeatmap;
        }

        private DialogOverlay dialogOverlay;

        [BackgroundDependencyLoader]
        private void load()
        {
            base.Content.AddRange(new Drawable[]
            {
                Content,
                dialogOverlay = new DialogOverlay(),
            });

            Dependencies.Cache(dialogOverlay);
        }
    }
}
