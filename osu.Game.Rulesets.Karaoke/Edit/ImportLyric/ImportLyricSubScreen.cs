// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Graphics.Overlays.Dialog;
using osu.Game.Screens;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class ImportLyricSubScreen : OsuScreen, IImportLyricSubScreen
    {
        public const float X_SHIFT = 200;
        public const double X_MOVE_DURATION = 800;
        public const double RESUME_TRANSITION_DELAY = DISAPPEAR_DURATION / 2;
        public const double APPEAR_DURATION = 800;
        public const double DISAPPEAR_DURATION = 500;

        [Resolved]
        protected ImportLyricSubScreenStack ScreenStack { get; private set; }

        [Resolved]
        protected DialogOverlay DialogOverlay { get; private set; }

        public abstract string ShortTitle { get; }

        public abstract ImportLyricStep Step { get; }

        public abstract IconUsage Icon { get; }

        public ImportLyricSubScreen()
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

        public override void OnEntering(IScreen last)
        {
            this.FadeInFromZero(APPEAR_DURATION, Easing.OutQuint);
            this.FadeInFromZero(APPEAR_DURATION, Easing.OutQuint);
            this.MoveToX(X_SHIFT).MoveToX(0, X_MOVE_DURATION, Easing.OutQuint);
        }

        public override bool OnExiting(IScreen next)
        {
            this.FadeOut(DISAPPEAR_DURATION, Easing.OutQuint);
            this.MoveToX(X_SHIFT, X_MOVE_DURATION, Easing.OutQuint);

            return false;
        }

        public override void OnResuming(IScreen last)
        {
            this.Delay(RESUME_TRANSITION_DELAY).FadeIn(APPEAR_DURATION, Easing.OutQuint);
            this.MoveToX(0, X_MOVE_DURATION, Easing.OutQuint);
        }

        public override void OnSuspending(IScreen next)
        {
            this.FadeOut(DISAPPEAR_DURATION, Easing.OutQuint);
            this.MoveToX(-X_SHIFT, X_MOVE_DURATION, Easing.OutQuint);
        }

        public abstract void Complete();

        public virtual void CanRollBack(IImportLyricSubScreen rollBackScreen, Action<bool> callBack)
        {
            DialogOverlay.Push(new OkPopupDialog(callBack)
            {
                Icon = rollBackScreen.Icon,
                HeaderText = rollBackScreen.ShortTitle,
                BodyText = $"Will roll-back to step '{rollBackScreen.Title}'",
            });
        }

        public override string ToString() => Title;
    }
}
