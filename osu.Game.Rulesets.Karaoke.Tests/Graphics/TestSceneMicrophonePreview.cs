// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    [TestFixture]
    public class TestSceneMicrophonePreview : OsuTestScene
    {
        private MicrophoneSoundVisualizer preview;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = new MicrophoneInputManager(-1)
            {
                Child = preview = new MicrophoneSoundVisualizer
                {
                    Width = 300,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            };
        });
    }
}
