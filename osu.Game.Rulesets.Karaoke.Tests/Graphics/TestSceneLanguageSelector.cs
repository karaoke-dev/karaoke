// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Globalization;
using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    public class TestSceneLanguageSelector : OsuManualInputManagerTestScene
    {
        [Test]
        public void TestAllFiles()
        {
            AddStep("create", () =>
            {
                var language = new Bindable<CultureInfo>(new CultureInfo("ja"));
                Child = new LanguageSelector
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.8f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Current = language
                };
            });
        }
    }
}
