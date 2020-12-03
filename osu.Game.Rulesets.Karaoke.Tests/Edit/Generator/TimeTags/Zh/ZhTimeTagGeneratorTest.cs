// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.TimeTags.Zh
{
    [TestFixture]
    public class ZhTimeTagGeneratorTest : BaseTimeTagGeneratorTest<ZhTimeTagGenerator, ZhTimeTagGeneratorConfig>
    {
        [TestCase("測試一些歌詞", new double[] { 0, 1, 2, 3, 4, 5, 5.5 })]
        [TestCase("拉拉拉~~~", new double[] { 0, 1, 2, 5.5 })]
        [TestCase("拉~拉~拉~", new double[] { 0, 2, 4, 5.5 })]
        public void TestLyricWithCheckLineEndKeyUp(string lyric, double[] index)
        {
            var config = GeneratorConfig(nameof(ZhTimeTagGeneratorConfig.CheckLineEndKeyUp));
            RunTimeTagCheckTest(lyric, index, config);
        }
    }
}
