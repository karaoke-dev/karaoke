// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects;

public class LyricTest
{
    #region Clone

    [Test]
    public void TestClone()
    {
        var referencedLyric = new Lyric();

        var lyric = new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000^ka", "[1,start]:2000^ra", "[2,start]:3000^o", "[3,start]:4000^ke", "[3,end]:5000" }),
            RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }),
            StartTime = 1000,
            Duration = 4000,
            SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(new[] { 1, 2 }),
            Translates = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo("en-US"), "karaoke" },
            },
            Language = new CultureInfo("ja-JP"),
            Order = 1,
            Lock = LockState.None,
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = new ReferenceLyricConfig
            {
                OffsetTime = 100,
            },
        };

        var clonedLyric = lyric.DeepClone();

        Assert.AreNotSame(clonedLyric.ID, lyric.ID);

        Assert.AreNotSame(clonedLyric.TextBindable, lyric.TextBindable);
        Assert.AreEqual(clonedLyric.Text, lyric.Text);

        Assert.AreNotSame(clonedLyric.TimeTagsTimingVersion, lyric.TimeTagsTimingVersion);
        Assert.AreNotSame(clonedLyric.TimeTagsBindable, lyric.TimeTagsBindable);
        TimeTagAssert.ArePropertyEqual(clonedLyric.TimeTags, lyric.TimeTags);

        Assert.AreEqual(clonedLyric.LyricStartTime, lyric.LyricStartTime);
        Assert.AreEqual(clonedLyric.LyricEndTime, lyric.LyricEndTime);
        Assert.AreEqual(clonedLyric.LyricDuration, lyric.LyricDuration);

        Assert.AreNotSame(clonedLyric.RubyTagsVersion, lyric.RubyTagsVersion);
        Assert.AreNotSame(clonedLyric.RubyTagsBindable, lyric.RubyTagsBindable);
        RubyTagAssert.ArePropertyEqual(clonedLyric.RubyTags, lyric.RubyTags);

        Assert.AreNotSame(clonedLyric.StartTimeBindable, lyric.StartTimeBindable);
        Assert.AreEqual(clonedLyric.StartTime, lyric.StartTime);

        Assert.AreEqual(clonedLyric.Duration, lyric.Duration);

        Assert.AreNotSame(clonedLyric.SingerIdsBindable, lyric.SingerIdsBindable);
        CollectionAssert.AreEquivalent(clonedLyric.SingerIds, lyric.SingerIds);

        Assert.AreNotSame(clonedLyric.TranslateTextBindable, lyric.TranslateTextBindable);
        CollectionAssert.AreEquivalent(clonedLyric.Translates, lyric.Translates);

        Assert.AreNotSame(clonedLyric.LanguageBindable, lyric.LanguageBindable);
        Assert.AreEqual(clonedLyric.Language, lyric.Language);

        Assert.AreNotSame(clonedLyric.OrderBindable, lyric.OrderBindable);
        Assert.AreEqual(clonedLyric.Order, lyric.Order);

        Assert.AreNotSame(clonedLyric.LockBindable, lyric.LockBindable);
        Assert.AreEqual(clonedLyric.Lock, lyric.Lock);

        Assert.AreNotSame(clonedLyric.ReferenceLyricBindable, lyric.ReferenceLyricBindable);
        Assert.AreSame(clonedLyric.ReferenceLyric, lyric.ReferenceLyric);
        Assert.AreEqual(clonedLyric.ReferenceLyricId, lyric.ReferenceLyricId);

        Assert.AreNotSame(clonedLyric.ReferenceLyricConfigBindable, lyric.ReferenceLyricConfigBindable);
        Assert.AreNotSame(clonedLyric.ReferenceLyricConfig, lyric.ReferenceLyricConfig);
        Assert.AreEqual(clonedLyric.ReferenceLyricConfig?.OffsetTime, lyric.ReferenceLyricConfig?.OffsetTime);
    }

    #endregion

    #region Reference lyric

    [Test]
    public void TestSyncFromReferenceLyric()
    {
        var referencedLyric = new Lyric
        {
            Text = "karaoke",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1100#^ka" }),
            RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か" }),
            SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(new[] { 1 }),
            Translates = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo(17), "からおけ" },
            },
            Language = new CultureInfo(17),
        };

        var lyric = new Lyric
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = new SyncLyricConfig(),
        };

        Assert.AreEqual(referencedLyric.Text, lyric.Text);
        TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
        RubyTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);
        Assert.AreEqual(referencedLyric.SingerIds, lyric.SingerIds);
        Assert.AreEqual(referencedLyric.Translates, lyric.Translates);
        Assert.AreEqual(referencedLyric.Language, lyric.Language);
    }

    [Test]
    public void TestReferenceLyricPropertyChanged()
    {
        var referencedLyric = new Lyric();

        var lyric = new Lyric
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = new SyncLyricConfig(),
        };

        referencedLyric.Text = "karaoke";
        referencedLyric.TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1100^ka" });
        referencedLyric.RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か" });
        referencedLyric.SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(new[] { 1 });
        referencedLyric.Translates = new Dictionary<CultureInfo, string>
        {
            { new CultureInfo(17), "からおけ" },
        };
        referencedLyric.Language = new CultureInfo(17);

        Assert.AreEqual(referencedLyric.Text, lyric.Text);
        TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
        RubyTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);
        Assert.AreEqual(referencedLyric.SingerIds, lyric.SingerIds);
        Assert.AreEqual(referencedLyric.Translates, lyric.Translates);
        Assert.AreEqual(referencedLyric.Language, lyric.Language);
    }

    [Test]
    public void TestReferenceLyricListPropertyChanged()
    {
        // test modify property inside the list.
        // ruby, romaji tag time-tag.
        var timeTag = TestCaseTagHelper.ParseTimeTag("[0,start]:1100#^ka");
        var rubyTag = TestCaseTagHelper.ParseRubyTag("[0]:か");

        var referencedLyric = new Lyric
        {
            Text = "karaoke",
            TimeTags = new[] { timeTag },
            RubyTags = new[] { rubyTag },
            SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(new[] { 1 }),
            Translates = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo(17), "からおけ" },
            },
            Language = new CultureInfo(17),
        };

        var lyric = new Lyric
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = new SyncLyricConfig(),
        };

        // property should be the same
        TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
        RubyTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);

        // and because there's no change inside the tag, so there's version change.
        Assert.AreEqual(0, lyric.TimeTagsTimingVersion.Value);
        Assert.AreEqual(0, lyric.TimeTagsRomanisationVersion.Value);
        Assert.AreEqual(0, lyric.RubyTagsVersion.Value);

        // it's time to change the property in the list.
        timeTag.Time = 2000;
        timeTag.RomanisedSyllable = "romaji";
        rubyTag.Text = "ruby";

        // property should be equal.
        TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
        RubyTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);

        // and note that because only one property is different, so version should change once.
        Assert.AreEqual(1, lyric.TimeTagsTimingVersion.Value);
        Assert.AreEqual(1, lyric.TimeTagsRomanisationVersion.Value);
        Assert.AreEqual(1, lyric.RubyTagsVersion.Value);
    }

    [Test]
    public void TestConfigChange()
    {
        // change the config from reference lyric into sync lyric config.
        // than, should auto sync the value.
        var config = new SyncLyricConfig
        {
            SyncSingerProperty = false,
            SyncTimeTagProperty = false,
        };

        var referencedLyric = new Lyric
        {
            Text = "karaoke",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1100#ka" }),
            RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か" }),
            SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(new[] { 1 }),
            Translates = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo(17), "からおけ" },
            },
            Language = new CultureInfo(17),
        };

        var lyric = new Lyric
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = config,
        };

        // the property should not same as the reference reference because those properties are not sync.
        Assert.IsEmpty(lyric.TimeTags);
        Assert.AreNotEqual(referencedLyric.SingerIds, lyric.SingerIds);

        // it's time to open the config.
        config.SyncSingerProperty = true;
        config.SyncTimeTagProperty = true;

        // after open the config, the property should sync from the reference lyric now.
        TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
        Assert.AreEqual(referencedLyric.SingerIds, lyric.SingerIds);
    }

    #endregion

    #region MyRegion

    [Test]
    public void TestLyricPropertyWritableVersion()
    {
        var lyric = new Lyric();
        Assert.AreEqual(0, lyric.LyricPropertyWritableVersion.Value);

        lyric.Lock = LockState.Partial;
        Assert.AreEqual(1, lyric.LyricPropertyWritableVersion.Value);

        var referencedLyric = new Lyric();
        lyric.ReferenceLyricId = referencedLyric.ID;
        lyric.ReferenceLyric = referencedLyric;
        Assert.AreEqual(2, lyric.LyricPropertyWritableVersion.Value);

        lyric.ReferenceLyricConfig = new SyncLyricConfig();
        Assert.AreEqual(3, lyric.LyricPropertyWritableVersion.Value);

        (lyric.ReferenceLyricConfig as SyncLyricConfig)!.OffsetTime = 200;
        Assert.AreEqual(4, lyric.LyricPropertyWritableVersion.Value);

        // version number will not increase if change not related property or assign the same value.
        lyric.Lock = LockState.Partial;
        lyric.Text = "karaoke";
        Assert.AreEqual(4, lyric.LyricPropertyWritableVersion.Value);
    }

    #endregion
}
