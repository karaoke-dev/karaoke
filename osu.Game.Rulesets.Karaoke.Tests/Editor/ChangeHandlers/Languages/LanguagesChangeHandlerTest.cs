// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Languages
{
    public class LanguagesChangeHandlerTest : BaseChangeHandlerTest<LanguagesChangeHandler>
    {
        [Test]
        public void TestAdd()
        {
            SetUpKaraokeBeatmap(karaokeBeatmap =>
            {
                karaokeBeatmap.AvailableTranslates = new List<CultureInfo>
                {
                    new("zh-TW")
                };
            });

            TriggerHandlerChanged(c =>
            {
                c.Add(new CultureInfo("Ja-jp"));
            });

            AssertKaraokeBeatmap(karaokeBeatmap =>
            {
                Assert.AreEqual(2, karaokeBeatmap.AvailableTranslates.Count);
                Assert.AreEqual(new CultureInfo("zh-TW"), karaokeBeatmap.AvailableTranslates[0]);
                Assert.AreEqual(new CultureInfo("Ja-jp"), karaokeBeatmap.AvailableTranslates[1]);
            });
        }

        [Test]
        public void TestRemove()
        {
            SetUpKaraokeBeatmap(karaokeBeatmap =>
            {
                karaokeBeatmap.AvailableTranslates = new List<CultureInfo>
                {
                    new("zh-TW"),
                    new("Ja-jp")
                };
            });

            TriggerHandlerChanged(c =>
            {
                c.Remove(new CultureInfo("Ja-jp"));
            });

            AssertKaraokeBeatmap(karaokeBeatmap =>
            {
                Assert.AreEqual(1, karaokeBeatmap.AvailableTranslates.Count);
                Assert.AreEqual(new CultureInfo("zh-TW"), karaokeBeatmap.AvailableTranslates[0]);
            });
        }

        [Test]
        public void TestIsLanguageContainsTranslate()
        {
            SetUpKaraokeBeatmap(karaokeBeatmap =>
            {
                karaokeBeatmap.AvailableTranslates = new List<CultureInfo>
                {
                    new("zh-TW"),
                    new("Ja-jp")
                };

                karaokeBeatmap.HitObjects = new List<KaraokeHitObject>
                {
                    new Lyric
                    {
                        Translates = new Dictionary<CultureInfo, string>
                        {
                            {
                                new("zh-TW"), "卡拉 OK"
                            }
                        }
                    }
                };
            });

            TriggerHandlerChanged(c =>
            {
                Assert.AreEqual(false, c.IsLanguageContainsTranslate(new CultureInfo("Ja-jp")));
                Assert.AreEqual(true, c.IsLanguageContainsTranslate(new CultureInfo("zh-TW")));
            });
        }
    }
}
