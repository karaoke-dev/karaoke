// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Settings;

public partial class PageEditorEditStepSection : EditStepSection<PageEditorEditMode>
{
    [Resolved]
    private IPageStateProvider pageStateProvider { get; set; } = null!;

    protected override PageEditorEditMode DefaultStep()
        => pageStateProvider.EditMode;

    internal sealed override void UpdateEditStep(PageEditorEditMode step)
    {
        pageStateProvider.ChangeEditMode(step);

        base.UpdateEditStep(step);
    }

    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Green;

    protected override Selection CreateSelection(PageEditorEditMode step) =>
        step switch
        {
            PageEditorEditMode.Generate => new Selection(),
            PageEditorEditMode.Edit => new Selection(),
            PageEditorEditMode.Verify => new PageEditorVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override LocalisableString GetSelectionText(PageEditorEditMode step) =>
        step switch
        {
            PageEditorEditMode.Generate => "Generate",
            PageEditorEditMode.Edit => "Edit",
            PageEditorEditMode.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override Color4 GetSelectionColour(OsuColour colours, PageEditorEditMode step, bool active) =>
        step switch
        {
            PageEditorEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
            PageEditorEditMode.Edit => active ? colours.Red : colours.RedDarker,
            PageEditorEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    protected override DescriptionFormat GetSelectionDescription(PageEditorEditMode step) =>
        step switch
        {
            PageEditorEditMode.Generate => "Generate the pages by lyric.",
            PageEditorEditMode.Edit => "Batch edit page in here.",
            PageEditorEditMode.Verify => "Check if have any page issues.",
            _ => throw new ArgumentOutOfRangeException(nameof(step), step, null),
        };

    private partial class PageEditorVerifySelection : VerifySelection
    {
        [BackgroundDependencyLoader]
        private void load(IPageEditorVerifier pageEditorVerifier)
        {
            Issues.BindTo(pageEditorVerifier.Issues);
        }
    }
}
