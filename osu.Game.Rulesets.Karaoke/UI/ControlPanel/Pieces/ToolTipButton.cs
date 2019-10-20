// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Cursor;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.UI.ControlPanel.Pieces
{
    public class ToolTipButton : TriangleButton, IHasTooltip
    {
        public string TooltipText { get; set; }
    }
}
