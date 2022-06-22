// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Screens.Settings;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens
{
    [TestFixture]
    public class TestSceneKaraokeSettings : ScreenTestScene<KaraokeSettings>
    {
        protected override KaraokeSettings CreateScreen() => new();
    }
}
