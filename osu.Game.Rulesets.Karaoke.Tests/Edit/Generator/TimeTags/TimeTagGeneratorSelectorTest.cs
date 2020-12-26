// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.TimeTags
{
    public class TimeTagGeneratorSelectorTest
    {
        [TestCase(17, "か", new[] { "[0,start]:" })] // Japanese
        [TestCase(1041, "か", new[] { "[0,start]:" })] // Japanese
        [TestCase(1028, "喵", new[] { "[0,start]:" })] // Chinses
        [TestCase(3081, "hello", null)] // English
        public void TestCreateTimeTag(int lcid, string text, string[] actualTimeTag)
        {
            var lyric = new Lyric
            {
                Language = new CultureInfo(lcid),
                Text = text,
            };
            var selector = new TimeTagGeneratorSelector();
            var generatedTimeTags = selector.GenerateTimeTags(lyric);
            TimeTagAssert.AreEqual(generatedTimeTags, TestCaseTagHelper.ParseTimeTags(actualTimeTag));
        }
    }
}
