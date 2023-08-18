// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class LyricEditorEditStepSection<TEditStepState, TEditStep> : LyricEditorEditStepSection<TEditStep>
    where TEditStepState : class, IHasEditStep<TEditStep>
    where TEditStep : struct, Enum
{
    [Resolved]
    private TEditStepState tEditStepState { get; set; } = null!;

    protected sealed override TEditStep DefaultStep() => tEditStepState.EditStep;

    internal sealed override void UpdateEditStep(TEditStep step)
    {
        tEditStepState.ChangeEditStep(step);

        base.UpdateEditStep(step);
    }
}

public abstract partial class LyricEditorEditStepSection<TEditStep> : EditStepSection<TEditStep>
    where TEditStep : struct, Enum
{
    [Resolved]
    private ILyricSelectionState lyricSelectionState { get; set; } = null!;

    protected override DescriptionTextFlowContainer CreateDescriptionTextFlowContainer()
        => new LyricEditorDescriptionTextFlowContainer();

    internal override void UpdateEditStep(TEditStep step)
    {
        // should cancel the selection after change to the new edit step.
        lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);

        base.UpdateEditStep(step);
    }

    protected abstract partial class LyricEditorVerifySelection : VerifySelection
    {
        [BackgroundDependencyLoader]
        private void load(ILyricEditorVerifier verifier)
        {
            Issues.BindTo(verifier.GetIssueByType(EditMode));
        }

        protected abstract LyricEditorMode EditMode { get; }
    }
}
