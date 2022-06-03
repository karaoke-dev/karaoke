// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricLanguageChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricLanguageChangeHandler, Lyric>
    {
        [Test]
        public void TestSetLanguageToJapanese()
        {
            var language = new CultureInfo("ja");
            PrepareHitObject(new Lyric());

            TriggerHandlerChanged(c => c.SetLanguage(language));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(language, h.Language);
            });
        }

        [Test]
        public void TestSetLanguageToNull()
        {
            PrepareHitObject(new Lyric
            {
                Text = "???"
            });

            TriggerHandlerChanged(c => c.SetLanguage(null));

            AssertSelectedHitObject(h =>
            {
                Assert.IsNull(h.Language);
            });
        }
    }
}
