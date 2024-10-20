// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage;

public interface IStageEditorStateProvider
{
    IBindable<StageEditorEditCategory> BindableEditCategory { get; }

    StageEditorEditCategory EditCategory => BindableEditCategory.Value;

    void ChangeEditCategory(StageEditorEditCategory mode);

    Bindable<StageEditorEditMode> BindableEditMode { get; }

    StageEditorEditMode EditMode => BindableEditMode.Value;

    ClassicStageInfo StageInfo { get; }
}
