// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.TimeTags
{
    public class TimeTagGeneratorSelectorTest : BaseGeneratorSelectorTest<TimeTagGeneratorSelector>
    {
        [TestCase(17, "か", new[] { "[0,start]:", "[0,end]:" })] // Japanese
        [TestCase(1041, "か", new[] { "[0,start]:", "[0,end]:" })] // Japanese
        [TestCase(1028, "喵", new[] { "[0,start]:" })] // Chinese
        [TestCase(3081, "hello", null)] // English
        public void TestGenerate(int lcid, string text, string[] expectedTimeTags)
        {
            var selector = CreateSelector();
            var lyric = new Lyric
            {
                Language = new CultureInfo(lcid),
                Text = text,
            };

            var expected = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            var actual = selector.Generate(lyric);
            TimeTagAssert.ArePropertyEqual(expected, actual);
        }
    }
}
