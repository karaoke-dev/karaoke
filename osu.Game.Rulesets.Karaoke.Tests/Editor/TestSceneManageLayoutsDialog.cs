// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Layout;
using osu.Game.Rulesets.Karaoke.Edit.Layout.Components;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    [Ignore("Will fix after need this dialog.")]
    public class TestSceneManageLanguagesDialog : OsuManualInputManagerTestScene
    {
        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        private DialogOverlay dialogOverlay;
        private LayoutManager manager;

        private ManageLayoutsDialog dialog;

        [BackgroundDependencyLoader]
        private void load()
        {
            var beatmap = new TestKaraokeBeatmap(null);
            var karaokeBeatmap = new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert();
            var editorBeatmap = new EditorBeatmap(karaokeBeatmap);
            Dependencies.Cache(editorBeatmap);

            base.Content.AddRange(new Drawable[]
            {
                manager = new LayoutManager(),
                Content,
                dialogOverlay = new DialogOverlay()
            });

            Dependencies.Cache(manager);
            Dependencies.Cache(dialogOverlay);
        }

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            manager.Layouts.Clear();
            Child = dialog = new ManageLayoutsDialog();
        });

        [SetUpSteps]
        public void SetUpSteps()
        {
            AddStep("show dialog", () => dialog.Show());
        }
    }
}
