// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage.Settings;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

[Cached(typeof(IStageEditorStateProvider))]
public partial class StageScreen : ClassicStageScreen, IStageEditorStateProvider
{
    public IBindable<StageEditorEditCategory> BindableEditCategory => bindableCategory;
    public Bindable<StageEditorEditMode> BindableEditMode { get; } = new();

    private readonly Bindable<StageEditorEditCategory> bindableCategory = new();

    [Cached(typeof(IClassicStageChangeHandler))]
    private readonly ClassicStageChangeHandler classicStageChangeHandler;

    [Cached(typeof(IStageEditorVerifier))]
    private readonly StageEditorVerifier stageEditorVerifier;

    public ClassicStageInfo StageInfo
    {
        get
        {
            // todo: should be able to read the stage info from the beatmap.
            throw new NotImplementedException();
        }
    }

    public StageScreen()
        : base(ClassicStageEditorScreenMode.Stage)
    {
        AddInternal(classicStageChangeHandler = new ClassicStageChangeHandler());
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
}
