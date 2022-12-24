// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class EditorSectionButton : OsuButton
{
    protected EditorSectionButton()
    {
        RelativeSizeAxes = Axes.X;
        Content.CornerRadius = 15;
    }
}
