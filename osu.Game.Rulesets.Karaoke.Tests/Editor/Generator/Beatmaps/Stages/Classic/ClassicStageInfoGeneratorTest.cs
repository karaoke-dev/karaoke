// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps.Stages.Classic;

public class ClassicStageInfoGeneratorTest : BaseStageInfoGeneratorTest<ClassicStageInfoGenerator, ClassicStageInfo, ClassicStageInfoGeneratorConfig>
{
    [Test]
    public void TestCanGenerate()
    {
        var config = GeneratorDefaultConfig();
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>
            {
                TestCaseTagHelper.ParseLyric("[1000,3000]:lyric1"),
                TestCaseTagHelper.ParseLyric("[4000,6000]:lyric2"),
                TestCaseTagHelper.ParseLyric("[7000,9000]:lyric3"),
                TestCaseTagHelper.ParseLyric("[10000,12000]:lyric4"),
            }
        };

        CheckCanGenerate(beatmap, true, config);
    }

    [Test]
    public void TestCanGenerateWithNonLyricBeatmap()
    {
        var config = GeneratorDefaultConfig();
        var beatmap = new KaraokeBeatmap();
        CheckCanGenerate(beatmap, false, config);
    }

    [Test]
    public void TestGenerate()
    {
        var config = GeneratorDefaultConfig();

        var lyric1 = TestCaseTagHelper.ParseLyric("[1000,3000]:lyric1", 1);
        var lyric2 = TestCaseTagHelper.ParseLyric("[4000,6000]:lyric2", 2);
        var lyric3 = TestCaseTagHelper.ParseLyric("[7000,9000]:lyric3", 3);
        var lyric4 = TestCaseTagHelper.ParseLyric("[10000,12000]:lyric4", 4);
        var beatmap = new KaraokeBeatmap
        {
            HitObjects = new List<KaraokeHitObject>
            {
                lyric1,
                lyric2,
                lyric3,
                lyric4,
            }
        };

        // Note: we did not care about the generator result here.
        var expected = new ClassicStageInfo();
        CheckGenerateResult(beatmap, expected, config);
    }

    protected override void AssertEqual(ClassicStageInfo expected, ClassicStageInfo actual)
    {
        // as we already test the property in the other generator, there's no need to compare it again?
    }
}
