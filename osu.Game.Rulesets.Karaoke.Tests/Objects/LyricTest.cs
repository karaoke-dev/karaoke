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
            SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(new[] { 1, 2 }),
            Translations = new Dictionary<CultureInfo, string>
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

        Assert.That(clonedLyric.ID, Is.Not.SameAs(lyric.ID));

        Assert.That(clonedLyric.TextBindable, Is.Not.SameAs(lyric.TextBindable));
        Assert.That(clonedLyric.Text, Is.EqualTo(lyric.Text));

        Assert.That(clonedLyric.TimeTagsTimingVersion, Is.Not.SameAs(lyric.TimeTagsTimingVersion));
        Assert.That(clonedLyric.TimeTagsBindable, Is.Not.SameAs(lyric.TimeTagsBindable));
        TimeTagAssert.ArePropertyEqual(clonedLyric.TimeTags, lyric.TimeTags);

        Assert.That(clonedLyric.RubyTagsVersion, Is.Not.SameAs(lyric.RubyTagsVersion));
        Assert.That(clonedLyric.RubyTagsBindable, Is.Not.SameAs(lyric.RubyTagsBindable));
        RubyTagAssert.ArePropertyEqual(clonedLyric.RubyTags, lyric.RubyTags);

        Assert.That(clonedLyric.StartTimeBindable, Is.Not.SameAs(lyric.StartTimeBindable));
        Assert.That(clonedLyric.StartTime, Is.EqualTo(lyric.StartTime));

        Assert.That(clonedLyric.Duration, Is.EqualTo(lyric.Duration));

        Assert.That(clonedLyric.EndTime, Is.EqualTo(lyric.EndTime));

        Assert.That(clonedLyric.SingerIdsBindable, Is.Not.SameAs(lyric.SingerIdsBindable));
        Assert.That(clonedLyric.SingerIds, Is.EquivalentTo(lyric.SingerIds));

        Assert.That(clonedLyric.TranslationsBindable, Is.Not.SameAs(lyric.TranslationsBindable));
        Assert.That(clonedLyric.Translations, Is.EquivalentTo(lyric.Translations));

        Assert.That(clonedLyric.LanguageBindable, Is.Not.SameAs(lyric.LanguageBindable));
        Assert.That(clonedLyric.Language, Is.EqualTo(lyric.Language));

        Assert.That(clonedLyric.OrderBindable, Is.Not.SameAs(lyric.OrderBindable));
        Assert.That(clonedLyric.Order, Is.EqualTo(lyric.Order));

        Assert.That(clonedLyric.LockBindable, Is.Not.SameAs(lyric.LockBindable));
        Assert.That(clonedLyric.Lock, Is.EqualTo(lyric.Lock));

        Assert.That(clonedLyric.ReferenceLyricBindable, Is.Not.SameAs(lyric.ReferenceLyricBindable));
        Assert.That(clonedLyric.ReferenceLyric, Is.SameAs(lyric.ReferenceLyric));
        Assert.That(clonedLyric.ReferenceLyricId, Is.EqualTo(lyric.ReferenceLyricId));

        Assert.That(clonedLyric.ReferenceLyricConfigBindable, Is.Not.SameAs(lyric.ReferenceLyricConfigBindable));
        Assert.That(clonedLyric.ReferenceLyricConfig, Is.Not.SameAs(lyric.ReferenceLyricConfig));
        Assert.That(clonedLyric.ReferenceLyricConfig?.OffsetTime, Is.EqualTo(lyric.ReferenceLyricConfig?.OffsetTime));
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
            Translations = new Dictionary<CultureInfo, string>
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

        Assert.That(referencedLyric.Text, Is.EqualTo(lyric.Text));
        TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
        RubyTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);
        Assert.That(lyric.SingerIds, Is.EqualTo(referencedLyric.SingerIds));
        Assert.That(lyric.Translations, Is.EqualTo(referencedLyric.Translations));
        Assert.That(lyric.Language, Is.EqualTo(referencedLyric.Language));
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
        referencedLyric.Translations = new Dictionary<CultureInfo, string>
        {
            { new CultureInfo(17), "からおけ" },
        };
        referencedLyric.Language = new CultureInfo(17);

        Assert.That(lyric.Text, Is.EqualTo(referencedLyric.Text));
        TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
        RubyTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);
        Assert.That(lyric.SingerIds, Is.EqualTo(referencedLyric.SingerIds));
        Assert.That(lyric.Translations, Is.EqualTo(referencedLyric.Translations));
        Assert.That(lyric.Language, Is.EqualTo(referencedLyric.Language));
    }

    [Test]
    public void TestReferenceLyricListPropertyChanged()
    {
        // test modify property inside the list.
        // e.g. ruby, time-tag and romanisation.
        var timeTag = TestCaseTagHelper.ParseTimeTag("[0,start]:1100#^ka");
        var rubyTag = TestCaseTagHelper.ParseRubyTag("[0]:か");

        var referencedLyric = new Lyric
        {
            Text = "karaoke",
            TimeTags = new[] { timeTag },
            RubyTags = new[] { rubyTag },
            SingerIds = TestCaseElementIdHelper.CreateElementIdsByNumbers(new[] { 1 }),
            Translations = new Dictionary<CultureInfo, string>
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
        Assert.That(lyric.TimeTagsTimingVersion.Value, Is.EqualTo(0));
        Assert.That(lyric.TimeTagsRomanisationVersion.Value, Is.EqualTo(0));
        Assert.That(lyric.RubyTagsVersion.Value, Is.EqualTo(0));

        // it's time to change the property in the list.
        timeTag.Time = 2000;
        timeTag.RomanisedSyllable = "ka--";
        rubyTag.Text = "ruby";

        // property should be equal.
        TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
        RubyTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);

        // and note that because only one property is different, so version should change once.
        Assert.That(lyric.TimeTagsTimingVersion.Value, Is.EqualTo(1));
        Assert.That(lyric.TimeTagsRomanisationVersion.Value, Is.EqualTo(1));
        Assert.That(lyric.RubyTagsVersion.Value, Is.EqualTo(1));
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
            Translations = new Dictionary<CultureInfo, string>
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
        Assert.That(lyric.TimeTags, Is.Empty);
        Assert.That(lyric.SingerIds, Is.Not.EqualTo(referencedLyric.SingerIds));

        // it's time to open the config.
        config.SyncSingerProperty = true;
        config.SyncTimeTagProperty = true;

        // after open the config, the property should sync from the reference lyric now.
        TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
        Assert.That(lyric.SingerIds, Is.EqualTo(referencedLyric.SingerIds));
    }

    #endregion

    #region MyRegion

    [Test]
    public void TestLyricPropertyWritableVersion()
    {
        var lyric = new Lyric();
        Assert.That(lyric.LyricPropertyWritableVersion.Value, Is.EqualTo(0));

        lyric.Lock = LockState.Partial;
        Assert.That(lyric.LyricPropertyWritableVersion.Value, Is.EqualTo(1));

        var referencedLyric = new Lyric();
        lyric.ReferenceLyricId = referencedLyric.ID;
        lyric.ReferenceLyric = referencedLyric;
        Assert.That(lyric.LyricPropertyWritableVersion.Value, Is.EqualTo(2));

        lyric.ReferenceLyricConfig = new SyncLyricConfig();
        Assert.That(lyric.LyricPropertyWritableVersion.Value, Is.EqualTo(3));

        (lyric.ReferenceLyricConfig as SyncLyricConfig)!.OffsetTime = 200;
        Assert.That(lyric.LyricPropertyWritableVersion.Value, Is.EqualTo(4));

        // version number will not increase if change not related property or assign the same value.
        lyric.Lock = LockState.Partial;
        lyric.Text = "karaoke";
        Assert.That(lyric.LyricPropertyWritableVersion.Value, Is.EqualTo(4));
    }

    #endregion
}
