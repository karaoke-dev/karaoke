// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Stages.Infos;

public class StageElementCategoryTest
{
    #region Edit

    [Test]
    public void TestAddElement()
    {
        var category = new TestStageElementCategory();
        var element = category.AddElement(x =>
        {
            x.Name = "Element 1";
        });

        Assert.That(category.AvailableElements.Count, Is.EqualTo(1));
        Assert.That(category.AvailableElements[0].ID, Is.Not.EqualTo(ElementId.Empty));
        Assert.That(category.AvailableElements[0].Name, Is.EqualTo("Element 1"));

        category.AddElement();

        Assert.That(category.AvailableElements.Count, Is.EqualTo(2));
        Assert.That(category.AvailableElements[1].ID, Is.Not.EqualTo(ElementId.Empty));
    }

    [Test]
    public void TestEditElement()
    {
        var category = new TestStageElementCategory();
        var element = category.AddElement();

        var id = element.ID;
        category.EditElement(id, x =>
        {
            x.Name = "Element 1";
        });

        Assert.That(category.AvailableElements.Count, Is.EqualTo(1));
        Assert.That(category.AvailableElements[0].ID, Is.Not.EqualTo(ElementId.Empty));
        Assert.That(category.AvailableElements[0].Name, Is.EqualTo("Element 1"));
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
        Assert.That(category.AvailableElements.Count, Is.EqualTo(1));

        // Should get the default element because mapping has been removed.
        Assert.That(category.GetElementByItem(lyric1), Is.EqualTo(defaultElement));
        Assert.That(category.GetElementByItem(lyric2), Is.EqualTo(defaultElement));
        Assert.That(category.GetElementByItem(lyric3), Is.EqualTo(element2));
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
        Assert.That(category.AvailableElements.Count, Is.EqualTo(0));
        Assert.That(category.Mappings.Count, Is.EqualTo(0));

        // should get the default element.
        var defaultElement = category.DefaultElement;
        Assert.That(category.GetElementByItem(lyric1), Is.EqualTo(defaultElement));
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
        Assert.That(category.GetElementByItem(lyric1), Is.EqualTo(element1));
        Assert.That(category.GetElementByItem(lyric2), Is.EqualTo(element1));
        Assert.That(category.GetElementByItem(lyric3), Is.EqualTo(element2));
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
        Assert.That(mappings, Is.Empty);
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
        Assert.That(mappings, Is.Empty);
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
        Assert.That(category.GetElementByItem(lyric1), Is.EqualTo(element1));
        Assert.That(category.GetElementByItem(lyric2), Is.EqualTo(element1));

        // should get the default element because lyric3 is clear in the mapping.
        var defaultElement = category.DefaultElement;
        Assert.That(category.GetElementByItem(lyric3), Is.EqualTo(defaultElement));
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
        Assert.That(category.GetElementByItem(lyric1), Is.EqualTo(element1));

        // Should get the default element because it's not in the mapping list.
        var defaultElement = category.DefaultElement;
        Assert.That(category.GetElementByItem(lyric2), Is.EqualTo(defaultElement));
    }

    [Test]
    public void TestGetHitObjectIdsByElement()
    {
        var category = new TestStageElementCategory();

        var element1 = category.AddElement();
        var lyric1 = new Lyric();

        category.AddToMapping(element1, lyric1);

        // Should get the matched element.
        Assert.That(category.GetHitObjectIdsByElement(element1), Is.EqualTo(new[] { lyric1.ID }));

        // Should get the default element because it's not in the mapping list.
        var defaultElement = category.DefaultElement;
        Assert.That(category.GetHitObjectIdsByElement(defaultElement), Is.Empty);
    }

    #endregion

    private class TestStageElement : StageElement, IComparable<TestStageElement>
    {
        public int CompareTo(TestStageElement? other)
        {
            return ComparableUtils.CompareByProperty(this, other,
                x => x.Name,
                x => x.ID);
        }
    }

    private class TestStageElementCategory : StageElementCategory<TestStageElement, Lyric>
    {
        protected override TestStageElement CreateDefaultElement()
            => new();
    }
}
