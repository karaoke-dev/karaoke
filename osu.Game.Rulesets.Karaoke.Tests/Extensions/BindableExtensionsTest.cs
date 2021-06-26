// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Extensions;

namespace osu.Game.Rulesets.Karaoke.Tests.Extensions
{
    public class BindableExtensionsTest
    {
        [TestCase(new[] { 1, 2, 3 }, new int[] { }, new int[] { }, new[] { 1, 2, 3 })]
        [TestCase(new int[] { }, new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, new int[] { })]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, new int[] { }, new int[] { })]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4, 5 }, new[] { 4, 5 }, new int[] { })]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3 }, new int[] { }, new[] { 4, 5 })]
        [TestCase(new[] { 1, 2, 3 }, new[] { 4, 5 }, new[] { 4, 5 }, new[] { 1, 2, 3 })]
        public void TestBindArrayChanged(int[] oldValues, int[] newValues, int[] addedValues, int[] removeValues)
        {
            // initial default value.
            var bindableArray = new Bindable<int[]> { Value = oldValues };

            bindableArray.BindArrayChanged(n =>
            {
                Assert.AreEqual(n, addedValues);
            }, r =>
            {
                Assert.AreEqual(r, removeValues);
            });

            // apply changes
            bindableArray.Value = newValues;
        }
    }
}
