// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class BindablesUtilsTest
    {
        [TestCase(null, null, new[] { 1 }, null, new[] { 1 }, new[] { 1 })]
        [TestCase(null, null, null, new[] { 1 }, new[] { 1 }, new[] { 1 })]
        [TestCase(null, null, new[] { 1 }, new[] { 2 }, new[] { 1, 2 }, new[] { 1, 2 })]
        [TestCase(new[] { 1 }, new[] { 2 }, null, null, new[] { 1, 2 }, new[] { 2, 1 })]
        [TestCase(new[] { 1 }, new[] { 2 }, new[] { 3 }, null, new[] { 1, 2, 3 }, new[] { 2, 1, 3 })]
        [TestCase(new[] { 1 }, new[] { 2 }, null, new[] { 3 }, new[] { 1, 2, 3 }, new[] { 2, 1, 3 })]
        public void TestSyncWithAddItems(int[] firstDefaultValues, int[] secondDefaultValues, int[] firstNewValues, int[] secondNewValues, int[] actualFirstValues, int[] actualSecondValues)
        {
            var firstBindableList = new BindableList<int>(firstDefaultValues);
            var secondBindableList = new BindableList<int>(secondDefaultValues);

            BindablesUtils.Sync(firstBindableList, secondBindableList);

            if (firstNewValues != null)
                firstBindableList.AddRange(firstNewValues);

            if (secondNewValues != null)
                secondBindableList.AddRange(secondNewValues);

            Assert.AreEqual(firstBindableList.ToArray(), actualFirstValues);
            Assert.AreEqual(secondBindableList.ToArray(), actualSecondValues);
        }

        [TestCase(null, null, new[] { 1 }, null, new int[] { }, new int[] { })] // should not clear if has no values.
        [TestCase(null, null, null, new[] { 1 }, new int[] { }, new int[] { })]
        [TestCase(null, null, new[] { 1 }, new[] { 2 }, new int[] { }, new int[] { })]
        [TestCase(new[] { 1 }, new[] { 2 }, new[] { 1 }, null, new[] { 2 }, new[] { 2 })] // matched value should be clear
        [TestCase(new[] { 1 }, new[] { 2 }, null, new[] { 1 }, new[] { 2 }, new[] { 2 })]
        [TestCase(new[] { 1 }, new[] { 2 }, new[] { 1 }, new[] { 2 }, new int[] { }, new int[] { })] // all value should be clear
        [TestCase(new[] { 1 }, new[] { 2 }, new[] { 2 }, new[] { 1 }, new int[] { }, new int[] { })]
        [TestCase(new[] { 1 }, new[] { 2 }, new[] { 3 }, null, new[] { 1, 2 }, new[] { 2, 1 })] // should not clear value if not contains.
        [TestCase(new[] { 1 }, new[] { 2 }, null, new[] { 3 }, new[] { 1, 2 }, new[] { 2, 1 })]
        public void TestSyncWithRemoveItems(int[] firstDefaultValues, int[] secondDefaultValues, int[] firstRemoveValues, int[] secondRemoveValues, int[] actualFirstValues, int[] actualSecondValues)
        {
            var firstBindableList = new BindableList<int>(firstDefaultValues);
            var secondBindableList = new BindableList<int>(secondDefaultValues);

            BindablesUtils.Sync(firstBindableList, secondBindableList);

            if (firstRemoveValues != null)
                firstBindableList.RemoveAll(firstRemoveValues.Contains);

            if (secondRemoveValues != null)
                secondBindableList.RemoveAll(secondRemoveValues.Contains);

            Assert.AreEqual(firstBindableList.ToArray(), actualFirstValues);
            Assert.AreEqual(secondBindableList.ToArray(), actualSecondValues);
        }

        [TestCase(new[] { 1 }, null, null, new[] { 1 })] // should sync default value also.
        [TestCase(new[] { 1 }, null, new[] { 2 }, new[] { 1, 2 })]
        [TestCase(null, null, new[] { 2 }, new[] { 2 })]
        [TestCase(new[] { 1 }, new[] { 1 }, null, new[] { 1 })]
        [TestCase(new[] { 1, 2 }, new[] { 1 }, null, new[] { 1, 2 })]
        [TestCase(new[] { 1, 1 }, new[] { 1 }, null, new[] { 1 })]
        [TestCase(new[] { 1, 1 }, new[] { 2 }, null, new[] { 2, 1 })] // it's ok if has duplicated value in default value.
        [TestCase(new[] { 1 }, new[] { 1 }, new[] { 1 }, new[] { 1 })] // should not sync to second list if add the same value.
        public void TestOnyWaySyncWithAddItems(int[] defaultValuesInFirstBindable, int[] defaultValuesInSecondBindable, int[] newValues, int[] actualValuesInSecondBindable)
        {
            var firstBindableList = new BindableList<int>(defaultValuesInFirstBindable);
            var secondBindableList = new BindableList<int>(defaultValuesInSecondBindable);

            BindablesUtils.OnyWaySync(firstBindableList, secondBindableList);

            if (newValues != null)
                firstBindableList.AddRange(newValues);

            Assert.AreEqual(secondBindableList.ToArray(), actualValuesInSecondBindable);
        }

        [TestCase(new[] { 1, 2, 3 }, null, new[] { 1 }, new[] { 2, 3 })]
        [TestCase(new[] { 1 }, null, new[] { 1 }, new int[] { })] // remove all values
        [TestCase(null, null, new[] { 2 }, new int[] { })] // remove value that is not exist.
        [TestCase(new[] { 1, 2 }, new[] { 3 }, new[] { 3 }, new[] { 3, 1, 2 })] // cannot remove value from second list if remove value is not in the first list.
        public void TestOnyWaySyncWithRemoveItems(int[] defaultValuesInFirstBindable, int[] defaultValuesInSecondBindable, int[] removeValues, int[] actualValuesInSecondBindable)
        {
            var firstBindableList = new BindableList<int>(defaultValuesInFirstBindable);
            var secondBindableList = new BindableList<int>(defaultValuesInSecondBindable);

            BindablesUtils.OnyWaySync(firstBindableList, secondBindableList);

            if (removeValues != null)
                firstBindableList.RemoveAll(removeValues.Contains);

            Assert.AreEqual(secondBindableList.ToArray(), actualValuesInSecondBindable);
        }
    }
}
