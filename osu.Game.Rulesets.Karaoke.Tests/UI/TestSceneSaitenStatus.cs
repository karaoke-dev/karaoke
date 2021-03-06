﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.UI
{
    [TestFixture]
    public class TestSceneSaitenStatus : OsuTestScene
    {
        [TestCase(SaitenStatusMode.AndroidMicrophonePermissionDeclined)]
        [TestCase(SaitenStatusMode.AndroidDoesNotSupported)]
        [TestCase(SaitenStatusMode.IOSMicrophonePermissionDeclined)]
        [TestCase(SaitenStatusMode.IOSDoesNotSupported)]
        [TestCase(SaitenStatusMode.OSXMicrophonePermissionDeclined)]
        [TestCase(SaitenStatusMode.OSXDoesNotSupported)]
        [TestCase(SaitenStatusMode.WindowsMicrophonePermissionDeclined)]
        [TestCase(SaitenStatusMode.NoMicrophoneDevice)]
        [TestCase(SaitenStatusMode.NotSaitening)]
        [TestCase(SaitenStatusMode.AutoPlay)]
        [TestCase(SaitenStatusMode.Edit)]
        [TestCase(SaitenStatusMode.Saitening)]
        [TestCase(SaitenStatusMode.NotInitialized)]
        public void TestMode(SaitenStatusMode mode)
        {
            AddStep("create mod display", () =>
            {
                Child = new SaitenStatus(mode)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                };
            });
        }
    }
}
