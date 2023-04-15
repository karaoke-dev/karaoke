// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects.Workings;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Workings;

public class LyricWorkingPropertyValidatorTest : HitObjectWorkingPropertyValidatorTest<Lyric, LyricWorkingProperty>
{
    [Test]
    public void TestPreemptTime()
    {
        var lyric = new Lyric();

        // state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.PreemptTime = 300);
        AssetIsValid(lyric, LyricWorkingProperty.PreemptTime, true);
    }

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

        // state is still valid because not assign all timing properties.
        Assert.DoesNotThrow(() => lyric.StartTime = 1000);
        AssetIsValid(lyric, LyricWorkingProperty.Timing, true);

        // ok, should be valid now.
        Assert.DoesNotThrow(() => lyric.Duration = 1000);
        AssetIsValid(lyric, LyricWorkingProperty.Timing, true);
    }

    [Test]
    public void TestSingers()
    {
        var lyric = new Lyric();

        // should be valid if singer is empty.
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<int>());
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should be invalid if assign the singer.
        Assert.DoesNotThrow(() => lyric.SingerIds.Add(1));
        AssetIsValid(lyric, LyricWorkingProperty.Singers, false);

        // should be valid again if remove the singer
        Assert.DoesNotThrow(() => lyric.SingerIds.Remove(1));
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should be matched if include all singers
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<int> { 1, 2, 3 });
        Assert.DoesNotThrow(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer(1), Array.Empty<SingerState>() },
            { new Singer(2), Array.Empty<SingerState>() },
            { new Singer(3), Array.Empty<SingerState>() }
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should be matched if include all singers
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<int> { 1, 2, 3 });
        Assert.DoesNotThrow(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer(1), new SingerState[] { new(2, 1), new(3, 1) } },
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works even id is not by order.
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<int> { 1, 2, 3 });
        Assert.DoesNotThrow(() => lyric.Singers = new Dictionary<Singer, SingerState[]>
        {
            { new Singer(3), Array.Empty<SingerState>() },
            { new Singer(2), Array.Empty<SingerState>() },
            { new Singer(1), Array.Empty<SingerState>() }
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works even id is not by order.
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<int> { 3, 2, 1 });
        Assert.DoesNotThrow(() => lyric.Singers = new System.Collections.Generic.Dictionary<Singer, SingerState[]>
        {
            { new Singer(1), Array.Empty<SingerState>() },
            { new Singer(2), Array.Empty<SingerState>() },
            { new Singer(3), Array.Empty<SingerState>() }
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works if id is duplicated
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<int> { 1, 1, 1 });
        Assert.DoesNotThrow(() => lyric.Singers = new System.Collections.Generic.Dictionary<Singer, SingerState[]>
        {
            { new Singer(1), Array.Empty<SingerState>() },
        });
        AssetIsValid(lyric, LyricWorkingProperty.Singers, true);

        // should works if id is duplicated
        Assert.DoesNotThrow(() => lyric.SingerIds = new List<int> { 1 });
        Assert.DoesNotThrow(() => lyric.Singers = new System.Collections.Generic.Dictionary<Singer, SingerState[]>
        {
            { new Singer(1), Array.Empty<SingerState>() },
            { new Singer(1), Array.Empty<SingerState>() },
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
            lyric.ReferenceLyricId = 1;
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
            lyric.ReferenceLyricId = 1;
            lyric.ReferenceLyric = new Lyric { ID = 1 };
        });
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should be invalid if change the reference lyric id.
        Assert.DoesNotThrow(() => lyric.ReferenceLyricId = 2);
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, false);

        // should be valid again if assign the reference lyric to the matched lyric.
        Assert.DoesNotThrow(() => lyric.ReferenceLyric = new Lyric { ID = 2 });
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should throw the exception if assign the working reference lyric to the unmatched reference lyric id.
        Assert.Throws<InvalidWorkingPropertyAssignException>(() => lyric.ReferenceLyric = new Lyric { ID = 3 });
        Assert.Throws<InvalidWorkingPropertyAssignException>(() => lyric.ReferenceLyric = null);
    }

    [Test]
    public void TestEffectApplier()
    {
        var lyric = new Lyric();

        // state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.EffectApplier = new LyricClassicStageEffectApplier(Array.Empty<StageElement>(), new ClassicStageDefinition()));
        AssetIsValid(lyric, LyricWorkingProperty.EffectApplier, true);
    }
}
