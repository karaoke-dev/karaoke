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

        [TestCase(typeof(LanguageDetectorConfigDialog))]
        [TestCase(typeof(LayoutGeneratorConfigDialog))]
        [TestCase(typeof(JaRomajiTagGeneratorConfigDialog))]
        [TestCase(typeof(JaRubyTagGeneratorConfigDialog))]
        [TestCase(typeof(JaTimeTagGeneratorConfigDialog))]
        [TestCase(typeof(ZhTimeTagGeneratorConfigDialog))]
        public void TestGenerate(Type configType)
        {
            Child = Activator.CreateInstance(configType) as Drawable;
        }
    }
}
