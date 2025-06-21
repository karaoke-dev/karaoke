// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Metadatas;

public class PageInfoTest
{
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 999, null)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 1000, 1000)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 1001, 1000)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 4000, 4000)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 4001, null)]
    [TestCase(new double[] { }, 0, null)]
    [TestCase(new double[] { 1000 }, 999, null)] // should be able to get the time only if time is between two pages.
    [TestCase(new double[] { 1000 }, 1000, null)]
    [TestCase(new double[] { 1000 }, 1002, null)]
    [TestCase(new double[] { 4000, 3000, 2000, 1000 }, 1000, 1000)] // should works even not sorting.
    public void TestGetPageAt(double[] times, double time, double? expectedTime)
    {
        var pageInfo = new PageInfo();
        pageInfo.Pages.AddRange(createPages(times));

        var actualPage = pageInfo.GetPageAt(time);

        Assert.That(actualPage?.Time, Is.EqualTo(expectedTime));
    }

    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 999, null)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 1000, 0)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 1001, 0)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 4000, 3)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 4001, null)]
    [TestCase(new double[] { }, 0, null)]
    [TestCase(new double[] { 1000 }, 999, null)] // should be able to get the time only if time is between two pages.
    [TestCase(new double[] { 1000 }, 1000, null)]
    [TestCase(new double[] { 1000 }, 1002, null)]
    [TestCase(new double[] { 4000, 3000, 2000, 1000 }, 1000, 0)] // should works even not sorting.
    public void TestGetPageIndexAt(double[] times, double time, int? expectedIndex)
    {
        var pageInfo = new PageInfo();
        pageInfo.Pages.AddRange(createPages(times));

        int? actualPageIndex = pageInfo.GetPageIndexAt(time);

        Assert.That(actualPageIndex, Is.EqualTo(expectedIndex));
    }

    [Test]
    public void TestGetPageOrder()
    {
        var pageInfo = new PageInfo();
        pageInfo.Pages.AddRange(createPages(new double[] { 1000 }));

        var existPage = pageInfo.Pages.First();
        int? existPageOrder = pageInfo.GetPageOrder(existPage);
        Assert.That(existPageOrder, Is.EqualTo(1));

        var notExistPage = new Page { Time = 1000 };
        int? notExistPageOrder = pageInfo.GetPageOrder(notExistPage);
        Assert.That(notExistPageOrder, Is.Null);
    }

    private static IEnumerable<Page> createPages(IEnumerable<double> times)
        => times.Select(x => new Page { Time = x });
}
