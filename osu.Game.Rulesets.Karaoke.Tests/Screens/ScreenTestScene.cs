// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Screens;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    public abstract class ScreenTestScene<T> : ScreenTestScene where T : OsuScreen
    {
        protected T Screen { get; private set; } = null!;

        public override void SetUpSteps()
        {
            base.SetUpSteps();

            AddStep("load screen", LoadScreen);
            AddUntilStep("wait for loaded", () => Screen.IsLoaded);
        }

        protected virtual void LoadScreen()
        {
            LoadScreen(Screen = CreateScreen());
        }

        protected abstract T CreateScreen();
    }
}
