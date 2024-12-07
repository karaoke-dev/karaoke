// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Beatmaps;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckStageInfo<osu.Game.Rulesets.Karaoke.Stages.Infos.Classic.ClassicStageInfo>;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

[Ignore("Disable this test until able to get the stage info from the resource file.")]
public class CheckStageInfoTest : BaseCheckTest<CheckStageInfoTest.CheckStageInfo>
{
    [Test]
    public void TestCheckNoElement()
    {
        var beatmap = createTestingBeatmap(Array.Empty<Lyric>());
        var stageInfo = createTestingStageInfo();
        AssertNotOk<IssueTemplateNoElement>(getContext(beatmap, stageInfo));
    }

    [Test]
    public void TestCheckMappingHitObjectNotExist()
    {
        var lyric = new Lyric();

        // note that this lyric does not added in to the beatmap.
        var beatmap = createTestingBeatmap(Array.Empty<Lyric>());
        var stageInfo = createTestingStageInfo(category =>
        {
            // add two elements to prevent no element error.
            category.AddElement();
            category.AddElement();

            var firstElement = category.AvailableElements.First();
            category.AddToMapping(firstElement, lyric);
        });
        AssertNotOk<IssueTemplateMappingHitObjectNotExist>(getContext(beatmap, stageInfo));
    }

    [Test]
    public void TestCheckMappingItemNotExist()
    {
        var lyric = new Lyric();

        var beatmap = createTestingBeatmap(new[] { lyric });
        var stageInfo = createTestingStageInfo(category =>
        {
            // add two elements to prevent no element error.
            category.AddElement();
            category.AddElement();

            // write value to the mapping directly to reproduce the behavior like loading value from the beatmap.
            category.Mappings.Add(lyric.ID, ElementId.NewElementId());
        });
        AssertNotOk<IssueTemplateMappingItemNotExist>(getContext(beatmap, stageInfo));
    }

    public class CheckStageInfo : CheckStageInfo<ClassicStageInfo>
    {
        protected override string Description => "Checks for testing the shared logic";

        public CheckStageInfo()
        {
            // Note that we only test the lyric layout category.
            RegisterCategory(x => x.StyleCategory, 0);
            RegisterCategory(x => x.LyricLayoutCategory, 2);
        }

        public override IEnumerable<IssueTemplate> CustomTemplates => Array.Empty<IssueTemplate>();

        public override IEnumerable<Issue> CheckStageInfoWithHitObjects(ClassicStageInfo stageInfo, IReadOnlyList<KaraokeHitObject> hitObjects)
        {
            yield break;
        }

        protected override IEnumerable<Issue> CheckElement<TStageElement>(TStageElement element)
        {
            yield break;
        }
    }

    private static IBeatmap createTestingBeatmap(IEnumerable<Lyric>? lyrics)
    {
        var karaokeBeatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
            HitObjects = lyrics?.OfType<KaraokeHitObject>().ToList() ?? new List<KaraokeHitObject>(),
        };
        return new EditorBeatmap(karaokeBeatmap);
    }

    private static StageInfo createTestingStageInfo(Action<ClassicLyricLayoutCategory>? editStageAction = null)
    {
        var stageInfo = new ClassicStageInfo();
        editStageAction?.Invoke(stageInfo.LyricLayoutCategory);

        return stageInfo;
    }

    private static BeatmapVerifierContext getContext(IBeatmap beatmap, StageInfo stageInfo)
        => new(beatmap, new TestWorkingBeatmap(beatmap));
}
