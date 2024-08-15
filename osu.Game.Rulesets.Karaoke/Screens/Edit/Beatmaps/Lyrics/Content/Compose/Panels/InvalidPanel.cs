// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Panels;

public partial class InvalidPanel : Panel
{
    public InvalidPanel()
    {
        Width = 200;
        Height = 300;
    }

    protected override IReadOnlyList<Drawable> CreateSections() =>
        new Drawable[]
        {
            new IssueSection(),
        };
}
