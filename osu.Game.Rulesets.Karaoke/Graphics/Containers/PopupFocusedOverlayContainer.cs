// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Graphics.Containers
{
    public abstract class PopupFocusedOverlayContainer : OsuFocusedOverlayContainer
    {
        private const double enter_duration = 500;
        private const double exit_duration = 200;

        protected PopupFocusedOverlayContainer()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override void PopIn()
        {
            base.PopIn();

            this.FadeIn(enter_duration, Easing.OutQuint);
            this.ScaleTo(0.9f).Then().ScaleTo(1f, enter_duration, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();

            this.FadeOut(exit_duration, Easing.OutQuint);
            this.ScaleTo(0.9f, exit_duration);

            // Ensure that textboxes commit
            GetContainingInputManager()?.TriggerFocusContention(this);
        }
    }
}
