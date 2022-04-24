// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class LyricImporterStepScreen : OsuScreen, ILyricImporterStepScreen
    {
        public const float X_SHIFT = 200;
        public const double X_MOVE_DURATION = 800;
        public const double RESUME_TRANSITION_DELAY = DISAPPEAR_DURATION / 2;
        public const double APPEAR_DURATION = 800;
        public const double DISAPPEAR_DURATION = 500;

        [Resolved]
        protected LyricImporterSubScreenStack ScreenStack { get; private set; }

        [Resolved]
        protected IDialogOverlay DialogOverlay { get; private set; }

        public abstract string ShortTitle { get; }

        public abstract LyricImporterStep Step { get; }

        public abstract IconUsage Icon { get; }

        protected LyricImporterStepScreen()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new OsuButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = 240,
                    Text = $"{Title}, Click to next step.",
                    Action = Complete
                }
            };
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            this.FadeInFromZero(APPEAR_DURATION, Easing.OutQuint);
        }

        public override bool OnExiting(ScreenExitEvent e)
        {
            base.OnExiting(e);
            this.FadeOut(DISAPPEAR_DURATION, Easing.OutQuint);
            return false;
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            base.OnResuming(e);
            this.FadeIn(APPEAR_DURATION, Easing.OutQuint);
        }

        public override void OnSuspending(ScreenTransitionEvent e)
        {
            base.OnSuspending(e);
            this.FadeOut(DISAPPEAR_DURATION, Easing.OutQuint);
        }

        public abstract void Complete();

        public virtual void ConfirmRollBackFromStep(ILyricImporterStepScreen fromScreen, Action<bool> callBack)
        {
            DialogOverlay.Push(new RollBackPopupDialog(fromScreen, ok =>
            {
                callBack?.Invoke(ok);
            }));
        }

        public override string ToString() => Title;
    }
}
