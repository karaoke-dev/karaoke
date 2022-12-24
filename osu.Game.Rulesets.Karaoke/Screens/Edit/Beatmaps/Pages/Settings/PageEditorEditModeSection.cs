// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages.Settings;

public partial class PageEditorEditModeSection : EditModeSection<PageEditorEditMode>
{
    [Resolved, AllowNull]
    private IPageStateProvider pageStateProvider { get; set; }

    protected override PageEditorEditMode DefaultMode()
        => pageStateProvider.EditMode;

    internal sealed override void UpdateEditMode(PageEditorEditMode mode)
    {
        pageStateProvider.ChangeEditMode(mode);

        base.UpdateEditMode(mode);
    }

    protected override OverlayColourScheme CreateColourScheme()
        => OverlayColourScheme.Green;

    protected override Selection CreateSelection(PageEditorEditMode mode) =>
        mode switch
        {
            PageEditorEditMode.Edit => new Selection(),
            PageEditorEditMode.Verify => new PageEditorVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override LocalisableString GetSelectionText(PageEditorEditMode mode) =>
        mode switch
        {
            PageEditorEditMode.Edit => "Generate",
            PageEditorEditMode.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override Color4 GetSelectionColour(OsuColour colours, PageEditorEditMode mode, bool active) =>
        mode switch
        {
            PageEditorEditMode.Edit => active ? colours.Blue : colours.BlueDarker,
            PageEditorEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override DescriptionFormat GetSelectionDescription(PageEditorEditMode mode) =>
        mode switch
        {
            PageEditorEditMode.Edit => "Edit the page.",
            PageEditorEditMode.Verify => "Check if have any page issues.",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
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
