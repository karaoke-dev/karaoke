// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Asserts;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.RomajiTags
{
    public class RomajiTagGeneratorSelectorTest : BaseGeneratorSelectorTest<RomajiTagGeneratorSelector>
    {
        [TestCase(17, "花火大会", new[] { "[0,2]:hanabi", "[2,4]:taikai" })] // Japanese
        [TestCase(1041, "はなび", new[] { "[0,3]:hanabi" })] // Japanese
        [TestCase(1028, "はなび", null)] // Chinese(should not supported)
        public void TestCreateRomajiTag(int lcid, string text, string[] actualRomaji)
        {
            var lyric = new Lyric
            {
                Language = new CultureInfo(lcid),
                Text = text,
            };
            var selector = CreateSelector();
            var generatedRomaji = selector.GenerateRomajiTags(lyric);
            TextTagAssert.ArePropertyEqual(generatedRomaji, TestCaseTagHelper.ParseRomajiTags(actualRomaji));
        }
    }
}
