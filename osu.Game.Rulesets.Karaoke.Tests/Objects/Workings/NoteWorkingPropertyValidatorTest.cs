// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Tests.Extensions;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects.Workings;

public class NoteWorkingPropertyValidatorTest : HitObjectWorkingPropertyValidatorTest<Note, NoteWorkingProperty>
{
    [Test]
    public void TestPage()
    {
        var note = new Note();

        // page state is valid because assign the property.
        Assert.DoesNotThrow(() => note.PageIndex = 1);
        AssetIsValid(note, NoteWorkingProperty.Page, true);
    }

    [Test]
    public void TestReferenceLyric()
    {
        var note = new Note();

        // should be valid if change the reference lyric id.
        Assert.DoesNotThrow(() =>
        {
            note.ReferenceLyricId = null;
            note.ReferenceLyric = null;
        });
        AssetIsValid(note, NoteWorkingProperty.ReferenceLyric, true);

        // should be invalid if change the reference lyric id.
        Assert.DoesNotThrow(() =>
        {
            note.ReferenceLyricId = TestCaseElementIdHelper.CreateElementIdByNumber(1);
        });
        AssetIsValid(note, NoteWorkingProperty.ReferenceLyric, false);

        // should be valid again if change the id back.
        Assert.DoesNotThrow(() =>
        {
            note.ReferenceLyricId = null;
        });
        AssetIsValid(note, NoteWorkingProperty.ReferenceLyric, true);

        // should be valid if change the reference lyric id.
        Assert.DoesNotThrow(() =>
        {
            var lyric = new Lyric();

            note.ReferenceLyricId = lyric.ID;
            note.ReferenceLyric = lyric;
        });
        AssetIsValid(note, NoteWorkingProperty.ReferenceLyric, true);

        // should be invalid if change the reference lyric id.
        Assert.DoesNotThrow(() => note.ReferenceLyricId = TestCaseElementIdHelper.CreateElementIdByNumber(2));
        AssetIsValid(note, NoteWorkingProperty.ReferenceLyric, false);

        // should be valid again if assign the reference lyric to the matched lyric.
        Assert.DoesNotThrow(() => note.ReferenceLyric = new Lyric().ChangeId(2));
        AssetIsValid(note, NoteWorkingProperty.ReferenceLyric, true);

        // should throw the exception if assign the working reference lyric to the unmatched reference lyric id.
        Assert.Throws<InvalidWorkingPropertyAssignException>(() => note.ReferenceLyric = new Lyric().ChangeId(3));
        Assert.Throws<InvalidWorkingPropertyAssignException>(() => note.ReferenceLyric = null);
    }

    [Test]
    public void TestEffectApplier()
    {
        var note = new Note();

        // page state is valid because assign the property.
        Assert.DoesNotThrow(() => note.EffectApplier = new NoteClassicStageEffectApplier(Array.Empty<StageElement>(), new ClassicStageDefinition()));
        AssetIsValid(note, NoteWorkingProperty.EffectApplier, true);
    }

    protected override bool IsInitialStateValid(NoteWorkingProperty flag)
    {
        return new NoteWorkingPropertyValidator(new Note()).IsValid(flag);
    }
}
