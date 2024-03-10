// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics.TimeTags.Ja;

[TestFixture]
public class JaTimeTagGeneratorTest : BaseTimeTagGeneratorTest<JaTimeTagGenerator, JaTimeTagGeneratorConfig>
{
    [TestCase("花火大会", true)]
    [TestCase("！", true)]
    [TestCase("   ", true)]
    [TestCase("", false)] // will not able to generate the romanisation if lyric is empty.
    [TestCase(null, false)]
    public void TestCanGenerate(string text, bool canGenerate)
    {
        var config = GeneratorEmptyConfig();
        CheckCanGenerate(text, canGenerate, config);
    }

    [TestCase("がんばって", new[] { "[0,start]", "[2,start]", "[4,start]" }, false)]
    [TestCase("がんばって", new[] { "[0,start]", "[1,start]", "[2,start]", "[4,start]" }, true)]
    public void TestGenerateWithCheckWhiteCheckん(string lyric, string[] expectedTimeTags, bool applyConfig)
    {
        var config = GeneratorEmptyConfig(x => x.Checkん.Value = applyConfig);
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }

    [TestCase("買って", new[] { "[0,start]", "[2,start]" }, false)]
    [TestCase("買って", new[] { "[0,start]", "[1,start]", "[2,start]" }, true)]
    public void TestGenerateWithCheckっ(string lyric, string[] expectedTimeTags, bool applyConfig)
    {
        var config = GeneratorEmptyConfig(x => x.Checkっ.Value = applyConfig);
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }

    [TestCase(" ", new string[] { }, false)]
    [TestCase(" ", new[] { "[0,start]" }, true)]
    public void TestGenerateWithCheckBlankLine(string lyric, string[] expectedTimeTags, bool applyConfig)
    {
        var config = GeneratorEmptyConfig(x => x.CheckBlankLine.Value = applyConfig);
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }

    [TestCase("か", new[] { "[0,start]" }, false)]
    [TestCase("か", new[] { "[0,start]", "[0,end]" }, true)]
    public void TestGenerateWithCheckLineEndKeyUp(string lyric, string[] expectedTimeTags, bool applyConfig)
    {
        var config = GeneratorEmptyConfig(x => x.CheckLineEndKeyUp.Value = applyConfig);
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }

    [TestCase("か     ", new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]", "[4,start]", "[5,start]" }, false)]
    [TestCase("か     ", new[] { "[0,start]", "[1,start]" }, true)]
    public void TestGenerateWithCheckWhiteSpace(string lyric, string[] expectedTimeTags, bool applyConfig)
    {
        var config = GeneratorEmptyConfig(x => x.CheckWhiteSpace.Value = applyConfig);
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }

    [TestCase("か ", new[] { "[0,start]", "[1,start]" }, false)]
    [TestCase("か ", new[] { "[0,start]", "[0,end]" }, true)]
    public void TestGenerateWithCheckWhiteSpaceKeyUp(string lyric, string[] expectedTimeTags, bool applyConfig)
    {
        var config = GeneratorEmptyConfig(x =>
        {
            x.CheckWhiteSpace.Value = true;
            x.CheckWhiteSpaceKeyUp.Value = applyConfig;
        });
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }

    [TestCase("a　b　c", new[] { "[0,start]", "[2,start]", "[4,start]" }, false, false)]
    [TestCase("a　b　c", new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]", "[4,start]" }, true, false)]
    [TestCase("a　b　c", new[] { "[0,start]", "[0,end]", "[2,start]", "[2,end]", "[4,start]" }, true, true)]
    [TestCase("Ａ　Ｂ　Ｃ", new[] { "[0,start]", "[2,start]", "[4,start]" }, false, false)]
    [TestCase("Ａ　Ｂ　Ｃ", new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]", "[4,start]" }, true, false)]
    [TestCase("Ａ　Ｂ　Ｃ", new[] { "[0,start]", "[0,end]", "[2,start]", "[2,end]", "[4,start]" }, true, true)]
    public void TestGenerateWithCheckWhiteSpaceAlphabet(string lyric, string[] expectedTimeTags, bool applyConfig, bool keyUp)
    {
        var config = GeneratorEmptyConfig(x =>
        {
            x.CheckWhiteSpace.Value = true;
            x.CheckWhiteSpaceAlphabet.Value = applyConfig;
            x.CheckWhiteSpaceKeyUp.Value = keyUp;
        });
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }

    [TestCase("0　1　2", new[] { "[0,start]", "[2,start]", "[4,start]" }, false, false)]
    [TestCase("0　1　2", new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]", "[4,start]" }, true, false)]
    [TestCase("0　1　2", new[] { "[0,start]", "[0,end]", "[2,start]", "[2,end]", "[4,start]" }, true, true)]
    [TestCase("０　１　２", new[] { "[0,start]", "[2,start]", "[4,start]" }, false, false)]
    [TestCase("０　１　２", new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]", "[4,start]" }, true, false)]
    [TestCase("０　１　２", new[] { "[0,start]", "[0,end]", "[2,start]", "[2,end]", "[4,start]" }, true, true)]
    public void TestGenerateWithCheckWhiteSpaceDigit(string lyric, string[] expectedTimeTags, bool applyConfig, bool keyUp)
    {
        var config = GeneratorEmptyConfig(x =>
        {
            x.CheckWhiteSpace.Value = true;
            x.CheckWhiteSpaceDigit.Value = applyConfig;
            x.CheckWhiteSpaceKeyUp.Value = keyUp;
        });
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }

    [TestCase("!　!　!", new[] { "[0,start]", "[2,start]", "[4,start]" }, false, false)]
    [TestCase("!　!　!", new[] { "[0,start]", "[1,start]", "[2,start]", "[3,start]", "[4,start]" }, true, false)]
    [TestCase("!　!　!", new[] { "[0,start]", "[0,end]", "[2,start]", "[2,end]", "[4,start]" }, true, true)]
    public void TestGenerateWitCheckWhiteSpaceAsciiSymbol(string lyric, string[] expectedTimeTags, bool applyConfig, bool keyUp)
    {
        var config = GeneratorEmptyConfig(x =>
        {
            x.CheckWhiteSpace.Value = true;
            x.CheckWhiteSpaceAsciiSymbol.Value = applyConfig;
            x.CheckWhiteSpaceKeyUp.Value = keyUp;
        });
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }

    [Test]
    public void TestGenerateWithRubyLyric()
    {
        var config = GeneratorEmptyConfig();
        var lyric = new Lyric
        {
            Text = "明日いっしょに遊びましょう！",
            RubyTags = new[]
            {
                new RubyTag
                {
                    StartIndex = 0,
                    EndIndex = 1,
                    Text = "あした",
                },
                new RubyTag
                {
                    StartIndex = 7,
                    EndIndex = 7,
                    Text = "あそ",
                },
            },
        };

        string[] expectedTimeTags =
        {
            "[0,start]",
            "[0,start]",
            "[0,start]",
            "[2,start]",
            "[4,start]",
            "[6,start]",
            "[7,start]",
            "[7,start]",
            "[8,start]",
            "[9,start]",
            "[10,start]",
            "[12,start]",
            "[13,start]",
        };
        CheckGenerateResult(lyric, expectedTimeTags, config);
    }
}
