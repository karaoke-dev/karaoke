// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.Language;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.Notes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.RomajiTags.Ja;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.RubyTags.Ja;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Configs.Generator.Lyrics.TimeTags.Zh;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap
{
    [TestFixture]
    public partial class TestSceneGeneratorConfigDialog : OsuTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            var config = new KaraokeRulesetEditGeneratorConfigManager();
            Dependencies.Cache(config);
        }

        [TestCase(typeof(LanguageDetectorConfigDialog), TestName = nameof(LanguageDetectorConfigDialog))]
        [TestCase(typeof(NoteGeneratorConfigDialog), TestName = nameof(NoteGeneratorConfigDialog))]
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
                    Child?.Show();
                });
            });
        }
    }
}
