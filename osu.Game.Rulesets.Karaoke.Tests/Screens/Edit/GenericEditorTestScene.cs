// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Overlays;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit;

public abstract partial class GenericEditorTestScene<TEditor, TScreenMode> : ScreenTestScene<TEditor>
    where TEditor : GenericEditor<TScreenMode>, new()
    where TScreenMode : Enum
{
    [Cached(typeof(EditorBeatmap))]
    [Cached(typeof(IBeatSnapProvider))]
    private readonly EditorBeatmap editorBeatmap;

    protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

    protected override TEditor CreateScreen() => new();

    private DialogOverlay dialogOverlay = null!;

    protected GenericEditorTestScene()
    {
        var karaokeBeatmap = CreateBeatmap();
        editorBeatmap = new EditorBeatmap(karaokeBeatmap);
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Beatmap.Value = CreateWorkingBeatmap(editorBeatmap.PlayableBeatmap);

        base.Content.Add(new EditorBeatmapDependencyContainer(editorBeatmap, new BindableBeatDivisor())
        {
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                editorBeatmap,
                Content,
                dialogOverlay = new DialogOverlay(),
            },
        });

        Dependencies.CacheAs<IDialogOverlay>(dialogOverlay);
    }

    protected virtual KaraokeBeatmap CreateBeatmap()
    {
        var beatmap = new TestKaraokeBeatmap(new KaraokeRuleset().RulesetInfo);
        if (new KaraokeBeatmapConverter(beatmap, new KaraokeRuleset()).Convert() is not KaraokeBeatmap karaokeBeatmap)
            throw new ArgumentNullException(nameof(karaokeBeatmap));

        return karaokeBeatmap;
    }

    private partial class EditorBeatmapDependencyContainer : Container
    {
        [Cached]
        private readonly EditorClock editorClock;

        [Cached]
        private readonly BindableBeatDivisor beatDivisor;

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        public EditorBeatmapDependencyContainer(IBeatmap beatmap, BindableBeatDivisor beatDivisor)
        {
            this.beatDivisor = beatDivisor;

            InternalChildren = new Drawable[]
            {
                editorClock = new EditorClock(beatmap, beatDivisor),
                Content,
            };
        }
    }
}
