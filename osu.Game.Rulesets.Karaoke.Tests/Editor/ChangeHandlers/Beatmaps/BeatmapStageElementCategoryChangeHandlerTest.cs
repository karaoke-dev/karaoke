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

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            stageInfo.LyricLayoutCategory.AddElement();
            karaokeBeatmap.StageInfos.Add(stageInfo);

            lyricLayout = stageInfo.LyricLayoutCategory.AvailableElements.First();
        });

        PrepareHitObject(() => new Lyric());

        TriggerHandlerChanged(c =>
        {
            c.AddToMapping(lyricLayout);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var category = getStageCategory(karaokeBeatmap);

            Assert.IsNotEmpty(category.Mappings);
        });
    }

    [Test]
    public void TestOffsetMapping()
    {
        Lyric lyric = new Lyric { ID = 1 };
        Lyric unSelectedLyric = new Lyric { ID = 2 };

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            stageInfo.LyricLayoutCategory.AddElement(x => x.Name = "Layout 1");
            stageInfo.LyricLayoutCategory.AddElement(x => x.Name = "Layout 2");
            karaokeBeatmap.StageInfos.Add(stageInfo);

            var lyricLayout = stageInfo.LyricLayoutCategory.AvailableElements.First();

            // Add to Mapping
            stageInfo.LyricLayoutCategory.AddToMapping(lyricLayout, lyric);
            stageInfo.LyricLayoutCategory.AddToMapping(lyricLayout, unSelectedLyric);
        });

        PrepareHitObject(() => lyric);
        PrepareHitObject(() => unSelectedLyric, false);

        TriggerHandlerChanged(c =>
        {
            c.OffsetMapping(1);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var category = getStageCategory(karaokeBeatmap);

            Assert.AreEqual("Layout 2", category.GetElementByItem(lyric).Name);
            Assert.AreEqual("Layout 1", category.GetElementByItem(unSelectedLyric).Name); // should not change the id if lyric is not selected.
        });
    }

    [Test]
    public void TestOffsetMappingWithZeroValue()
    {
        PrepareHitObject(() => new Lyric());

        // offset value should not be zero.
        TriggerHandlerChangedWithException<InvalidOperationException>(c => c.OffsetMapping(0));
    }

    [Test]
    public void TestRemoveFromMapping()
    {
        Lyric lyric = new Lyric();

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            stageInfo.LyricLayoutCategory.AddElement();
            karaokeBeatmap.StageInfos.Add(stageInfo);

            var lyricLayout = stageInfo.LyricLayoutCategory.AvailableElements.First();

            // Add to Mapping
            stageInfo.LyricLayoutCategory.AddToMapping(lyricLayout, lyric);
        });

        PrepareHitObject(() => lyric);

        TriggerHandlerChanged(c =>
        {
            c.RemoveFromMapping();
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
        public TestBeatmapStageElementCategoryChangeHandler(Func<IEnumerable<StageInfo>, StageElementCategory<ClassicLyricLayout, Lyric>> stageCategoryAction)
            : base(stageCategoryAction)
        {
        }
    }
}
