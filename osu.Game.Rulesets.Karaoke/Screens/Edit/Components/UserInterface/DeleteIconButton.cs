// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Components.UserInterface;

public partial class DeleteIconButton : IconButton
{
    [Resolved]
    protected OsuColour Colours { get; private set; } = null!;

    public Action<bool>? Hover;

    public DeleteIconButton()
    {
        Icon = FontAwesome.Solid.Trash;
    }

    protected override bool OnHover(HoverEvent e)
    {
        Colour = Colours.Yellow;
        Hover?.Invoke(true);
        return base.OnHover(e);
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        Colour = Colours.GrayF;
        Hover?.Invoke(false);
        base.OnHoverLost(e);
    }
}
