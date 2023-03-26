// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;
using osu.Game.Rulesets.Karaoke.Tests.Resources;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning;

public class KaraokeSkinDecodingTest
{
    [Test]
    public void TestKaraokeSkinDefaultValue()
    {
        var storage = TestResources.CreateSkinStorageResourceProvider();
        var skin = new KaraokeSkin(new SkinInfo { Name = "special-skin" }, storage);

        var referencedLyric = new Lyric { ID = 2 };
        var testingNote = new Note
        {
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric
        };

        // try to get default value from the skin.
        var defaultLyricFontInfo = skin.GetConfig<Lyric, LyricFontInfo>(referencedLyric)!.Value;
        var defaultLyricStyle = skin.GetConfig<Lyric, LyricStyle>(referencedLyric)!.Value;
        var defaultNoteStyle = skin.GetConfig<Note, NoteStyle>(testingNote)!.Value;

        // should be able to get the default value.
        Assert.IsNotNull(defaultLyricFontInfo);
        Assert.IsNotNull(defaultLyricStyle);
        Assert.IsNotNull(defaultNoteStyle);

        // Check the content
        Assert.IsNotNull(defaultLyricFontInfo.Name, "Default lyric config");
        Assert.IsNotNull(defaultLyricStyle.Name, "Default lyric style");
        Assert.IsNotNull(defaultNoteStyle.Name, "Default note style");
    }
}
