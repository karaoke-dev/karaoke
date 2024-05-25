// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public abstract partial class LyricEditorSettingsHeader<TEditStep> : EditorSettingsHeader<TEditStep>
    where TEditStep : struct, Enum
{
    [Resolved]
    private ILyricSelectionState lyricSelectionState { get; set; } = null!;

    protected override DescriptionTextFlowContainer CreateDescriptionTextFlowContainer()
        => new LyricEditorDescriptionTextFlowContainer();

    protected override void UpdateEditStep(TEditStep step)
    {
        // should cancel the selection after change to the new edit step.
        lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);
    }

    protected sealed partial class VerifyStepTabButton : IssueStepTabButton
    {
        [Resolved]
        private ILyricEditorVerifier verifier { get; set; } = null!;

        public VerifyStepTabButton(TEditStep value)
            : base(value)
        {
        }

        private LyricEditorMode editorMode;

        public LyricEditorMode EditMode
        {
            get => editorMode;
            set
            {
                editorMode = value;

                Schedule(() =>
                {
                    Issues.BindTo(verifier.GetIssueByType(EditMode));
                });
            }
        }
    }
}
