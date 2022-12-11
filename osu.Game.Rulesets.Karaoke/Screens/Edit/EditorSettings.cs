// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class EditorSettings : EditorRoundedScreenSettings
{
    protected void ReloadSections()
    {
        this.ChildrenOfType<FillFlowContainer>().First().Children = CreateSections();
    }

    protected void ChangeBackgroundColour(Colour4 colour4)
    {
        this.ChildrenOfType<Box>().First().Colour = colour4;
    }
}
