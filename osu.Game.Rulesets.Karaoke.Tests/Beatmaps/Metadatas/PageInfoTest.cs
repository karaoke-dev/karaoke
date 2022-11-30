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
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 4001, 4000)]
    [TestCase(new double[] { }, 0, null)]
    [TestCase(new double[] { 4000, 3000, 2000, 1000 }, 1000, 1000)] // should works even not sorting.
    public void TestGetPageAt(double[] times, double time, double? expectedTime)
    {
        var pageInfo = new PageInfo();
        pageInfo.Pages.AddRange(getPages(times));

        var actualPage = pageInfo.GetPageAt(time);

        Assert.AreEqual(expectedTime, actualPage?.Time);
    }

    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 999, null)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 1000, 0)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 1001, 0)]
    [TestCase(new double[] { 1000, 2000, 3000, 4000 }, 4001, 3)]
    [TestCase(new double[] { }, 0, null)]
    [TestCase(new double[] { 4000, 3000, 2000, 1000 }, 1000, 0)] // should works even not sorting.
    public void TestGetPageIndexAt(double[] times, double time, int? expectedIndex)
    {
        var pageInfo = new PageInfo();
        pageInfo.Pages.AddRange(getPages(times));

        int? actualPageIndex = pageInfo.GetPageIndexAt(time);

        Assert.AreEqual(expectedIndex, actualPageIndex);
    }

    private static IEnumerable<Page> getPages(double[] pages)
        => pages.Select(x => new Page { Time = x });
}
