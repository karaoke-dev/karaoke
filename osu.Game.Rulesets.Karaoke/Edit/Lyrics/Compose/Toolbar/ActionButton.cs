// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar
{
    public abstract class ActionButton : ToolbarButton
    {
        [Resolved, AllowNull]
        private OsuColour colours { get; set; }

        protected void ToggleClickEffect()
        {
            if (Enabled.Value)
            {
                IconContainer.FadeOut(100).Then().FadeIn();
            }
            else
            {
                IconContainer.FadeColour(colours.Red, 100).Then().FadeColour(Colour4.White);
            }
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (Enabled.Value)
                ToggleClickEffect();

            return base.OnClick(e);
        }
    }
}
