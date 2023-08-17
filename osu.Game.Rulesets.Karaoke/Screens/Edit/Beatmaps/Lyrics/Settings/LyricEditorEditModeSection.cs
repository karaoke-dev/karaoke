// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class LyricEditorEditModeSection<TEditModeState, TEditMode> : LyricEditorEditModeSection<TEditMode>
    where TEditModeState : class, IHasEditStep<TEditMode>
    where TEditMode : struct, Enum
{
    [Resolved]
    private TEditModeState tEditModeState { get; set; } = null!;

    protected sealed override TEditMode DefaultMode() => tEditModeState.EditStep;

    internal sealed override void UpdateEditMode(TEditMode mode)
    {
        tEditModeState.ChangeEditStep(mode);

        base.UpdateEditMode(mode);
    }
}

public abstract partial class LyricEditorEditModeSection<TEditMode> : EditModeSection<TEditMode>
    where TEditMode : struct, Enum
{
    [Resolved]
    private ILyricSelectionState lyricSelectionState { get; set; } = null!;

    protected override DescriptionTextFlowContainer CreateDescriptionTextFlowContainer()
        => new LyricEditorDescriptionTextFlowContainer();

    internal override void UpdateEditMode(TEditMode mode)
    {
        // should cancel the selection after change to the new edit mode.
        lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);

        base.UpdateEditMode(mode);
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
