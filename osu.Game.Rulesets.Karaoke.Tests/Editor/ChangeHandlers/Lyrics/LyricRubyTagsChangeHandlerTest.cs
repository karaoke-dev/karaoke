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
    public class LyricRubyTagsChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricRubyTagsChangeHandler, Lyric>
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
                Text = "風",
                Language = new CultureInfo(17)
            });

            TriggerHandlerChanged(c => c.AutoGenerate());

            AssertSelectedHitObject(h =>
            {
                var rubyTags = h.RubyTags;
                Assert.AreEqual(1, rubyTags.Count);
                Assert.AreEqual("かぜ", rubyTags[0].Text);
            });
        }

        [Test]
        public void TestAutoGenerateNonSupportedLyric()
        {
            PrepareHitObjects(new[]
            {
                new Lyric
                {
                    Text = "風",
                },
                new Lyric
                {
                    Text = "",
                },
                new Lyric
                {
                    Text = null,
                },
                new Lyric
                {
                    Text = "",
                    Language = new CultureInfo(17)
                },
                new Lyric
                {
                    Text = null,
                    Language = new CultureInfo(17)
                }
            });

            TriggerHandlerChanged(c => c.AutoGenerate());

            AssertSelectedHitObject(h =>
            {
                // should not able to generate time-tag if lyric text is empty, or did not have language.
                Assert.IsEmpty(h.RubyTags);
            });
        }
    }
}
