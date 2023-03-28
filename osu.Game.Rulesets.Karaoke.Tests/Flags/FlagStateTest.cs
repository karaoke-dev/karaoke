// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Flags;

namespace osu.Game.Rulesets.Karaoke.Tests.Flags;

public class FlagStateTest
{
    [TestCase(default, new[] { TestEnum.Enum0, TestEnum.Enum1, TestEnum.Enum2 })]
    [TestCase(TestEnum.Enum0, new[] { TestEnum.Enum1, TestEnum.Enum2 })]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum1, new[] { TestEnum.Enum2 })]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum1 | TestEnum.Enum2, new TestEnum[] { })]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum2, new[] { TestEnum.Enum1 })] // Should work without continuous values.
    [TestCase(TestEnum.Enum0 | TestEnum.Enum0, new[] { TestEnum.Enum1, TestEnum.Enum2 })] // Test edge case.
    public void TestInvalidate(TestEnum invalidFlags, TestEnum[] expectedValue)
    {
        var validator = new FlagState<TestEnum>();
        validator.ValidateAll();

        // check the value.
        validator.Invalidate(invalidFlags);
        Assert.AreEqual(expectedValue, validator.GetAllValidFlags());

        // value should not changed if did the same action.
        validator.Invalidate(invalidFlags);
        Assert.AreEqual(expectedValue, validator.GetAllValidFlags());

        // should not be negative if remove the value from the validator.
        validator.InvalidateAll();
        validator.Invalidate(invalidFlags);
        Assert.AreEqual(Array.Empty<TestEnum>(), validator.GetAllValidFlags());
        Assert.AreEqual(0, validator.GetAllValidFlags().Sum(x => Convert.ToInt32(x)));
    }

    [TestCase(TestEnum.Enum0)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum1)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum1 | TestEnum.Enum2)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum2)] // Should work without continuous values.
    [TestCase(TestEnum.Enum0 | TestEnum.Enum0)] // Test edge case.
    public void TestInvalidateAll(TestEnum defaultFlags)
    {
        var validator = new FlagState<TestEnum>();
        validator.Validate(defaultFlags);

        // check the value.
        validator.InvalidateAll();
        Assert.AreEqual(Array.Empty<TestEnum>(), validator.GetAllValidFlags());
        Assert.AreEqual(0, validator.GetAllValidFlags().Sum(x => Convert.ToInt32(x)));
    }

    [TestCase(default, new TestEnum[] { })]
    [TestCase(TestEnum.Enum0, new[] { TestEnum.Enum0 })]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum1, new[] { TestEnum.Enum0, TestEnum.Enum1 })]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum1 | TestEnum.Enum2, new[] { TestEnum.Enum0, TestEnum.Enum1, TestEnum.Enum2 })]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum2, new[] { TestEnum.Enum0, TestEnum.Enum2 })] // Should work without continuous values.
    [TestCase(TestEnum.Enum0 | TestEnum.Enum0, new[] { TestEnum.Enum0 })] // Test edge case.
    [TestCase(TestEnum.Enum2 | TestEnum.Enum2, new[] { TestEnum.Enum2 })] // Test edge case.
    public void TestValidate(TestEnum validateFlags, TestEnum[] expectedValue)
    {
        var validator = new FlagState<TestEnum>();

        // check the value.
        validator.Validate(validateFlags);
        Assert.AreEqual(expectedValue, validator.GetAllValidFlags());

        // value should not changed if did the same action.
        validator.Validate(validateFlags);
        Assert.AreEqual(expectedValue, validator.GetAllValidFlags());

        // should not exceed sum values if remove the value from the validator.
        validator.ValidateAll();
        validator.Validate(validateFlags);
        Assert.AreEqual(Enum.GetValues<TestEnum>(), validator.GetAllValidFlags());
        Assert.AreEqual(Enum.GetValues<TestEnum>().Sum(x => Convert.ToInt32(x)), validator.GetAllValidFlags().Sum(x => Convert.ToInt32(x)));
    }

    [TestCase(TestEnum.Enum0)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum1)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum1 | TestEnum.Enum2)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum2)] // Should work without continuous values.
    [TestCase(TestEnum.Enum0 | TestEnum.Enum0)] // Test edge case.
    public void TestValidateAll(TestEnum validateFlags)
    {
        var validator = new FlagState<TestEnum>();

        // check the value.
        validator.ValidateAll();
        Assert.AreEqual(Enum.GetValues<TestEnum>(), validator.GetAllValidFlags());
        Assert.AreEqual(Enum.GetValues<TestEnum>().Sum(x => Convert.ToInt32(x)), validator.GetAllValidFlags().Sum(x => Convert.ToInt32(x)));
    }

    [TestCase(TestEnum.Enum0, TestEnum.Enum0, true)]
    [TestCase(TestEnum.Enum0, TestEnum.Enum1, false)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum2, TestEnum.Enum0, true)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum2, TestEnum.Enum1, false)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum2, TestEnum.Enum2, true)]
    [TestCase(TestEnum.Enum0 | TestEnum.Enum0, TestEnum.Enum0, true)] // Test edge case.
    [TestCase(TestEnum.Enum0 | TestEnum.Enum0, TestEnum.Enum2, false)] // Test edge case.
    [TestCase(TestEnum.Enum2 | TestEnum.Enum2, TestEnum.Enum0, false)] // Test edge case.
    [TestCase(TestEnum.Enum2 | TestEnum.Enum2, TestEnum.Enum2, true)] // Test edge case.
    public void TestIsValid(TestEnum validateFlags, TestEnum validFlag, bool expectedValue)
    {
        var validator = new FlagState<TestEnum>();

        // check the value.
        validator.Validate(validateFlags);
        Assert.AreEqual(expectedValue, validator.IsValid(validFlag));
    }

    [Test]
    public void TestGetAllValidFlags()
    {
        var validator = new FlagState<TestEnum>();

        // Should be possible to get all tags.
        validator.ValidateAll();
        Assert.AreEqual(Enum.GetValues<TestEnum>(), validator.GetAllValidFlags());

        // Should not be possible to get any tags.
        validator.InvalidateAll();
        Assert.AreEqual(Array.Empty<TestEnum>(), validator.GetAllValidFlags());
    }

    [Test]
    public void TestGetAllInvalidFlags()
    {
        var validator = new FlagState<TestEnum>();

        // Should be possible to get all tags.
        validator.ValidateAll();
        Assert.AreEqual(Array.Empty<TestEnum>(), validator.GetAllInvalidFlags());

        // Should not be possible to get any tags.
        validator.InvalidateAll();
        Assert.AreEqual(Enum.GetValues<TestEnum>(), validator.GetAllInvalidFlags());
    }

    [Test]
    public void TestGetAllValidFlagsWithAndCondition()
    {
        var validator = new FlagState<TestAndEnum>();

        validator.Validate(TestAndEnum.Enum0);
        Assert.AreEqual(new[] { TestAndEnum.Enum0 }, validator.GetAllValidFlags());

        validator.Validate(TestAndEnum.Enum0 | TestAndEnum.Enum1);
        Assert.AreEqual(new[] { TestAndEnum.Enum0, TestAndEnum.Enum1, TestAndEnum.Enum0And1 }, validator.GetAllValidFlags());
        Assert.AreEqual(Enum.GetValues<TestAndEnum>(), validator.GetAllValidFlags());

        validator.ValidateAll();
        Assert.AreEqual(new[] { TestAndEnum.Enum0, TestAndEnum.Enum1, TestAndEnum.Enum0And1 }, validator.GetAllValidFlags());
        Assert.AreEqual(Enum.GetValues<TestAndEnum>(), validator.GetAllValidFlags());
    }

    [Test]
    public void TestGetAllInvalidFlagsWithAndCondition()
    {
        var validator = new FlagState<TestAndEnum>();
        validator.ValidateAll();

        validator.Invalidate(TestAndEnum.Enum0);
        Assert.AreEqual(new[] { TestAndEnum.Enum1 }, validator.GetAllValidFlags());

        validator.Invalidate(TestAndEnum.Enum1);
        Assert.AreEqual(Array.Empty<TestEnum>(), validator.GetAllValidFlags());

        validator.InvalidateAll();
        Assert.AreEqual(Array.Empty<TestEnum>(), validator.GetAllValidFlags());
    }

    [Flags]
    public enum TestEnum
    {
        Enum0 = 1,

        Enum1 = 1 << 1,

        Enum2 = 1 << 2,
    }

    [Flags]
    public enum TestAndEnum
    {
        Enum0 = 1,

        Enum1 = 1 << 1,

        Enum0And1 = Enum0 | Enum1,
    }
}
