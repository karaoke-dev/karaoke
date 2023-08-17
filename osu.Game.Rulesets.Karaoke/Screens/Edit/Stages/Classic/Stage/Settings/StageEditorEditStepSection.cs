// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage.Settings;

public partial class StageEditorEditStepSection : EditStepSection<StageEditorEditMode>
{
    [Resolved]
    private IStageEditorStateProvider stageEditorStateProvider { get; set; } = null!;

    private readonly StageEditorEditCategory category;

    public StageEditorEditStepSection(StageEditorEditCategory category)
    {
        this.category = category;
    }

    protected override StageEditorEditMode DefaultStep()
        => stageEditorStateProvider.EditMode;

    internal sealed override void UpdateEditStep(StageEditorEditMode step)
    {
        stageEditorStateProvider.ChangeEditMode(step);

        base.UpdateEditStep(step);
    }

    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Green;

    protected override Selection CreateSelection(StageEditorEditMode step) =>
        step switch
        {
            StageEditorEditMode.Edit => new Selection(),
            StageEditorEditMode.Verify => new StageEditorVerifySelection(category),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(StageEditorEditMode step) =>
        step switch
        {
            StageEditorEditMode.Edit => "Edit",
            StageEditorEditMode.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, StageEditorEditMode step, bool active) =>
        step switch
        {
            StageEditorEditMode.Edit => active ? colours.Red : colours.RedDarker,
            StageEditorEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override DescriptionFormat GetSelectionDescription(StageEditorEditMode step) =>
        step switch
        {
            StageEditorEditMode.Edit => "Edit the stage property in here.",
            StageEditorEditMode.Verify => "Check if have any stage issues.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class StageEditorVerifySelection : VerifySelection
    {
        private readonly StageEditorEditCategory category;

        public StageEditorVerifySelection(StageEditorEditCategory category)
        {
            this.category = category;
        }

        [BackgroundDependencyLoader]
        private void load(IStageEditorVerifier stageEditorVerifier)
        {
            Issues.BindTo(stageEditorVerifier.GetIssueByType(category));
        }
    }
}
