// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Settings;

public partial class PageEditorSettingsHeader : EditorSettingsHeader<PageEditorEditMode>
{
    [Resolved]
    private IPageStateProvider pageStateProvider { get; set; } = null!;

    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Green;

    protected override PageEditorEditMode DefaultStep()
        => pageStateProvider.EditMode;

    protected override EditStepTabControl CreateTabControl()
        => new PageEditStepTabControl();

    protected sealed override void UpdateEditStep(PageEditorEditMode step)
    {
        pageStateProvider.ChangeEditMode(step);
    }

    protected override DescriptionFormat GetSelectionDescription(PageEditorEditMode step) =>
        step switch
        {
            PageEditorEditMode.Generate => "Generate the pages by lyric.",
            PageEditorEditMode.Edit => "Batch edit page in here.",
            PageEditorEditMode.Verify => "Check if have any page issues.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class PageEditStepTabControl : EditStepTabControl
    {
        protected override StepTabButton CreateStepButton(OsuColour colours, PageEditorEditMode value)
        {
            return value switch
            {
                PageEditorEditMode.Generate => new StepTabButton(value)
                {
                    Text = "Generate",
                    SelectedColour = colours.Blue,
                    UnSelectedColour = colours.BlueDarker,
                },
                PageEditorEditMode.Edit => new StepTabButton(value)
                {
                    Text = "Edit",
                    SelectedColour = colours.Red,
                    UnSelectedColour = colours.RedDarker,
                },
                PageEditorEditMode.Verify => new VerifyStepTabButton(value)
                {
                    Text = "Verify",
                    SelectedColour = colours.Yellow,
                    UnSelectedColour = colours.YellowDarker,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }

    private partial class VerifyStepTabButton : IssueStepTabButton
    {
        public VerifyStepTabButton(PageEditorEditMode value)
            : base(value)
        {
        }

        [BackgroundDependencyLoader]
        private void load(IPageEditorVerifier pageEditorVerifier)
        {
            Issues.BindTo(pageEditorVerifier.Issues);
        }
    }
}
