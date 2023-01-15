// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

public partial class StageScreen : ClassicStageScreen, IStageEditorStateProvider
{
    public IBindable<StageEditorEditMode> BindableEditMode => bindableEditMode;
    public IBindable<StageEditorEditCategory> BindableEditCategory => bindableCategory;

    private readonly Bindable<StageEditorEditMode> bindableEditMode = new();
    private readonly Bindable<StageEditorEditCategory> bindableCategory = new();

    public StageScreen()
        : base(ClassicStageEditorScreenMode.Stage)
    {
    }

    public void ChangeEditMode(StageEditorEditMode mode)
    {
        bindableEditMode.Value = mode;
    }

    public void ChangeEditCategory(StageEditorEditCategory mode)
    {
        bindableCategory.Value = mode;
    }
}
