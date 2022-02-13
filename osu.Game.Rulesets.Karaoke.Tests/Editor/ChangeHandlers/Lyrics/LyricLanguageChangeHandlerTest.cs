// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricLanguageChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricLanguageChangeHandler, Lyric>
    {
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var baseDependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            baseDependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());
            return baseDependencies;
        }

        [Test]
        public void TestAutoGenerateSupportedLyric()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ"
            });

            TriggerHandlerChanged(c => c.AutoGenerate());

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(new CultureInfo("ja"), h.Language);
            });
        }

        [Test]
        public void TestAutoGenerateNonSupportedLyric()
        {
            PrepareHitObject(new Lyric
            {
                Text = "???"
            });

            TriggerHandlerChanged(c => c.AutoGenerate());

            AssertSelectedHitObject(h =>
            {
                Assert.IsNull(h.Language);
            });
        }

        [Test]
        public void TestSetLanguageToJapanese()
        {
            var language = new CultureInfo("ja");
            PrepareHitObject(new Lyric());

            TriggerHandlerChanged(c => c.SetLanguage(language));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(language, h.Language);
            });
        }

        [Test]
        public void TestSetLanguageToNull()
        {
            PrepareHitObject(new Lyric
            {
                Text = "???"
            });

            TriggerHandlerChanged(c => c.SetLanguage(null));

            AssertSelectedHitObject(h =>
            {
                Assert.IsNull(h.Language);
            });
        }
    }
}
