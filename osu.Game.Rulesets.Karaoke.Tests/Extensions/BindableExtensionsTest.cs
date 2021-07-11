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

            var addCount = 0;
            var removedCount = 0;

            bindableArray.BindArrayChanged(n =>
            {
                addCount++;
                Assert.AreEqual(n, addedValues);
            }, r =>
            {
                removedCount++;
                Assert.AreEqual(r, removeValues);
            });

            // apply changes
            bindableArray.Value = newValues;

            // should only run once.
            Assert.AreEqual(addCount, 1);
            Assert.AreEqual(removedCount, 1);
        }

        [TestCase(new[] { 1, 2, 3 }, new int[] { }, new int[] { })]
        [TestCase(new int[] { }, new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5 })]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        [TestCase(new[] { 1, 2, 3 }, new[] { 4, 5 }, new[] { 4, 5 })]
        public void TestBindArrayChangedIfRunImmediately(int[] oldValues, int[] newValues, int[] addedValues)
        {
            // initial default value.
            var bindableArray = new Bindable<int[]> { Value = oldValues };

            // apply changes
            bindableArray.Value = newValues;

            var addCount = 0;
            var removedCount = 0;

            bindableArray.BindArrayChanged(n =>
            {
                addCount++;
                Assert.AreEqual(n, addedValues);
            }, _ =>
            {
                removedCount++;
            }, true);

            // should only run once in add count event.
            Assert.AreEqual(addCount, 1);
            Assert.AreEqual(removedCount, 0);
        }
    }
}
