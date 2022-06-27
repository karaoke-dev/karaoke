// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricTranslateChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricTranslateChangeHandler, Lyric>
    {
        [Test]
        public void TestUpdateTranslateWithNewLanguage()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
            });

            TriggerHandlerChanged(c => c.UpdateTranslate(new CultureInfo(17), "からおけ"));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(1, h.Translates.Count);
                Assert.AreEqual("からおけ", h.Translates[new CultureInfo(17)]);
            });
        }

        [Test]
        public void TestUpdateTranslateWithExistLanguage()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Translates = new Dictionary<CultureInfo, string>
                {
                    { new CultureInfo(17), "からおけ" }
                }
            });

            TriggerHandlerChanged(c => c.UpdateTranslate(new CultureInfo(17), "karaoke"));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(1, h.Translates.Count);
                Assert.AreEqual("karaoke", h.Translates[new CultureInfo(17)]);
            });
        }

        [Test]
        public void TestUpdateTranslateWithEmptyText()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Translates = new Dictionary<CultureInfo, string>
                {
                    { new CultureInfo(17), "からおけ" }
                }
            });

            TriggerHandlerChanged(c => c.UpdateTranslate(new CultureInfo(17), string.Empty));

            AssertSelectedHitObject(h =>
            {
                Assert.IsEmpty(h.Translates);
            });
        }

        [Test]
        public void TestUpdateTranslateWithNullText()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Translates = new Dictionary<CultureInfo, string>
                {
                    { new CultureInfo(17), "からおけ" }
                }
            });

            TriggerHandlerChanged(c => c.UpdateTranslate(new CultureInfo(17), ""));

            AssertSelectedHitObject(h =>
            {
                Assert.IsEmpty(h.Translates);
            });
        }
    }
}
