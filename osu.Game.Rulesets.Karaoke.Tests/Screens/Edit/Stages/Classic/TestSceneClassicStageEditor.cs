// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Stages.Classic;

public partial class TestSceneClassicStageEditor : ScreenTestScene<ClassicStageEditor>
{
    [Cached(typeof(EditorBeatmap))]
    [Cached(typeof(IBeatSnapProvider))]
    private readonly EditorBeatmap editorBeatmap;

    protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

    protected override ClassicStageEditor CreateScreen() => new();

    private DialogOverlay dialogOverlay = null!;

    public TestSceneClassicStageEditor()
    {
        var beatmap = new TestKaraokeBeatmap(new KaraokeRuleset().RulesetInfo);
        var karaokeBeatmap = new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() as KaraokeBeatmap;
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
        });

        Dependencies.CacheAs<IDialogOverlay>(dialogOverlay);
    }
}
