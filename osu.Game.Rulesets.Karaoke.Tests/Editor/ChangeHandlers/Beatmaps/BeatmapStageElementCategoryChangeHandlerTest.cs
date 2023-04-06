// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public partial class BeatmapStageElementCategoryChangeHandlerTest : BaseChangeHandlerTest<BeatmapStageElementCategoryChangeHandlerTest.TestBeatmapStageElementCategoryChangeHandler>
{
    protected override TestBeatmapStageElementCategoryChangeHandler CreateChangeHandler()
        => new(x => x.OfType<TestStageinfo>().First().Category);

    [Test]
    public void TestAddElement()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.StageInfos.Add(new TestStageinfo());
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
        TestStageElement element = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new TestStageinfo();
            element = stageInfo.Category.AddElement();

            karaokeBeatmap.StageInfos.Add(stageInfo);
        });

        TriggerHandlerChanged(c =>
        {
            c.EditElement(element.ID, x =>
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
        TestStageElement element = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new TestStageinfo();
            element = stageInfo.Category.AddElement();

            karaokeBeatmap.StageInfos.Add(stageInfo);
        });

        TriggerHandlerChanged(c =>
        {
            c.RemoveElement(element);
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
        TestStageElement element = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new TestStageinfo();
            element = stageInfo.Category.AddElement();

            karaokeBeatmap.StageInfos.Add(stageInfo);
        });

        PrepareHitObject(() => new Lyric());

        TriggerHandlerChanged(c =>
        {
            c.AddToMapping(element);
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
            var stageInfo = new TestStageinfo();
            var element = stageInfo.Category.AddElement(x => x.Name = "Element 1");
            stageInfo.Category.AddElement(x => x.Name = "Element 2");

            // Add to Mapping
            stageInfo.Category.AddToMapping(element, lyric);
            stageInfo.Category.AddToMapping(element, unSelectedLyric);

            karaokeBeatmap.StageInfos.Add(stageInfo);
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

            Assert.AreEqual("Element 2", category.GetElementByItem(lyric).Name);
            Assert.AreEqual("Element 1", category.GetElementByItem(unSelectedLyric).Name); // should not change the id if lyric is not selected.
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
            var stageInfo = new TestStageinfo();
            var element = stageInfo.Category.AddElement();

            // Add to Mapping
            stageInfo.Category.AddToMapping(element, lyric);

            karaokeBeatmap.StageInfos.Add(stageInfo);
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
            var stageInfo = new TestStageinfo();
            var element = stageInfo.Category.AddElement();

            // Add to Mapping
            stageInfo.Category.AddToMapping(element, new Lyric());

            karaokeBeatmap.StageInfos.Add(stageInfo);
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

    private static TestCategory getStageCategory(KaraokeBeatmap beatmap)
    {
        return beatmap.StageInfos.OfType<TestStageinfo>().First().Category;
    }

    public partial class TestBeatmapStageElementCategoryChangeHandler : BeatmapStageElementCategoryChangeHandler<TestStageElement, Lyric>
    {
        public TestBeatmapStageElementCategoryChangeHandler(Func<IEnumerable<StageInfo>, StageElementCategory<TestStageElement, Lyric>> stageCategoryAction)
            : base(stageCategoryAction)
        {
        }
    }

    private class TestStageinfo : StageInfo
    {
        #region Category

        /// <summary>
        /// Category to save the <see cref="Lyric"/>'s and <see cref="Note"/>'s style.
        /// </summary>
        public TestCategory Category { get; } = new();

        #endregion

        protected override IEnumerable<StageElement> GetLyricStageElements(Lyric lyric)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<StageElement> GetNoteStageElements(Note note)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> ConvertToLyricStageAppliers(IEnumerable<StageElement> elements)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> ConvertToNoteStageAppliers(IEnumerable<StageElement> elements)
        {
            throw new NotImplementedException();
        }

        protected override Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
        {
            throw new NotImplementedException();
        }
    }

    private class TestCategory : StageElementCategory<TestStageElement, Lyric>
    {
        protected override TestStageElement CreateElement(int id) => new(id);
    }

    public class TestStageElement : StageElement, IComparable<TestStageElement>
    {
        public TestStageElement(int id)
            : base(id)
        {
        }

        public int CompareTo(TestStageElement? other)
        {
            return ComparableUtils.CompareByProperty(this, other,
                x => x.Name,
                x => x.ID);
        }
    }
}
