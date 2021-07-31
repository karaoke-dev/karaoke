// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Karaoke.IO.Stores;
using osu.Game.Rulesets.Karaoke.Tests.Resources;

namespace osu.Game.Rulesets.Karaoke.Tests.IO.Stores
{
    public class OtfGlyphStoreTest
    {
        private OtfGlyphStore glyphStore;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var fontResourceStore = new NamespacedResourceStore<byte[]>(TestResources.GetStore(), "Resources.Testing.Fonts.Ttf");
            glyphStore = new OtfGlyphStore(fontResourceStore, "OpenSans-Regular");
            glyphStore.LoadFontAsync().Wait();
        }

        [TestCase('a', true)]
        [TestCase(' ', true)]
        [TestCase('ㄅ', false)]
        [TestCase('あ', false)]
        public void TestHasGlyph(char c, bool include)
        {
            Assert.AreEqual(glyphStore.HasGlyph(c), include);
        }

        [Test]
        public void TestGetBaseHeight()
        {
            // todo : not really sure why is 2780 in here.
            Assert.AreEqual(glyphStore.GetBaseHeight(), 2789);
        }

        [TestCase('a', true)]
        [TestCase('ㄅ', false)]
        [TestCase('あ', false)]
        public void TestGetCharacterGlyph(char c, bool include)
        {
            var characterGlyph = glyphStore.Get(c);

            if (characterGlyph == null)
            {
                Assert.IsFalse(include);
                return;
            }

            Assert.AreEqual(characterGlyph.Character, c);
        }
    }
}
