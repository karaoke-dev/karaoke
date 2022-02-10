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
        public void TestCreateNotes(string[] timeTags, string[] expected)
        {
            var generator = new NoteGenerator(new NoteGeneratorConfig());
            var lyric = new Lyric
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            };
            var notes = generator.CreateNotes(lyric);

            string[] actual = notes.Select(x => x.Text).ToArray();
            Assert.AreEqual(expected, actual);
        }
    }
}
