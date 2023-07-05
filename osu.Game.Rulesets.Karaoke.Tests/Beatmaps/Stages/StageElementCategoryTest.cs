// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Stages;

public class StageElementCategoryTest
{
    #region Edit

    [Test]
    public void TestAddElement()
    {
        var category = new TestStageElementCategory();
        category.AddElement(x =>
        {
            x.Name = "Element 1";
        });

        Assert.AreEqual(1, category.AvailableElements.Count);
        Assert.AreEqual(1, category.AvailableElements[0].ID);
        Assert.AreEqual("Element 1", category.AvailableElements[0].Name);

        category.AddElement();

        Assert.AreEqual(2, category.AvailableElements.Count);
        Assert.AreEqual(2, category.AvailableElements[1].ID);
    }

    [Test]
    public void TestEditElement()
    {
        var category = new TestStageElementCategory();
        var element = category.AddElement();

        int id = element.ID;
        category.EditElement(id, x =>
        {
            x.Name = "Element 1";
        });

        Assert.AreEqual(1, category.AvailableElements.Count);
        Assert.AreEqual(1, category.AvailableElements[0].ID);
        Assert.AreEqual("Element 1", category.AvailableElements[0].Name);
    }

    [Test]
    public void TestRemoveElement()
    {
        var category = new TestStageElementCategory();

        var element1 = category.AddElement();
        var element2 = category.AddElement();
        var lyric1 = new Lyric();
        var lyric2 = new Lyric();
        var lyric3 = new Lyric();

        category.AddToMapping(element1, lyric1);
        category.AddToMapping(element1, lyric2);
        category.AddToMapping(element2, lyric3);

        // Only remove element 1.
        category.RemoveElement(element1);

        // Should have only one element.
        var defaultElement = category.DefaultElement;
        Assert.AreEqual(1, category.AvailableElements.Count);

        // Should get the default element because mapping has been removed.
        Assert.AreEqual(defaultElement, category.GetElementByItem(lyric1));
        Assert.AreEqual(defaultElement, category.GetElementByItem(lyric2));
        Assert.AreEqual(element2, category.GetElementByItem(lyric3));
    }

    [Test]
    public void TestClearElements()
    {
        var category = new TestStageElementCategory();

        var element1 = category.AddElement();
        var element2 = category.AddElement();
        var lyric1 = new Lyric();
        var lyric2 = new Lyric();
        var lyric3 = new Lyric();

        category.AddToMapping(element1, lyric1);
        category.AddToMapping(element1, lyric2);
        category.AddToMapping(element2, lyric3);

        // Clear all elements.
        category.ClearElements();

        // Should clear everything.
        Assert.AreEqual(0, category.AvailableElements.Count);
        Assert.AreEqual(0, category.Mappings.Count);

        // should get the default element.
        var defaultElement = category.DefaultElement;
        Assert.AreEqual(defaultElement, category.GetElementByItem(lyric1));
    }

    [Test]
    public void TestAddToMapping()
    {
        var category = new TestStageElementCategory();

        var element1 = category.AddElement();
        var element2 = category.AddElement();
        var lyric1 = new Lyric();
        var lyric2 = new Lyric();
        var lyric3 = new Lyric();

        category.AddToMapping(element1, lyric1);
        category.AddToMapping(element1, lyric2);
        category.AddToMapping(element2, lyric3);

        // Should get the matched element.
        Assert.AreEqual(element1, category.GetElementByItem(lyric1));
        Assert.AreEqual(element1, category.GetElementByItem(lyric2));
        Assert.AreEqual(element2, category.GetElementByItem(lyric3));
    }

    [Test]
    public void TestRemoveHitObjectFromMapping()
    {
        var category = new TestStageElementCategory();

        var element1 = category.AddElement();
        var lyric1 = new Lyric();

        category.AddToMapping(element1, lyric1);
        category.RemoveHitObjectFromMapping(lyric1);

        // Should clear added mappings.
        var mappings = category.Mappings;
        Assert.IsEmpty(mappings);
    }

    [Test]
    public void TestRemoveElementFromMapping()
    {
        var category = new TestStageElementCategory();

        var element1 = category.AddElement();
        var lyric1 = new Lyric();

        category.AddToMapping(element1, lyric1);
        category.RemoveElementFromMapping(element1);

        // Should clear added mappings.
        var mappings = category.Mappings;
        Assert.IsEmpty(mappings);
    }

    [Test]
    public void TestClearUnusedMapping()
    {
        var category = new TestStageElementCategory();

        var element1 = category.AddElement();
        var lyric1 = new Lyric();
        var lyric2 = new Lyric();
        var lyric3 = new Lyric();

        category.AddToMapping(element1, lyric1);
        category.AddToMapping(element1, lyric2);
        category.AddToMapping(element1, lyric3);

        var lyricsInTheBeatmap = new[] { lyric1, lyric2 };
        category.ClearUnusedMapping(id => lyricsInTheBeatmap.Any(x => x.ID == id));

        // Should get the matched element.
        Assert.AreEqual(element1, category.GetElementByItem(lyric1));
        Assert.AreEqual(element1, category.GetElementByItem(lyric2));

        // should get the default element because lyric3 is clear in the mapping.
        var defaultElement = category.DefaultElement;
        Assert.AreEqual(defaultElement, category.GetElementByItem(lyric3));
    }

    #endregion

    #region Query

    [Test]
    public void TestGetElementByItem()
    {
        var category = new TestStageElementCategory();

        var element1 = category.AddElement();
        var lyric1 = new Lyric();
        var lyric2 = new Lyric();

        category.AddToMapping(element1, lyric1);

        // Should get the matched element.
        Assert.AreEqual(element1, category.GetElementByItem(lyric1));

        // Should get the default element because it's not in the mapping list.
        var defaultElement = category.DefaultElement;
        Assert.AreEqual(defaultElement, category.GetElementByItem(lyric2));
    }

    [Test]
    public void TestGetElementOrder()
    {
        var category = new TestStageElementCategory();
        var element = category.AddElement();

        int? existElementOrder = category.GetElementOrder(element);
        Assert.AreEqual(1, existElementOrder);

        var notExistElement = new TestStageElement(-1);
        int? notExistElementOrder = category.GetElementOrder(notExistElement);
        Assert.IsNull(notExistElementOrder);
    }

    #endregion

    private class TestStageElement : StageElement, IComparable<TestStageElement>
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

    private class TestStageElementCategory : StageElementCategory<TestStageElement, Lyric>
    {
        protected override TestStageElement CreateElement(int id) => new(id);
    }
}
