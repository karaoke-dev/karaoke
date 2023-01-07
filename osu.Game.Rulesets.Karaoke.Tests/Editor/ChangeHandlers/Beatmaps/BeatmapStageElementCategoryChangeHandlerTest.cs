// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public partial class BeatmapStageElementCategoryChangeHandlerTest : BaseChangeHandlerTest<BeatmapStageElementCategoryChangeHandlerTest.TestBeatmapStageElementCategoryChangeHandler>
{
    protected override TestBeatmapStageElementCategoryChangeHandler CreateChangeHandler()
        => new(x => x.OfType<ClassicStageInfo>().First().LyricLayoutCategory);

    [Test]
    public void TestAddElement()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.StageInfos.Add(new ClassicStageInfo());
        });

        TriggerHandlerChanged(c =>
        {
            c.AddElement(x =>
            {
                x.Name = "Element 1";
            });
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var category = getStageCategory(karaokeBeatmap);
            var firstElement = category.AvailableElements.First();

            Assert.AreEqual("Element 1", firstElement.Name);
        });
    }

    [Test]
    public void TestEditElement()
    {
        ClassicLyricLayout lyricLayout = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            stageInfo.LyricLayoutCategory.AddElement();
            karaokeBeatmap.StageInfos.Add(stageInfo);

            lyricLayout = stageInfo.LyricLayoutCategory.AvailableElements.First();
        });

        TriggerHandlerChanged(c =>
        {
            c.EditElement(lyricLayout.ID, x =>
            {
                x.Name = "Edit Element 1";
            });
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var category = getStageCategory(karaokeBeatmap);
            var firstElement = category.AvailableElements.First();

            Assert.AreEqual("Edit Element 1", firstElement.Name);
        });
    }

    [Test]
    public void TestRemoveElement()
    {
        ClassicLyricLayout lyricLayout = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            stageInfo.LyricLayoutCategory.AddElement();
            karaokeBeatmap.StageInfos.Add(stageInfo);

            lyricLayout = stageInfo.LyricLayoutCategory.AvailableElements.First();
        });

        TriggerHandlerChanged(c =>
        {
            c.RemoveElement(lyricLayout);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var category = getStageCategory(karaokeBeatmap);

            Assert.IsEmpty(category.AvailableElements);
        });
    }

    [Test]
    public void TestAddToMapping()
    {
        ClassicLyricLayout lyricLayout = null!;
        Lyric lyric = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            stageInfo.LyricLayoutCategory.AddElement();
            karaokeBeatmap.StageInfos.Add(stageInfo);

            lyricLayout = stageInfo.LyricLayoutCategory.AvailableElements.First();
        });

        PrepareHitObject(lyric = new Lyric());

        TriggerHandlerChanged(c =>
        {
            c.AddToMapping(lyricLayout, lyric);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var category = getStageCategory(karaokeBeatmap);

            Assert.IsNotEmpty(category.Mappings);
        });
    }

    [Test]
    public void TestRemoveFromMapping()
    {
        Lyric lyric = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            stageInfo.LyricLayoutCategory.AddElement();
            karaokeBeatmap.StageInfos.Add(stageInfo);
            karaokeBeatmap.HitObjects.Add(lyric = new Lyric());

            var lyricLayout = stageInfo.LyricLayoutCategory.AvailableElements.First();

            // Add to Mapping
            stageInfo.LyricLayoutCategory.AddToMapping(lyricLayout, lyric);
        });

        TriggerHandlerChanged(c =>
        {
            c.RemoveFromMapping(lyric);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var category = getStageCategory(karaokeBeatmap);

            Assert.IsEmpty(category.Mappings);
        });
    }

    [Test]
    public void TestClearUnusedMapping()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            stageInfo.LyricLayoutCategory.AddElement();
            karaokeBeatmap.StageInfos.Add(stageInfo);

            var lyricLayout = stageInfo.LyricLayoutCategory.AvailableElements.First();

            // Add to Mapping
            stageInfo.LyricLayoutCategory.AddToMapping(lyricLayout, new Lyric());
        });

        TriggerHandlerChanged(c =>
        {
            c.ClearUnusedMapping();
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var category = getStageCategory(karaokeBeatmap);

            Assert.IsEmpty(category.Mappings);
        });
    }

    private static ClassicLyricLayoutCategory getStageCategory(KaraokeBeatmap beatmap)
    {
        return beatmap.StageInfos.OfType<ClassicStageInfo>().First().LyricLayoutCategory;
    }

    public partial class TestBeatmapStageElementCategoryChangeHandler : BeatmapStageElementCategoryChangeHandler<ClassicLyricLayout, Lyric>
    {
        public TestBeatmapStageElementCategoryChangeHandler(Func<IEnumerable<StageInfo>, StageElementCategory<ClassicLyricLayout, Lyric>> action)
            : base(action)
        {
        }
    }
}
