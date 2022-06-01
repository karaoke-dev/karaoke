// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.TimeTags.Zh
{
    [TestFixture]
    public class ZhTimeTagGeneratorTest : BaseTimeTagGeneratorTest<ZhTimeTagGenerator, ZhTimeTagGeneratorConfig>
    {
        [TestCase("測試一些歌詞", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[3,start]:", "[4,start]:", "[5,start]:", "[5,end]:" })]
        [TestCase("拉拉拉~~~", new[] { "[0,start]:", "[1,start]:", "[2,start]:", "[5,end]:" })]
        [TestCase("拉~拉~拉~", new[] { "[0,start]:", "[2,start]:", "[4,start]:", "[5,end]:" })]
        public void TestGenerateWithCheckLineEndKeyUp(string lyric, string[] expectedTimeTags)
        {
            var config = GeneratorConfig(nameof(ZhTimeTagGeneratorConfig.CheckLineEndKeyUp));
            RunTimeTagCheckTest(lyric, expectedTimeTags, config);
        }
    }
}
