// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Tests.Visual;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    [TestFixture]
    public class TestSceneMessageContainer : OsuTestScene
    {
        private MessageContainer messageContainer = null!;

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

        [Test]
        public void TestDisplayMultiMessage()
        {
            AddStep("Display multi message", () =>
            {
                messageContainer.AddSuccessParagraph("Success message");
                messageContainer.AddWarningParagraph("Warning message");
                messageContainer.AddAlertParagraph("Alert message");
                messageContainer.AddHighlightText("I'm highlighting.");
            });
        }

        [Test]
        public void TestDisplayMessageWithPostfix()
        {
            AddStep("Display multi message", () =>
            {
                messageContainer.AddSuccessParagraph("Success message");
                messageContainer.AddText(" with postfix", s => s.Colour = Color4.Blue);
                messageContainer.AddWarningParagraph("Warning message");
                messageContainer.AddText(" with postfix", s => s.Colour = Color4.Yellow);
                messageContainer.AddAlertParagraph("Alert message");
                messageContainer.AddText(" with postfix", s => s.Colour = Color4.Red);
            });
        }
    }
}
