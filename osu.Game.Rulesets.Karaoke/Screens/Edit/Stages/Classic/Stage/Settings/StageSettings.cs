// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage.Settings;

public partial class StageSettings : EditorSettings
{
    private readonly IBindable<StageEditorEditCategory> bindableCategory = new Bindable<StageEditorEditCategory>();
    private readonly Bindable<StageEditorEditMode> bindableMode = new();

    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider, IStageEditorStateProvider stageEditorStateProvider)
    {
        bindableCategory.BindTo(stageEditorStateProvider.BindableEditCategory);
        bindableCategory.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);

        bindableMode.BindTo(stageEditorStateProvider.BindableEditMode);

        // change the background colour to the lighter one.
        ChangeBackgroundColour(colourProvider.Background3);
    }

    protected override EditorSettingsHeader CreateSettingHeader()
        => new StageEditorSettingsHeader(bindableCategory.Value)
        {
            Current = bindableMode,
        };

    protected override IReadOnlyList<EditorSection> CreateEditorSections() => bindableCategory.Value switch
    {
        StageEditorEditCategory.Layout => createSectionsForLayoutCategory(bindableMode.Value),
        StageEditorEditCategory.Timing => createSectionsForTimingCategory(bindableMode.Value),
        StageEditorEditCategory.Style => createSectionsForStyleCategory(bindableMode.Value),
        _ => throw new ArgumentOutOfRangeException(),
    };

    private static IReadOnlyList<EditorSection> createSectionsForLayoutCategory(StageEditorEditMode editMode) => editMode switch
    {
        StageEditorEditMode.Edit => Array.Empty<EditorSection>(),
        StageEditorEditMode.Verify => new EditorSection[]
        {
            new StageEditorIssueSection(StageEditorEditCategory.Layout),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };

    private static IReadOnlyList<EditorSection> createSectionsForTimingCategory(StageEditorEditMode editMode) => editMode switch
    {
        StageEditorEditMode.Edit => new[]
        {
            new TimingPointsSection(),
        },
        StageEditorEditMode.Verify => new[]
        {
            new StageEditorIssueSection(StageEditorEditCategory.Timing),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };

    private static IReadOnlyList<EditorSection> createSectionsForStyleCategory(StageEditorEditMode editMode) => editMode switch
    {
        StageEditorEditMode.Edit => Array.Empty<EditorSection>(),
        StageEditorEditMode.Verify => new[]
        {
            new StageEditorIssueSection(StageEditorEditCategory.Style),
        },
        _ => throw new ArgumentOutOfRangeException(),
    };
}
