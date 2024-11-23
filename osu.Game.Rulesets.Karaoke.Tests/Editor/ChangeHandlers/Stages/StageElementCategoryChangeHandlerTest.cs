// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.UI.Stages;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Stages;

public partial class StageElementCategoryChangeHandlerTest : BaseChangeHandlerTest<StageElementCategoryChangeHandlerTest.TestStageElementCategoryChangeHandler>
{
    protected override TestStageElementCategoryChangeHandler CreateChangeHandler()
        => new(x => x.OfType<TestStageInfo>().First().Category);

    [Test]
    public void TestAddElement()
    {
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
            var category = getStageCategory(karaokeBeatmap);
            element = category.AddElement();
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
            var category = getStageCategory(karaokeBeatmap);
            element = category.AddElement();
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
            var category = getStageCategory(karaokeBeatmap);
            element = category.AddElement();
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
        Lyric lyric = new Lyric();
        Lyric unSelectedLyric = new Lyric();

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var category = getStageCategory(karaokeBeatmap);
            var element = category.AddElement(x => x.Name = "Element 1");
            category.AddElement(x => x.Name = "Element 2");

            // Add to Mapping
            category.AddToMapping(element, lyric);
            category.AddToMapping(element, unSelectedLyric);
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
            var category = getStageCategory(karaokeBeatmap);
            var element = category.AddElement();

            // Add to Mapping
            category.AddToMapping(element, lyric);
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
            var category = getStageCategory(karaokeBeatmap);
            var element = category.AddElement();

            // Add to Mapping
            category.AddToMapping(element, new Lyric());
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

    protected override void SetUpKaraokeBeatmap(Action<KaraokeBeatmap> action)
    {
        base.SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new TestStageInfo();
            karaokeBeatmap.StageInfos = new List<StageInfo>
            {
                stageInfo,
            };
            karaokeBeatmap.CurrentStageInfo = stageInfo;

            action(karaokeBeatmap);
        });
    }

    private static TestCategory getStageCategory(KaraokeBeatmap beatmap)
        => beatmap.StageInfos.OfType<TestStageInfo>().First().Category;

    public partial class TestStageElementCategoryChangeHandler : StageElementCategoryChangeHandler<TestStageElement, Lyric>
    {
        public TestStageElementCategoryChangeHandler(Func<IEnumerable<StageInfo>, StageElementCategory<TestStageElement, Lyric>> stageCategoryAction)
            : base(stageCategoryAction)
        {
        }
    }

    private class TestStageInfo : StageInfo
    {
        #region Category

        /// <summary>
        /// Category to save the <see cref="Lyric"/>'s and <see cref="Note"/>'s style.
        /// </summary>
        public TestCategory Category { get; } = new();

        #endregion

        #region Stage element

        protected override IEnumerable<StageElement> GetLyricStageElements(Lyric lyric)
        {
            return Array.Empty<StageElement>();
        }

        protected override IEnumerable<StageElement> GetNoteStageElements(Note note)
        {
            return Array.Empty<StageElement>();
        }

        #endregion

        #region Provider

        public override IPlayfieldStageApplier GetPlayfieldStageApplier()
            => throw new NotImplementedException();

        public override IPlayfieldCommandProvider CreatePlayfieldCommandProvider(bool displayNotePlayfield)
            => throw new NotImplementedException();

        public override IHitObjectCommandProvider? CreateHitObjectCommandProvider<TObject>() =>
            typeof(TObject) switch
            {
                Type type when type == typeof(Lyric) => new TestCommandProvider(this),
                Type type when type == typeof(Note) => null,
                _ => null
            };

        #endregion
    }

    private class TestCategory : StageElementCategory<TestStageElement, Lyric>;

    public class TestStageElement : StageElement, IComparable<TestStageElement>
    {
        public int CompareTo(TestStageElement? other)
        {
            return ComparableUtils.CompareByProperty(this, other,
                x => x.Name,
                x => x.ID);
        }
    }

    private class TestCommandProvider : HitObjectCommandProvider<TestStageInfo, Lyric>
    {
        public TestCommandProvider(TestStageInfo stageInfo)
            : base(stageInfo)
        {
        }

        protected override double GeneratePreemptTime(Lyric hitObject)
            => 0;

        protected override Tuple<double?, double?> GetStartAndEndTime(Lyric lyric)
        {
            if(!lyric.TimeValid)
                return new Tuple<double?, double?>(null, null);

            return new Tuple<double?, double?>(lyric.StartTime, lyric.EndTime);
        }

        protected override IEnumerable<IStageCommand> GetInitialCommands(Lyric hitObject)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IStageCommand> GetStartTimeStateCommands(Lyric hitObject)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IStageCommand> GetHitStateCommands(Lyric hitObject, ArmedState state)
        {
            throw new NotImplementedException();
        }
    }
}
