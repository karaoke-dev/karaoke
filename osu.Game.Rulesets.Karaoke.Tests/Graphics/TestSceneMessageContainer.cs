// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    [TestFixture]
    public class TestSceneMessageContainer : OsuTestScene
    {
        private MessageContainer messageContainer;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = messageContainer = new MessageContainer
            {
                Width = 300,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
        });

        [Test]
        public void TestDisplayMessage()
        {
            AddStep("Display success message", () =>
            {
                messageContainer.AddSuccessParagraph("Success message");
            });

            AddStep("Display warning message", () =>
            {
                messageContainer.AddWarningParagraph("Warning message");
            });

            AddStep("Display alert message", () =>
            {
                messageContainer.AddAlertParagraph("Alert message");
            });
        }

        public void TestDisplayMultiPessage()
        {
            AddStep("Display multi message", () =>
            {
                messageContainer.AddSuccessParagraph("Success message");
                messageContainer.AddWarningParagraph("Warning message");
                messageContainer.AddAlertParagraph("Alert message");
            });
        }
    }
}
