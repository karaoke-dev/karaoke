// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

public partial class PageSettings : EditorSettings
{
    protected override IReadOnlyList<Drawable> CreateSections() => new Drawable[]
    {
        // todo: should create section for able to show all the pages and the invalid page info.
    };

    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider)
    {
        // change the background colour to the lighter one.
        ChangeBackgroundColour(colourProvider.Background3);
    }
}
