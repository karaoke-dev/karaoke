// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
using osu.Game.Rulesets.Karaoke.Tests.Extensions;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Workings;

public class LyricWorkingPropertyValidatorTest : HitObjectWorkingPropertyValidatorTest<Lyric, LyricWorkingProperty>
{
    [Test]
    public void TestStartTime()
    {
        var lyric = new Lyric();

        // state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.StartTime = 1000);
        AssetIsValid(lyric, LyricWorkingProperty.StartTime, true);
    }

    [Test]
    public void TestDuration()
    {
        var lyric = new Lyric();

        // state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.Duration = 1000);
        AssetIsValid(lyric, LyricWorkingProperty.Duration, true);
    }

    [Test]
    public void TestTiming()
    {
        var lyric = new Lyric();

        // state is still invalid because duration is not assign.
        Assert.DoesNotThrow(() => lyric.StartTime = 1000);
        AssetIsValid(lyric, LyricWorkingProperty.Timing, false);

        // ok, should be valid now.
        Assert.DoesNotThrow(() => lyric.Duration = 1000);
        AssetIsValid(lyric, LyricWorkingProperty.Timing, true);
    }

    [Test]
    public void TestSingers()
    {
        var lyric = new Lyric();

        var singerId1 = TestCaseElementIdHelper.CreateElementIdByNumber(1);
        var singerId2 = TestCaseElementIdHelper.CreateElementIdByNumber(2);
        var singerId3 = TestCaseElementIdHelper.CreateElementIdByNumber(3);

        // should be valid if singer is empty.
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<ElementId>());
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should be invalid if assign the singer.
        Assert.DoesNotThrow(() => lyric.SingerIds.Add(singerId1));
        AssetIsValid(lyric, LyricWorkingProperty.Singers, false);

        // should be valid again if remove the singer
        Assert.DoesNotThrow(() => lyric.SingerIds.Remove(singerId1));
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should be matched if include all singers
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<ElementId> { singerId1, singerId2, singerId3 });
        Assert.DoesNotThrow(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId2), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId3), Array.Empty<SingerState>() },
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should be matched if include all singers
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<ElementId> { singerId1, singerId2, singerId3 });
        Assert.DoesNotThrow(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), new[] { new SingerState(singerId1).ChangeId(singerId2), new SingerState(singerId1).ChangeId(singerId3) } },
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works even id is not by order.
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<ElementId> { singerId1, singerId2, singerId3 });
        Assert.DoesNotThrow(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId3), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId2), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works even id is not by order.
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<ElementId> { singerId3, singerId2, singerId1 });
        Assert.DoesNotThrow(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId2), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId3), Array.Empty<SingerState>() },
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works if id is duplicated
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<ElementId> { singerId1, singerId1, singerId1 });
        Assert.DoesNotThrow(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works if id is duplicated
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<ElementId> { singerId1 });
        Assert.DoesNotThrow(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
            { new Singer().ChangeId(singerId1), Array.Empty<SingerState>() },
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);
    }

    [Test]
    public void TestPage()
    {
        var lyric = new Lyric();

        // page state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.PageIndex = 1);
        AssetIsValid(lyric, LyricWorkingProperty.Page, true);
    }

    [Test]
    public void TestReferenceLyric()
    {
        var lyric = new Lyric();

        // should be valid if change the reference lyric id.
        Assert.DoesNotThrow(() =>
        {
            lyric.ReferenceLyricId = null;
            lyric.ReferenceLyric = null;
        });
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should be invalid if change the reference lyric id.
        Assert.DoesNotThrow(() =>
        {
            lyric.ReferenceLyricId = TestCaseElementIdHelper.CreateElementIdByNumber(1);
        });
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, false);

        // should be valid again if change the id back.
        Assert.DoesNotThrow(() =>
        {
            lyric.ReferenceLyricId = null;
        });
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should be valid if change the reference lyric id.
        Assert.DoesNotThrow(() =>
        {
            var referencedLyric = new Lyric();

            lyric.ReferenceLyricId = referencedLyric.ID;
            lyric.ReferenceLyric = referencedLyric;
        });
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should be invalid if change the reference lyric id.
        Assert.DoesNotThrow(() => lyric.ReferenceLyricId = TestCaseElementIdHelper.CreateElementIdByNumber(2));
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, false);

        // should be valid again if assign the reference lyric to the matched lyric.
        Assert.DoesNotThrow(() => lyric.ReferenceLyric = new Lyric().ChangeId(2));
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should throw the exception if assign the working reference lyric to the unmatched reference lyric id.
        Assert.Throws<InvalidWorkingPropertyAssignException>(() => lyric.ReferenceLyric = new Lyric().ChangeId(3));
        Assert.Throws<InvalidWorkingPropertyAssignException>(() => lyric.ReferenceLyric = null);
    }

    [Test]
    public void TestCommandGenerator()
    {
        var lyric = new Lyric();

        // state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.CommandGenerator = new ClassicLyricCommandGenerator(new ClassicStageInfo()));
        AssetIsValid(lyric, LyricWorkingProperty.CommandGenerator, true);
    }

    protected override bool IsInitialStateValid(LyricWorkingProperty flag)
    {
        return new LyricWorkingPropertyValidator(new Lyric()).IsValid(flag);
    }
}
