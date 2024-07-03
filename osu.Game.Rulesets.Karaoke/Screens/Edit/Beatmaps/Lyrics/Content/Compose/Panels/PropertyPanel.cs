// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Panels;

public partial class PropertyPanel : Panel
{
    public PropertyPanel()
    {
        Width = 200;
    }

    protected override IReadOnlyList<Drawable> CreateSections()
    {
        return Array.Empty<Drawable>();
    }
}
