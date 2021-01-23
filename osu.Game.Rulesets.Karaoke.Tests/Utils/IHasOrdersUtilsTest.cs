// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class IHasOrdersUtilsTest
    {
        [TestCase(new[] { 1, 2, 3, 4 }, false)]
        [TestCase(new[] { 1, 2, 3, 5 }, false)]
        [TestCase(new[] { 1, 2, 3, 3 }, true)]
        [TestCase(new[] { 1, 1, 1, 1 }, true)]
        [TestCase(new[] { -1, -2, -3, -4 }, false)] // should not include those ids but not check for now.
        public void TestContainDuplicatedId(int[] orders, bool containDuplicated)
        {
            var objects = orders.Select(x => new TestOrderObject { Order = x }).ToArray();
            var result = IHasOrdersUtils.ContainDuplicatedId(objects);
            Assert.AreEqual(result, containDuplicated);
        }

        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 })]
        [TestCase(new[] { 4, 3, 2, 1 }, new[] { 1, 2, 3, 4 })]
        [TestCase(new[] { 4, 4, 2, 2 }, new[] { 2, 2, 4, 4 })] // should not happen but still make a check.
        public void TestGetOrder(int[] orders, int[] actualOrders)
        {
            var objects = orders.Select(x => new TestOrderObject { Order = x });
            var orderedArray = IHasOrdersUtils.GetOrder(objects);
            var result = orderedArray.Select(x => x.Order).ToArray();
            Assert.AreEqual(result, actualOrders);
        }

        internal class TestOrderObject : IHasOrder
        {
            public int Order { get; set; }
        }
    }
}
