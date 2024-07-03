// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public partial class BeatmapLanguagesChangeHandlerTest : BaseChangeHandlerTest<BeatmapLanguagesChangeHandler>
{
    [Test]
    public void TestAdd()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.AvailableTranslationLanguages = new List<CultureInfo>
            {
                new("zh-TW"),
            };
        });

        TriggerHandlerChanged(c =>
        {
            c.Add(new CultureInfo("Ja-jp"));
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            Assert.AreEqual(2, karaokeBeatmap.AvailableTranslationLanguages.Count);
            Assert.AreEqual(new CultureInfo("zh-TW"), karaokeBeatmap.AvailableTranslationLanguages[0]);
            Assert.AreEqual(new CultureInfo("Ja-jp"), karaokeBeatmap.AvailableTranslationLanguages[1]);
        });
    }

    [Test]
    public void TestRemove()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.AvailableTranslationLanguages = new List<CultureInfo>
            {
                new("zh-TW"),
                new("Ja-jp"),
            };
        });

        TriggerHandlerChanged(c =>
        {
            c.Remove(new CultureInfo("Ja-jp"));
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            Assert.AreEqual(1, karaokeBeatmap.AvailableTranslationLanguages.Count);
            Assert.AreEqual(new CultureInfo("zh-TW"), karaokeBeatmap.AvailableTranslationLanguages[0]);
        });
    }

    [Test]
    public void TestIsLanguageContainsTranslation()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.AvailableTranslationLanguages = new List<CultureInfo>
            {
                new("zh-TW"),
                new("Ja-jp"),
            };
        });

        PrepareHitObject(() => new Lyric
        {
            Translations = new Dictionary<CultureInfo, string>
            {
                {
                    new("zh-TW"), "卡拉 OK"
                },
            },
        });

        TriggerHandlerChanged(c =>
        {
            Assert.AreEqual(false, c.IsLanguageContainsTranslation(new CultureInfo("Ja-jp")));
            Assert.AreEqual(true, c.IsLanguageContainsTranslation(new CultureInfo("zh-TW")));
        });
    }

    protected override void SetUpKaraokeBeatmap(Action<KaraokeBeatmap> action)
    {
        base.SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.AvailableTranslationLanguages = new List<CultureInfo>();

            action(karaokeBeatmap);
        });
    }
}
