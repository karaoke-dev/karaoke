// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.UI.ControlPanel.Pieces
{
    public class TooltipSpriteText : OsuSpriteText, IHasTooltip
    {
        public string TooltipText { get; set; }

        public TooltipSpriteText()
        {
            UseFullGlyphHeight = false;
            Origin = Anchor.CentreLeft;
            Anchor = Anchor.TopLeft;
            Font = new FontUsage(size: 20);
            Alpha = 1;
        }
    }
}
