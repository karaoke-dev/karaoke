// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Tests.Extensions;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Workings;

public class LyricWorkingPropertyValidatorTest : HitObjectWorkingPropertyValidatorTest<Lyric, LyricWorkingProperty>
{
    [Test]
    public void TestSingers()
    {
        var lyric = new Lyric();

        var singerId1 = TestCaseElementIdHelper.CreateElementIdByNumber(1);
        var singerId2 = TestCaseElementIdHelper.CreateElementIdByNumber(2);
        var singerId3 = TestCaseElementIdHelper.CreateElementIdByNumber(3);

        // should be valid if singer is empty.
        Assert.That(() => lyric.SingerIds = new List<ElementId>(), Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should be invalid if assign the singer.
        Assert.That(() => lyric.SingerIds.Add(singerId1), Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Singers, false);

        // should be valid again if remove the singer
        Assert.That(() => lyric.SingerIds.Remove(singerId1), Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should be matched if include all singers
        Assert.That(() => lyric.SingerIds = new List<ElementId> { singerId1, singerId2, singerId3 }, Throws.Nothing);
        Assert.That(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId2), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId3), Array.Empty<SingerState>() },
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should be matched if include all singers
        Assert.That(() => lyric.SingerIds = new List<ElementId> { singerId1, singerId2, singerId3 }, Throws.Nothing);
        Assert.That(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), new[] { new SingerState(singerId1).ChangeId(singerId2), new SingerState(singerId1).ChangeId(singerId3) } },
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works even id is not by order.
        Assert.That(() => lyric.SingerIds = new List<ElementId> { singerId1, singerId2, singerId3 }, Throws.Nothing);
        Assert.That(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId3), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId2), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works even id is not by order.
        Assert.That(() => lyric.SingerIds = new List<ElementId> { singerId3, singerId2, singerId1 }, Throws.Nothing);
        Assert.That(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId2), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId3), Array.Empty<SingerState>() },
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works if id is duplicated
        Assert.That(() => lyric.SingerIds = new List<ElementId> { singerId1, singerId1, singerId1 }, Throws.Nothing);
        Assert.That(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works if id is duplicated
        Assert.That(() => lyric.SingerIds = new List<ElementId> { singerId1 }, Throws.Nothing);
        Assert.That(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);
    }

    [Test]
    public void TestPage()
    {
        var lyric = new Lyric();

        // page state is valid because assign the property.
        Assert.That(() => lyric.PageIndex = 1, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.Page, true);
    }

    [Test]
    public void TestReferenceLyric()
    {
        var lyric = new Lyric();

        // should be valid if change the reference lyric id.
        Assert.That(() =>
        {
            lyric.ReferenceLyricId = null;
            lyric.ReferenceLyric = null;
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should be invalid if change the reference lyric id.
        Assert.That(() =>
        {
            lyric.ReferenceLyricId = TestCaseElementIdHelper.CreateElementIdByNumber(1);
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, false);

        // should be valid again if change the id back.
        Assert.That(() =>
        {
            lyric.ReferenceLyricId = null;
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should be valid if change the reference lyric id.
        Assert.That(() =>
        {
            var referencedLyric = new Lyric();

            lyric.ReferenceLyricId = referencedLyric.ID;
            lyric.ReferenceLyric = referencedLyric;
        }, Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should be invalid if change the reference lyric id.
        Assert.That(() => lyric.ReferenceLyricId = TestCaseElementIdHelper.CreateElementIdByNumber(2), Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, false);

        // should be valid again if assign the reference lyric to the matched lyric.
        Assert.That(() => lyric.ReferenceLyric = new Lyric().ChangeId(2), Throws.Nothing);
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should throw the exception if assign the working reference lyric to the unmatched reference lyric id.
        Assert.That(() => lyric.ReferenceLyric = new Lyric().ChangeId(3), Throws.TypeOf<InvalidWorkingPropertyAssignException>());
        Assert.That(() => lyric.ReferenceLyric = null, Throws.TypeOf<InvalidWorkingPropertyAssignException>());
    }

    protected override bool IsInitialStateValid(LyricWorkingProperty flag)
    {
        return new LyricWorkingPropertyValidator(new Lyric()).IsValid(flag);
    }
}
