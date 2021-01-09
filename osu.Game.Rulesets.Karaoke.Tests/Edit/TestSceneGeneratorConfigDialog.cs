// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Layouts;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.TimeTags.Zh;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneGeneratorConfigDialog : OsuTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var config = new KaraokeRulesetEditGeneratorConfigManager();
            Dependencies.Cache(config);
        }

        [TestCase(typeof(LanguageDetectorConfigDialog), TestName = nameof(LanguageDetectorConfigDialog))]
        [TestCase(typeof(LayoutGeneratorConfigDialog), TestName = nameof(LayoutGeneratorConfigDialog))]
        [TestCase(typeof(JaRomajiTagGeneratorConfigDialog), TestName = nameof(JaRomajiTagGeneratorConfigDialog))]
        [TestCase(typeof(JaRubyTagGeneratorConfigDialog), TestName = nameof(JaRubyTagGeneratorConfigDialog))]
        [TestCase(typeof(JaTimeTagGeneratorConfigDialog), TestName = nameof(JaTimeTagGeneratorConfigDialog))]
        [TestCase(typeof(ZhTimeTagGeneratorConfigDialog), TestName = nameof(ZhTimeTagGeneratorConfigDialog))]
        public void TestGenerate(Type configType)
        {
            AddStep("Show dialog", () =>
            {
                Schedule(() =>
                {
                    Child = Activator.CreateInstance(configType) as Drawable;
                    Child.Show();
                });
            });
        }
    }
}
