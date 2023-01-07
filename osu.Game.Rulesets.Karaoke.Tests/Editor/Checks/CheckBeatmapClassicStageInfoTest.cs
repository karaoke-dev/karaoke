// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Beatmaps;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckBeatmapClassicStageInfo;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

public class CheckBeatmapClassicStageInfoTest : BeatmapPropertyCheckTest<CheckBeatmapClassicStageInfo>
{
    [Test]
    public void TestCheckInvalidRowHeight()
    {
        var beatmap = createTestingBeatmap(Array.Empty<Lyric>(), stage =>
        {
            stage.LyricLayoutDefinition.LineHeight = MIN_ROW_HEIGHT - 1;
        });
        AssertNotOk<IssueTemplateInvalidRowHeight>(getContext(beatmap));

        var beatmap2 = createTestingBeatmap(Array.Empty<Lyric>(), stage =>
        {
            stage.LyricLayoutDefinition.LineHeight = MAX_ROW_HEIGHT + 1;
        });
        AssertNotOk<IssueTemplateInvalidRowHeight>(getContext(beatmap2));
    }

    [Test]
    public void TestCheckLyricLayoutInvalidLineNumber()
    {
        var beatmap = createTestingBeatmap(Array.Empty<Lyric>(), stage =>
        {
            var layoutElement = stage.LyricLayoutCategory.AvailableElements.First();
            layoutElement.Line = MIN_LINE_SIZE - 1;
        });
        AssertNotOk<IssueTemplateLyricLayoutInvalidLineNumber>(getContext(beatmap));

        var beatmap2 = createTestingBeatmap(Array.Empty<Lyric>(), stage =>
        {
            var layoutElement = stage.LyricLayoutCategory.AvailableElements.First();
            layoutElement.Line = MAX_LINE_SIZE + 1;
        });
        AssertNotOk<IssueTemplateLyricLayoutInvalidLineNumber>(getContext(beatmap2));
    }

    private static IBeatmap createTestingBeatmap(IEnumerable<Lyric>? lyrics, Action<ClassicStageInfo>? editStageAction = null)
    {
        var stageInfo = new ClassicStageInfo();

        // add two elements to prevent no element error.
        stageInfo.LyricLayoutCategory.AddElement(x => x.Line = MIN_LINE_SIZE);
        stageInfo.LyricLayoutCategory.AddElement(x => x.Line = MIN_LINE_SIZE);
        stageInfo.LyricLayoutDefinition.LineHeight = MIN_ROW_HEIGHT;

        editStageAction?.Invoke(stageInfo);

        var karaokeBeatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
            StageInfos = new List<StageInfo>
            {
                stageInfo
            },
            HitObjects = lyrics?.OfType<KaraokeHitObject>().ToList() ?? new List<KaraokeHitObject>()
        };
        return new EditorBeatmap(karaokeBeatmap);
    }

    private static BeatmapVerifierContext getContext(IBeatmap beatmap)
        => new(beatmap, new TestWorkingBeatmap(beatmap));
}
