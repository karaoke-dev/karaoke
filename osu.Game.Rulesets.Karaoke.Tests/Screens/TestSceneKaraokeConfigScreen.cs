// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Screens.Config;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    [TestFixture]
    public class TestSceneKaraokeConfigScreen : ScreenTestScene
    {
        public TestSceneKaraokeConfigScreen()
        {
            var screen = new KaraokeConfigScreen();
            AddStep("show", () => LoadScreen(screen));
            AddUntilStep("wait for loaded", () => screen.IsLoaded);
        }
    }
}
