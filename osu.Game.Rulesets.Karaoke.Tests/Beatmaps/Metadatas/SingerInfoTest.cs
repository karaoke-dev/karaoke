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
        var singerState = singerInfo.AddSingerState(singer);

        Assert.That(singerInfo.Singers.Count, Is.EqualTo(1));
        Assert.That(singerInfo.SingerState.Count, Is.EqualTo(1));
        Assert.That(singerInfo.Singers[0], Is.EqualTo(singer));
        Assert.That(singerInfo.SingerState[0], Is.EqualTo(singerState));
    }

    [Test]
    public void TestGetAllSingers()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        singerInfo.AddSingerState(singer);

        var allSingers = singerInfo.GetAllSingers().ToArray();
        Assert.That(allSingers.Length, Is.EqualTo(1));
        Assert.That(allSingers[0], Is.EqualTo(singer));
    }

    [Test]
    public void TestGetAllAvailableSingerState()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var singerState = singerInfo.AddSingerState(singer);

        var singerStates = singerInfo.GetAllAvailableSingerStates(singer).ToArray();
        Assert.That(singerStates.Length, Is.EqualTo(1));
        Assert.That(singerStates[0], Is.EqualTo(singerState));
    }

    [Test]
    public void TestGetSingerByIds()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var singerState = singerInfo.AddSingerState(singer);

        // the case if contains singer and sub-singer
        var singerMapWithSingerAndStates = singerInfo.GetSingerByIds(new[] { singer.ID, singerState.ID });
        Assert.That(singerMapWithSingerAndStates.Count, Is.EqualTo(1));
        Assert.That(singerMapWithSingerAndStates.Keys.FirstOrDefault(), Is.EqualTo(singer));
        Assert.That(singerMapWithSingerAndStates.Values.FirstOrDefault(), Is.EqualTo(new[] { singerState }));

        // the case with main singer id.
        var singerMapWithSingerOnly = singerInfo.GetSingerByIds(new[] { singer.ID });
        Assert.That(singerMapWithSingerOnly.Count, Is.EqualTo(1));
        Assert.That(singerMapWithSingerOnly.Keys.FirstOrDefault(), Is.EqualTo(singer));
        Assert.That(singerMapWithSingerOnly.Values.FirstOrDefault(), Is.EqualTo(Array.Empty<SingerState>()));

        // the case with sub-singer only.
        // technically should not happened.
        var singerMap = singerInfo.GetSingerByIds(new[] { singerState.ID });
        Assert.That(singerMap.Count, Is.EqualTo(0));
    }

    [Test]
    public void TestGetSingerMap()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var singerState = singerInfo.AddSingerState(singer);

        var singerMap = singerInfo.GetSingerMap();
        Assert.That(singerMap.Count, Is.EqualTo(1));
        Assert.That(singerMap.Keys.FirstOrDefault(), Is.EqualTo(singer));
        Assert.That(singerMap.Values.FirstOrDefault(), Is.EqualTo(new[] { singerState }));
    }

    [Test]
    public void TestAddSinger()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();

        Assert.That(singerInfo.Singers.Count, Is.EqualTo(1));
        Assert.That(singer.ID.ToString(), Is.Not.Empty);
    }

    [Test]
    public void TestAddSingerState()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var singerState = singerInfo.AddSingerState(singer);

        Assert.That(singerInfo.Singers.Count, Is.EqualTo(1));
        Assert.That(singerInfo.SingerState.Count, Is.EqualTo(1));
        Assert.That(singerState.ID.ToString(), Is.Not.Empty);
        Assert.That(singerState.MainSingerId.ToString(), Is.Not.Empty);
    }

    [Test]
    public void TestRemoveSinger()
    {
        var singerInfo = new SingerInfo();
        var singer = singerInfo.AddSinger();
        var singerState = singerInfo.AddSingerState(singer);

        // should remove all the related sub-singer
        singerInfo.RemoveSinger(singer);
        Assert.That(singerInfo.Singers.Count, Is.EqualTo(0));

        // should ignore the sub-singer
        singerInfo.RemoveSinger(singerState);
        Assert.That(singerInfo.Singers.Count, Is.EqualTo(0));
    }
}
