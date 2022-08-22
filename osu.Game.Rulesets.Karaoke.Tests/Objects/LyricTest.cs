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
            Assert.AreEqual(clonedLyric.ReferenceLyricConfig.OffsetTime, lyric.ReferenceLyricConfig.OffsetTime);
        }
    }
}
