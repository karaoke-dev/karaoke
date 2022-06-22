// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.UI
{
    [TestFixture]
    public class TestSceneScoringStatus : OsuTestScene
    {
        [TestCase(ScoringStatusMode.AndroidMicrophonePermissionDeclined)]
        [TestCase(ScoringStatusMode.AndroidDoesNotSupported)]
        [TestCase(ScoringStatusMode.IOSMicrophonePermissionDeclined)]
        [TestCase(ScoringStatusMode.IOSDoesNotSupported)]
        [TestCase(ScoringStatusMode.OSXMicrophonePermissionDeclined)]
        [TestCase(ScoringStatusMode.OSXDoesNotSupported)]
        [TestCase(ScoringStatusMode.WindowsMicrophonePermissionDeclined)]
        [TestCase(ScoringStatusMode.NoMicrophoneDevice)]
        [TestCase(ScoringStatusMode.NotScoring)]
        [TestCase(ScoringStatusMode.AutoPlay)]
        [TestCase(ScoringStatusMode.Edit)]
        [TestCase(ScoringStatusMode.Scoring)]
        [TestCase(ScoringStatusMode.NotInitialized)]
        public void TestMode(ScoringStatusMode mode)
        {
            AddStep("create mod display", () =>
            {
                Child = new ScoringStatus(mode)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                };
            });
        }
    }
}
