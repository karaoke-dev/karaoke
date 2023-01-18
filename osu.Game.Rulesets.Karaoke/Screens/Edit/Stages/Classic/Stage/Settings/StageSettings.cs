// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage.Settings;

public partial class StageSettings : EditorSettings
{
    private readonly IBindable<StageEditorEditCategory> bindableCategory = new Bindable<StageEditorEditCategory>();
    private readonly IBindable<StageEditorEditMode> bindableMode = new Bindable<StageEditorEditMode>();

    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider, IStageEditorStateProvider stageEditorStateProvider)
    {
        bindableCategory.BindTo(stageEditorStateProvider.BindableEditCategory);
        bindableCategory.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);

        bindableMode.BindTo(stageEditorStateProvider.BindableEditMode);
        bindableMode.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);

        // change the background colour to the lighter one.
        ChangeBackgroundColour(colourProvider.Background3);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => bindableCategory.Value switch
    {
        StageEditorEditCategory.Layout => createSectionsForLayoutCategory(bindableMode.Value),
        StageEditorEditCategory.Timing => createSectionsForTimingCategory(bindableMode.Value),
        StageEditorEditCategory.Style => createSectionsForStyleCategory(bindableMode.Value),
        _ => throw new ArgumentOutOfRangeException()
    };

    private static IReadOnlyList<Drawable> createSectionsForLayoutCategory(StageEditorEditMode editMode) => editMode switch
    {
        StageEditorEditMode.Edit => new Drawable[]
        {
            new StageEditorEditModeSection(StageEditorEditCategory.Layout),
        },
        StageEditorEditMode.Verify => new Drawable[]
        {
            new StageEditorEditModeSection(StageEditorEditCategory.Layout),
            new StageEditorIssueSection(StageEditorEditCategory.Layout)
        },
        _ => throw new ArgumentOutOfRangeException()
    };

    private static IReadOnlyList<Drawable> createSectionsForTimingCategory(StageEditorEditMode editMode) => editMode switch
    {
        StageEditorEditMode.Edit => new Drawable[]
        {
            new StageEditorEditModeSection(StageEditorEditCategory.Timing),
            new TimingPointsSection(),
        },
        StageEditorEditMode.Verify => new Drawable[]
        {
            new StageEditorEditModeSection(StageEditorEditCategory.Timing),
            new StageEditorIssueSection(StageEditorEditCategory.Timing)
        },
        _ => throw new ArgumentOutOfRangeException()
    };

    private static IReadOnlyList<Drawable> createSectionsForStyleCategory(StageEditorEditMode editMode) => editMode switch
    {
        StageEditorEditMode.Edit => new Drawable[]
        {
            new StageEditorEditModeSection(StageEditorEditCategory.Style),
        },
        StageEditorEditMode.Verify => new Drawable[]
        {
            new StageEditorEditModeSection(StageEditorEditCategory.Style),
            new StageEditorIssueSection(StageEditorEditCategory.Style)
        },
        _ => throw new ArgumentOutOfRangeException()
    };
}
