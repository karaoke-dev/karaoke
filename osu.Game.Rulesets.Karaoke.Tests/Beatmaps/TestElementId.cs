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
            Assert.DoesNotThrow(() =>
            {
                var _ = new ElementId(id);
            });
        }
        else
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var _ = new ElementId(id);
            });
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

    [TestCase("1234567", "1234567", 0)]
    [TestCase("1234567", "2345678", -1)]
    [TestCase("2345678", "1234567", 1)]
    public void TestCompareTo(string a, string b, int expected)
    {
        var elementIdA = new ElementId(a);
        var elementIdB = new ElementId(b);

        int actual = elementIdA.CompareTo(elementIdB);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestCompareToOther()
    {
        var elementId = new ElementId("1234567");

        Assert.AreEqual(elementId.CompareTo(null), 1);
        Assert.Throws<ArgumentException>(() =>
        {
            int _ = elementId.CompareTo(3);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            // should not compare to other type
            int _ = elementId.CompareTo("123");
        });
        Assert.DoesNotThrow(() =>
        {
            // should not compare to the string also.
            int _ = elementId.CompareTo(new ElementId("1234567"));
        });
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
