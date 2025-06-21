// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning;

public class KaraokeBeatmapSkinDecodingTest
{
    [Test]
    public void TestKaraokeBeatmapSkinDefaultValue()
    {
        var storage = TestResources.CreateSkinStorageResourceProvider();
        var skin = new KaraokeBeatmapSkin(new SkinInfo { Name = "special-skin" }, storage);

        var referencedLyric = new Lyric();
        var testingNote = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
        };

        // try to get default value from the skin.
        var defaultLyricFontInfo = skin.GetConfig<Lyric, LyricFontInfo>(referencedLyric)!.Value;
        var defaultNoteStyle = skin.GetConfig<Note, NoteStyle>(testingNote)!.Value;

        // should be able to get the default value.
        Assert.That(defaultLyricFontInfo, Is.Not.Null);
        Assert.That(defaultNoteStyle, Is.Not.Null);

        // Check the content
        Assert.That(defaultLyricFontInfo.Name, Is.Not.Null, "Default lyric config");
        Assert.That(defaultNoteStyle.Name, Is.Not.Null, "Default note style");
    }
}
