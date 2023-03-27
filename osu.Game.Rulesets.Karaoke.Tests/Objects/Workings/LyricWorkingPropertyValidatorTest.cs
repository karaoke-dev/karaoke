// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Workings;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Workings;

public class LyricWorkingPropertyValidatorTest : HitObjectWorkingPropertyValidatorTest<Lyric, LyricWorkingProperty>
{
    [Test]
    public void TestPage()
    {
        var lyric = new Lyric();

        // should be invalid on the first load.
        AssetIsValid(lyric, LyricWorkingProperty.Page, false);

        // page state is valid because assign the property.
        Assert.DoesNotThrow(() => lyric.PageIndex = 1);
        AssetIsValid(lyric, LyricWorkingProperty.Page, true);
    }

    [Test]
    public void TestReferenceLyric()
    {
        var lyric = new Lyric();

        // should be invalid on the first load.
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, false);

        // should be valid if change the reference lyric id.
        Assert.DoesNotThrow(() =>
        {
            lyric.ReferenceLyricId = null;
            lyric.ReferenceLyric = null;
        });
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should be valid if change the reference lyric id.
        Assert.DoesNotThrow(() =>
        {
            lyric.ReferenceLyricId = 1;
            lyric.ReferenceLyric = new Lyric { ID = 1 };
        });
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should be invalid if change the reference lyric id.
        Assert.DoesNotThrow(() => lyric.ReferenceLyricId = 2);
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, false);

        // should be valid again if assign the reference lyric to the matched lyric.
        Assert.DoesNotThrow(() => lyric.ReferenceLyric = new Lyric { ID = 2 });
        AssetIsValid(lyric, LyricWorkingProperty.ReferenceLyric, true);

        // should throw the exception if assign the working reference lyric to the unmatched reference lyric id.
        Assert.Throws<InvalidWorkingPropertyAssignException>(() => lyric.ReferenceLyric = new Lyric { ID = 3 });
        Assert.Throws<InvalidWorkingPropertyAssignException>(() => lyric.ReferenceLyric = null);
    }
}
