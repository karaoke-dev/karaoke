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

namespace osu.Game.Rulesets.Karaoke.Tests.Objects
{
    public class LyricTest
    {
        #region Clone

        [TestCase]
        public void TestClone()
        {
            var referencedLyric = new Lyric();

            var lyric = new Lyric
            {
                ID = 1,
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", "[3,end]:5000" }),
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }),
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[] { "[0,2]:ka", "[2,4]:ra", "[4,5]:o", "[5,7]:ke" }),
                StartTime = 1000,
                Duration = 4000,
                Singers = new[] { 1, 2 },
                Translates = new Dictionary<CultureInfo, string>
                {
                    { new CultureInfo("en-US"), "karaoke" }
                },
                Language = new CultureInfo("ja-JP"),
                Order = 1,
                Lock = LockState.None,
                ReferenceLyric = referencedLyric,
                ReferenceLyricConfig = new ReferenceLyricConfig
                {
                    OffsetTime = 100
                }
            };

            var clonedLyric = lyric.DeepClone();

            Assert.AreEqual(clonedLyric.ID, lyric.ID);

            Assert.AreNotSame(clonedLyric.TextBindable, lyric.TextBindable);
            Assert.AreEqual(clonedLyric.Text, lyric.Text);

            Assert.AreNotSame(clonedLyric.TimeTagsVersion, lyric.TimeTagsVersion);
            Assert.AreNotSame(clonedLyric.TimeTagsBindable, lyric.TimeTagsBindable);
            TimeTagAssert.ArePropertyEqual(clonedLyric.TimeTags, lyric.TimeTags);

            Assert.AreNotSame(clonedLyric.RubyTagsVersion, lyric.RubyTagsVersion);
            Assert.AreNotSame(clonedLyric.RubyTagsBindable, lyric.RubyTagsBindable);
            TextTagAssert.ArePropertyEqual(clonedLyric.RubyTags, lyric.RubyTags);

            Assert.AreNotSame(clonedLyric.RomajiTagsVersion, lyric.RomajiTagsVersion);
            Assert.AreNotSame(clonedLyric.RomajiTagsBindable, lyric.RomajiTagsBindable);
            TextTagAssert.ArePropertyEqual(clonedLyric.RomajiTags, lyric.RomajiTags);

            Assert.AreNotSame(clonedLyric.StartTimeBindable, lyric.StartTimeBindable);
            Assert.AreEqual(clonedLyric.StartTime, lyric.StartTime);

            Assert.AreEqual(clonedLyric.Duration, lyric.Duration);

            Assert.AreNotSame(clonedLyric.SingersBindable, lyric.SingersBindable);
            CollectionAssert.AreEquivalent(clonedLyric.Singers, lyric.Singers);

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
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1100" }),
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0,1]:か" }),
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[] { "[0,1]:ka" }),
                Singers = new[] { 1 },
                Translates = new Dictionary<CultureInfo, string>
                {
                    { new CultureInfo(17), "からおけ" }
                },
                Language = new CultureInfo(17)
            };

            var lyric = new Lyric
            {
                ReferenceLyric = referencedLyric,
                ReferenceLyricConfig = new SyncLyricConfig(),
            };

            Assert.AreEqual(referencedLyric.Text, lyric.Text);
            TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
            TextTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);
            TextTagAssert.ArePropertyEqual(referencedLyric.RomajiTags, lyric.RomajiTags);
            Assert.AreEqual(referencedLyric.Singers, lyric.Singers);
            Assert.AreEqual(referencedLyric.Translates, lyric.Translates);
            Assert.AreEqual(referencedLyric.Language, lyric.Language);
        }

        [Test]
        public void TestReferenceLyricPropertyChanged()
        {
            var referencedLyric = new Lyric();

            var lyric = new Lyric
            {
                ReferenceLyric = referencedLyric,
                ReferenceLyricConfig = new SyncLyricConfig(),
            };

            referencedLyric.Text = "karaoke";
            referencedLyric.TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1100" });
            referencedLyric.RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0,1]:か" });
            referencedLyric.RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[] { "[0,1]:ka" });
            referencedLyric.Singers = new[] { 1 };
            referencedLyric.Translates = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo(17), "からおけ" }
            };
            referencedLyric.Language = new CultureInfo(17);

            Assert.AreEqual(referencedLyric.Text, lyric.Text);
            TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
            TextTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);
            TextTagAssert.ArePropertyEqual(referencedLyric.RomajiTags, lyric.RomajiTags);
            Assert.AreEqual(referencedLyric.Singers, lyric.Singers);
            Assert.AreEqual(referencedLyric.Translates, lyric.Translates);
            Assert.AreEqual(referencedLyric.Language, lyric.Language);
        }

        [Test]
        public void TestReferenceLyricListPropertyChanged()
        {
            // test modify property inside the list.
            // ruby, romaji tag time-tag.
            var timeTag = TestCaseTagHelper.ParseTimeTag("[0,start]:1100");
            var rubyTag = TestCaseTagHelper.ParseRubyTag("[0,1]:か");
            var romajiTag = TestCaseTagHelper.ParseRomajiTag("[0,1]:ka");

            var referencedLyric = new Lyric
            {
                Text = "karaoke",
                TimeTags = new[] { timeTag },
                RubyTags = new[] { rubyTag },
                RomajiTags = new[] { romajiTag },
                Singers = new[] { 1 },
                Translates = new Dictionary<CultureInfo, string>
                {
                    { new CultureInfo(17), "からおけ" }
                },
                Language = new CultureInfo(17)
            };

            var lyric = new Lyric
            {
                ReferenceLyric = referencedLyric,
                ReferenceLyricConfig = new SyncLyricConfig(),
            };

            // property should be the same
            TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
            TextTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);
            TextTagAssert.ArePropertyEqual(referencedLyric.RomajiTags, lyric.RomajiTags);

            // and because there's no change inside the tag, so there's version change.
            Assert.AreEqual(0, lyric.TimeTagsVersion.Value);
            Assert.AreEqual(0, lyric.RubyTagsVersion.Value);
            Assert.AreEqual(0, lyric.RomajiTagsVersion.Value);

            // it's time to change the property in the list.
            timeTag.Time = 2000;
            rubyTag.Text = "ruby";
            romajiTag.Text = "romaji";

            // property should be equal.
            TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
            TextTagAssert.ArePropertyEqual(referencedLyric.RubyTags, lyric.RubyTags);
            TextTagAssert.ArePropertyEqual(referencedLyric.RomajiTags, lyric.RomajiTags);

            // and note that because only one property is different, so version should change once.
            Assert.AreEqual(1, lyric.TimeTagsVersion.Value);
            Assert.AreEqual(1, lyric.RubyTagsVersion.Value);
            Assert.AreEqual(1, lyric.RomajiTagsVersion.Value);
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
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1100" }),
                RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0,1]:か" }),
                RomajiTags = TestCaseTagHelper.ParseRomajiTags(new[] { "[0,1]:ka" }),
                Singers = new[] { 1 },
                Translates = new Dictionary<CultureInfo, string>
                {
                    { new CultureInfo(17), "からおけ" }
                },
                Language = new CultureInfo(17)
            };

            var lyric = new Lyric
            {
                ReferenceLyric = referencedLyric,
                ReferenceLyricConfig = config
            };

            // the property should not same as the reference reference because those properties are not sync.
            Assert.IsEmpty(lyric.TimeTags);
            Assert.AreNotEqual(referencedLyric.Singers, lyric.Singers);

            // it's time to open the config.
            config.SyncSingerProperty = true;
            config.SyncTimeTagProperty = true;

            // after open the config, the property should sync from the reference lyric now.
            TimeTagAssert.ArePropertyEqual(referencedLyric.TimeTags, lyric.TimeTags);
            Assert.AreEqual(referencedLyric.Singers, lyric.Singers);
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

            lyric.ReferenceLyric = new Lyric();
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
}
