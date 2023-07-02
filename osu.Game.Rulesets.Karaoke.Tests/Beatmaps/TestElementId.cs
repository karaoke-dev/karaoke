// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using J2N.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps;

public class TestElementId
{
    [Test]
    public void TestEmptyElementId()
    {
        var emptyElementId = ElementId.Empty;

        // Should get empty string if the element is empty.
        Assert.IsEmpty(emptyElementId.ToString());

        // Should be equal to empty element id.
        Assert.IsTrue(emptyElementId.Equals(ElementId.Empty));
        Assert.IsFalse(emptyElementId.Equals(ElementId.NewElementId()));
    }

    [TestCase("1234567", true)] // number is OK
    [TestCase("0abcdef", true)] // alphabet is OK
    [TestCase("0ABCDEF", false)] // upper case is not allowed.
    [TestCase("abcdefg", false)] // alphabet should be within a~f
    [TestCase("xyz7890", false)] // alphabet should be within a~f
    [TestCase("123456", false)] // should be 7 digits
    [TestCase("abcdefgh", false)] // should be 7 digits
    [TestCase("", false)] // should be 7 digits
    public void TestCreateElementId(string id, bool created)
    {
        if (created)
        {
            Assert.DoesNotThrow(() => new ElementId(id));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => new ElementId(id));
        }
    }

    [Test]
    public void TestNewElementIdShouldNotDuplicated()
    {
        const int create_amount = 1000;

        // Arrange
        HashSet<ElementId> idSet = new HashSet<ElementId>();

        for (int i = 0; i < create_amount; i++)
        {
            var elementId = ElementId.NewElementId();
            idSet.Add(elementId);
        }

        Assert.AreEqual(idSet.Count, create_amount);
    }

    [TestCase("1234567", "1234567", true)]
    [TestCase("1234567", "7654321", false)]
    public void TestEqual(string a, string b, bool expected)
    {
        var elementIdA = new ElementId(a);
        var elementIdB = new ElementId(b);

        bool actual = elementIdA.Equals(elementIdB);
        Assert.AreEqual(expected, actual);
    }

    [TestCase("1234567", "1234567", true)]
    [TestCase("1234567", "7654321", false)]
    public void TestEqualWithEqualityComparer(string a, string b, bool expected)
    {
        var elementIdA = new ElementId(a);
        var elementIdB = new ElementId(b);

        bool actual = EqualityComparer<object>.Default.Equals(elementIdA, elementIdB);
        Assert.AreEqual(expected, actual);
    }

    [TestCase("1234567", "1234567", true)]
    [TestCase("1234567", "7654321", false)]
    public void TestEqualOperator(string a, string b, bool expected)
    {
        var elementIdA = new ElementId(a);
        var elementIdB = new ElementId(b);

        bool actual = elementIdA == elementIdB;
        Assert.AreEqual(expected, actual);
    }

    [TestCase("1234567", "1234567", false)]
    [TestCase("1234567", "7654321", true)]
    public void TestNotEqualOperator(string a, string b, bool expected)
    {
        var elementIdA = new ElementId(a);
        var elementIdB = new ElementId(b);

        bool actual = elementIdA != elementIdB;
        Assert.AreEqual(expected, actual);
    }

    [TestCase("1234567", "1234567")]
    [TestCase("7654321", "7654321")]
    public void TestToString(string a, string expected)
    {
        var elementId = new ElementId(a);

        Assert.AreEqual(expected, elementId.ToString());
    }
}
