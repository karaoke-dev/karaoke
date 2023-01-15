// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage.Settings;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

[Cached(typeof(IStageEditorStateProvider))]
public partial class StageScreen : ClassicStageScreen, IStageEditorStateProvider
{
    public IBindable<StageEditorEditCategory> BindableEditCategory => bindableCategory;
    public IBindable<StageEditorEditMode> BindableEditMode => bindableEditMode;

    private readonly Bindable<StageEditorEditCategory> bindableCategory = new();
    private readonly Bindable<StageEditorEditMode> bindableEditMode = new();

    [Cached(typeof(IStageEditorVerifier))]
    private readonly StageEditorVerifier stageEditorVerifier;

    public StageScreen()
        : base(ClassicStageEditorScreenMode.Stage)
    {
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
            }
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
