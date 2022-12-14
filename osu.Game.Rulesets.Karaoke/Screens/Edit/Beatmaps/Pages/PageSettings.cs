// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

public partial class PageSettings : EditorSettings
{
    private readonly IBindable<PageEditorEditMode> bindableMode = new Bindable<PageEditorEditMode>();

    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider, IPageStateProvider pageStateProvider)
    {
        bindableMode.BindTo(pageStateProvider.BindableEditMode);
        bindableMode.BindValueChanged(e =>
        {
            ReloadSections();
        }, true);

        // change the background colour to the lighter one.
        ChangeBackgroundColour(colourProvider.Background3);
    }

    protected override IReadOnlyList<Drawable> CreateSections() => bindableMode.Value switch
    {
        PageEditorEditMode.Edit => new Drawable[]
        {
            new PageEditorEditModeSection(),
            new PagesSection(),
        },
        PageEditorEditMode.Verify => new Drawable[]
        {
            new PageEditorEditModeSection(),
            new PageEditorIssueSection(),
        },
        _ => throw new ArgumentOutOfRangeException()
    };
}
