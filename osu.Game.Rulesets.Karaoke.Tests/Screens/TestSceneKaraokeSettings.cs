// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Screens.Settings;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    [TestFixture]
    public class TestSceneKaraokeSettings : ScreenTestScene
    {
        public TestSceneKaraokeSettings()
        {
            var screen = new KaraokeSettings();
            AddStep("show", () => LoadScreen(screen));
            AddUntilStep("wait for loaded", () => screen.IsLoaded);
        }
    }
}
