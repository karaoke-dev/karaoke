// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.RubyTags
{
    public class RubyTagGeneratorSelectorTest
    {
        [TestCase(17, "花火大会", new[] { "[0,2]:はなび", "[2,4]:たいかい" })] // Japanese
        [TestCase(1041, "はなび", new string[] { })] // Japanese
        [TestCase(1028, "はなび", null)] // Chinese(should not supported)
        public void TestCreateRubyTag(int lcid, string text, string[] actualRuby)
        {
            var lyric = new Lyric
            {
                Language = new CultureInfo(lcid),
                Text = text,
            };
            var selector = new RubyTagGeneratorSelector();
            var generatedRuby = selector.GenerateRubyTags(lyric);
            Assert.AreEqual(generatedRuby, TestCaseTagHelper.ParseRubyTags(actualRuby));
        }
    }
}
