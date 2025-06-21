// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricTranslationChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricTranslationChangeHandler>
{
    [Test]
    public void TestUpdateTranslationWithNewLanguage()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.UpdateTranslation(new CultureInfo(17), "からおけ"));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Translations.Count, Is.EqualTo(1));
            Assert.That(h.Translations[new CultureInfo(17)], Is.EqualTo("からおけ"));
        });
    }

    [Test]
    public void TestUpdateTranslationWithExistLanguage()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Translations = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo(17), "からおけ" },
            },
        });

        TriggerHandlerChanged(c => c.UpdateTranslation(new CultureInfo(17), "karaoke"));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Translations.Count, Is.EqualTo(1));
            Assert.That(h.Translations[new CultureInfo(17)], Is.EqualTo("karaoke"));
        });
    }

    [Test]
    public void TestUpdateTranslationWithEmptyText()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Translations = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo(17), "からおけ" },
            },
        });

        TriggerHandlerChanged(c => c.UpdateTranslation(new CultureInfo(17), string.Empty));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Translations, Is.Empty);
        });
    }

    [Test]
    public void TestUpdateTranslationWithNullText()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Translations = new Dictionary<CultureInfo, string>
            {
                { new CultureInfo(17), "からおけ" },
            },
        });

        TriggerHandlerChanged(c => c.UpdateTranslation(new CultureInfo(17), string.Empty));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.Translations, Is.Empty);
        });
    }

    [Test]
    public void TestWithReferenceLyric()
    {
        PrepareLyricWithSyncConfig(new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChangedWithException<ChangeForbiddenException>(c => c.UpdateTranslation(new CultureInfo(17), "からおけ"));
    }
}
