// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage.Settings;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

[Cached(typeof(IStageEditorStateProvider))]
public partial class StageScreen : ClassicStageScreen, IStageEditorStateProvider
{
    public IBindable<StageEditorEditCategory> BindableEditCategory => bindableCategory;
    public IBindable<StageEditorEditMode> BindableEditMode => bindableEditMode;

    private readonly Bindable<StageEditorEditCategory> bindableCategory = new();
    private readonly Bindable<StageEditorEditMode> bindableEditMode = new();

    [Cached(typeof(IBeatmapClassicStageChangeHandler))]
    private readonly BeatmapClassicStageChangeHandler beatmapClassicStageChangeHandler;

    [Cached(typeof(IStageEditorVerifier))]
    private readonly StageEditorVerifier stageEditorVerifier;

    [Resolved]
    private EditorBeatmap editorBeatmap { get; set; } = null!;

    public ClassicStageInfo StageInfo
    {
        get
        {
            // we should make sure that current stage info is classic stage info.
            // otherwise, we might not able to see the edit result in the editor.
            var currentStageInfo = EditorBeatmapUtils.GetPlayableBeatmap(editorBeatmap).CurrentStageInfo;
            if (currentStageInfo is not ClassicStageInfo classicStageInfo)
                throw new NotSupportedException();

            return classicStageInfo;
        }
    }

    public StageScreen()
        : base(ClassicStageEditorScreenMode.Stage)
    {
        AddInternal(beatmapClassicStageChangeHandler = new BeatmapClassicStageChangeHandler());
        AddInternal(stageEditorVerifier = new StageEditorVerifier());

        Child = new GridContainer
        {
            RelativeSizeAxes = Axes.Both,
            ColumnDimensions = new[]
            {
                new Dimension(),
                new Dimension(GridSizeMode.Absolute, 250),
            },
            Content = new[]
            {
                new Drawable[]
                {
                    new StageEditor
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    new StageSettings(),
                },
            },
        };
    }

    public void ChangeEditCategory(StageEditorEditCategory mode)
    {
        bindableCategory.Value = mode;
    }

    public void ChangeEditMode(StageEditorEditMode mode)
    {
        bindableEditMode.Value = mode;
    }
}
