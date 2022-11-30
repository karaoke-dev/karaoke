// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Metadatas;

public class SingerInfoTest
{
    [Test]
    public void TestSingers()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var subSinger = singerInfo.AddSubSinger(singer);

        var allSingers = singerInfo.Singers;
        Assert.AreEqual(2, allSingers.Count);
        Assert.AreEqual(singer, allSingers[0]);
        Assert.AreEqual(subSinger, allSingers[1]);
    }

    [Test]
    public void TestGetAllSingers()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        singerInfo.AddSubSinger(singer);

        var allSingers = singerInfo.GetAllSingers().ToArray();
        Assert.AreEqual(1, allSingers.Length);
        Assert.AreEqual(singer, allSingers[0]);
    }

    [Test]
    public void TestGetAllAvailableSubSinger()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var subSinger = singerInfo.AddSubSinger(singer);

        var subSingers = singerInfo.GetAllAvailableSubSinger(singer).ToArray();
        Assert.AreEqual(1, subSingers.Length);
        Assert.AreEqual(subSinger, subSingers[0]);
    }

    [Test]
    public void TestGetSingerByIds()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var subSinger = singerInfo.AddSubSinger(singer);

        // the case if contains singer and sub-singer
        var singerMapWithSingerAndSubSinger = singerInfo.GetSingerByIds(new[] { singer.ID, subSinger.ID });
        Assert.AreEqual(1, singerMapWithSingerAndSubSinger.Count);
        Assert.AreEqual(singer, singerMapWithSingerAndSubSinger.Keys.FirstOrDefault());
        Assert.AreEqual(new[] { subSinger }, singerMapWithSingerAndSubSinger.Values.FirstOrDefault());

        // the case with main singer id.
        var singerMapWithSingerOnly = singerInfo.GetSingerByIds(new[] { singer.ID });
        Assert.AreEqual(1, singerMapWithSingerOnly.Count);
        Assert.AreEqual(singer, singerMapWithSingerOnly.Keys.FirstOrDefault());
        Assert.AreEqual(Array.Empty<SubSinger>(), singerMapWithSingerOnly.Values.FirstOrDefault());

        // the case with sub-singer only.
        // technically should not happened.
        var singerMap = singerInfo.GetSingerByIds(new[] { subSinger.ID });
        Assert.AreEqual(0, singerMap.Count);
    }

    [Test]
    public void TestGetSingerMap()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var subSinger = singerInfo.AddSubSinger(singer);

        var singerMap = singerInfo.GetSingerMap();
        Assert.AreEqual(1, singerMap.Count);
        Assert.AreEqual(singer, singerMap.Keys.FirstOrDefault());
        Assert.AreEqual(new[] { subSinger }, singerMap.Values.FirstOrDefault());
    }

    [Test]
    public void TestAddSinger()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();

        Assert.AreEqual(1, singerInfo.Singers.Count);
        Assert.AreEqual(1, singer.ID);
    }

    [Test]
    public void TestAddSubSinger()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var subSinger = singerInfo.AddSubSinger(singer);

        Assert.AreEqual(2, singerInfo.Singers.Count);
        Assert.AreEqual(2, subSinger.ID);
    }

    [Test]
    public void TestRemoveSinger()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var subSinger = singerInfo.AddSubSinger(singer);

        // should remove all the related sub-singer
        singerInfo.RemoveSinger(singer);
        Assert.AreEqual(0, singerInfo.Singers.Count);

        // should ignore the sub-singer
        singerInfo.RemoveSinger(subSinger);
        Assert.AreEqual(0, singerInfo.Singers.Count);
    }
}
