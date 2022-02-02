// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    public class KaraokeBeatmapSkinDecodingTest
    {
        [Test]
        public void TestKaraokeBeatmapSkinDefaultValue()
        {
            var storage = TestResources.CreateSkinStorageResourceProvider();
            var skin = new KaraokeBeatmapSkin(new SkinInfo { Name = "special-skin" }, storage);

            var testingLyric = new Lyric();
            var testingNote = new Note
            {
                ParentLyric = testingLyric
            };

            // try to get default value from the skin.
            var defaultLyricConfig = skin.GetConfig<Lyric, LyricConfig>(testingLyric).Value;
            var defaultLyricStyle = skin.GetConfig<Lyric, LyricStyle>(testingLyric).Value;
            var defaultNoteStyle = skin.GetConfig<Note, NoteStyle>(testingNote).Value;

            // should be able to get the default value.
            Assert.NotNull(defaultLyricConfig);
            Assert.NotNull(defaultLyricStyle);
            Assert.NotNull(defaultNoteStyle);

            // Check the content
            Assert.NotNull(defaultLyricConfig.Name, "Default lyric config");
            Assert.NotNull(defaultLyricStyle.Name, "Default lyric style");
            Assert.NotNull(defaultNoteStyle.Name, "Default note style");
        }

        [Test]
        public void TestKaraokeBeatmapSkinLayout()
        {
            var storage = TestResources.CreateSkinStorageResourceProvider();
            var skin = new KaraokeBeatmapSkin(new SkinInfo { Name = "special-skin" }, storage);

            var firstLyric = new Lyric
            {
                ID = 1,
            };
            var secondLyric = new Lyric
            {
                ID = 2,
            };

            // try to get customized value from the skin.
            var firstLyricLayout = skin.GetConfig<Lyric, LyricLayout>(firstLyric).Value;
            var secondLyricLayout = skin.GetConfig<Lyric, LyricLayout>(secondLyric).Value;

            // should be able to get the default value.
            Assert.NotNull(firstLyricLayout);
            Assert.NotNull(secondLyricLayout);

            // Check the content
            Assert.NotNull(firstLyricLayout.Name, "下-1");
            Assert.NotNull(secondLyricLayout.Name, "下-2");
        }
    }
}
