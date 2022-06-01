// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Notes
{
    [TestFixture]
    public class NoteGeneratorTest
    {
        [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new[] { "カ", "ラ", "オ", "ケ" })]
        [TestCase(new[] { "[3,end]:1000", "[3,start]:2000", "[2,start]:3000", "[1,start]:4000", "[0,start]:5000" }, new string[] { })]
        [TestCase(new[] { "[0,start]:1000", "[1,start]:1000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new[] { "カラ", "オ", "ケ" })] // will combine the note if time is duplicated.
        [TestCase(new[] { "[0,start]:1000", "[1,start]:", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new[] { "カラ", "オ", "ケ" })] // will combine the note if got no time.
        [TestCase(new[] { "[0,start]:", "[1,start]:1000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new[] { "ラ", "オ", "ケ" })]
        [TestCase(new[] { "[0,start]:1000" }, new string[] { })]
        [TestCase(new[] { "[0,start]:" }, new string[] { })]
        public void TestGenerate(string[] timeTags, string[] expected)
        {
            var generator = new NoteGenerator(new NoteGeneratorConfig());
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            };
            var notes = generator.Generate(lyric);

            string[] actual = notes.Select(x => x.Text).ToArray();
            Assert.AreEqual(expected, actual);
        }
    }
}
