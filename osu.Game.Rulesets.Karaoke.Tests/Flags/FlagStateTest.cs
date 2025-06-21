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
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(expectedValue));

        // value should not changed if did the same action.
        validator.Invalidate(invalidFlags);
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(expectedValue));

        // should not be negative if remove the value from the validator.
        validator.InvalidateAll();
        validator.Invalidate(invalidFlags);
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Array.Empty<TestEnum>()));
        Assert.That(validator.GetAllValidFlags().Sum(x => Convert.ToInt32(x)), Is.EqualTo(0));
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
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Array.Empty<TestEnum>()));
        Assert.That(validator.GetAllValidFlags().Sum(x => Convert.ToInt32(x)), Is.EqualTo(0));
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
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(expectedValue));

        // value should not changed if did the same action.
        validator.Validate(validateFlags);
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(expectedValue));

        // should not exceed sum values if remove the value from the validator.
        validator.ValidateAll();
        validator.Validate(validateFlags);
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Enum.GetValues<TestEnum>()));
        Assert.That(validator.GetAllValidFlags().Sum(x => Convert.ToInt32(x)), Is.EqualTo(Enum.GetValues<TestEnum>().Sum(x => Convert.ToInt32(x))));
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
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Enum.GetValues<TestEnum>()));
        Assert.That(validator.GetAllValidFlags().Sum(x => Convert.ToInt32(x)), Is.EqualTo(Enum.GetValues<TestEnum>().Sum(x => Convert.ToInt32(x))));
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
        Assert.That(validator.IsValid(validFlag), Is.EqualTo(expectedValue));
    }

    [Test]
    public void TestGetAllValidFlags()
    {
        var validator = new FlagState<TestEnum>();

        // Should be possible to get all tags.
        validator.ValidateAll();
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Enum.GetValues<TestEnum>()));

        // Should not be possible to get any tags.
        validator.InvalidateAll();
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Array.Empty<TestEnum>()));
    }

    [Test]
    public void TestGetAllInvalidFlags()
    {
        var validator = new FlagState<TestEnum>();

        // Should be possible to get all tags.
        validator.ValidateAll();
        Assert.That(validator.GetAllInvalidFlags(), Is.EqualTo(Array.Empty<TestEnum>()));

        // Should not be possible to get any tags.
        validator.InvalidateAll();
        Assert.That(validator.GetAllInvalidFlags(), Is.EqualTo(Enum.GetValues<TestEnum>()));
    }

    [Test]
    public void TestGetAllValidFlagsWithAndCondition()
    {
        var validator = new FlagState<TestAndEnum>();

        validator.Validate(TestAndEnum.Enum0);
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(new[] { TestAndEnum.Enum0 }));

        validator.Validate(TestAndEnum.Enum0 | TestAndEnum.Enum1);
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(new[] { TestAndEnum.Enum0, TestAndEnum.Enum1, TestAndEnum.Enum0And1 }));
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Enum.GetValues<TestAndEnum>()));

        validator.ValidateAll();
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(new[] { TestAndEnum.Enum0, TestAndEnum.Enum1, TestAndEnum.Enum0And1 }));
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Enum.GetValues<TestAndEnum>()));
    }

    [Test]
    public void TestGetAllInvalidFlagsWithAndCondition()
    {
        var validator = new FlagState<TestAndEnum>();
        validator.ValidateAll();

        validator.Invalidate(TestAndEnum.Enum0);
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(new[] { TestAndEnum.Enum1 }));

        validator.Invalidate(TestAndEnum.Enum1);
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Array.Empty<TestEnum>()));

        validator.InvalidateAll();
        Assert.That(validator.GetAllValidFlags(), Is.EqualTo(Array.Empty<TestEnum>()));
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
