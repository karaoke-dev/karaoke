// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit.Generator.Languages
{
    [TestFixture]
    public class LanguageDetectorTest
    {
        [TestCase("花火大会", "Ja-jp")]
        public void TestDetectLanguage(string text, string language)
        {
            var detector = new LanguageDetector();
        }
    }
}
