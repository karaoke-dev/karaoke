// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.Notes;

[TestFixture]
public class NoteGeneratorTest : BaseLyricGeneratorTest<NoteGenerator, Note[], NoteGeneratorConfig>
{
    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, true)]
    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000" }, true)]
    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]" }, false)] // all time-tag should with time.
    [TestCase(new[] { "[0,start]:1000", "[1,start]" }, false)] // should have at least two time-tags with time.
    [TestCase(new[] { "[0,start]:1000" }, false)] // should have at least two time-tags.
    [TestCase(new[] { "[0,start]" }, false)] // no-time.
    [TestCase(new string[] { }, false)]
    public void TestCanGenerate(string[] timeTags, bool canGenerate)
    {
        var config = GeneratorEmptyConfig();
        var lyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        CheckCanGenerate(lyric, canGenerate, config);
    }

    [TestCase(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new[] { "カ", "ラ", "オ", "ケ" })]
    [TestCase(new[] { "[3,end]:1000", "[3,start]:2000", "[2,start]:3000", "[1,start]:4000", "[0,start]:5000" }, new string[] { })]
    [TestCase(new[] { "[0,start]:1000", "[1,start]:1000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }, new[] { "カラ", "オ", "ケ" })] // will combine the note if time is duplicated.
    public void TestGenerate(string[] timeTags, string[] expectedNoteTexts)
    {
        var config = GeneratorEmptyConfig();
        var lyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };

        var expectedNotes = expectedNoteTexts.Select(x => new Note { Text = x }).ToArray();
        CheckGenerateResult(lyric, expectedNotes, config);
    }

    [TestCase(new string[] { }, new[] { "カ", "ラ", "オ", "ケ" })]
    [TestCase(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, new[] { "カ,か", "ラ,ら", "オ,お", "ケ,け" })]
    public void TestGenerateWithRuby(string[] rubyTags, string[] expectedNoteTexts)
    {
        var config = GeneratorEmptyConfig();
        var lyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }),
            RubyTags = TestCaseTagHelper.ParseRubyTags(rubyTags),
        };

        var expectedNotes = expectedNoteTexts.Select(x =>
        {
            if (x.Contains(','))
            {
                return new Note
                {
                    Text = x.Split(',')[0],
                    RubyText = x.Split(',')[1],
                };
            }

            return new Note
            {
                Text = x,
            };
        }).ToArray();
        CheckGenerateResult(lyric, expectedNotes, config);
    }

    protected override void AssertEqual(Note[] expected, Note[] actual)
    {
        Assert.That(actual.Select(x => x.Text), Is.EqualTo(expected.Select(x => x.Text)));
        Assert.That(actual.Select(x => x.RubyText), Is.EqualTo(expected.Select(x => x.RubyText)));
    }
}
