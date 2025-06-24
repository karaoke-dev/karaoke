// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Notes;

public partial class NotesChangeHandlerTest : BaseHitObjectChangeHandlerTest<NotesChangeHandler, Note>
{
    [Test]
    public void TestSplit()
    {
        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "カラオケ", 1000, 1000);

        PrepareHitObject(() => new Note
        {
            Text = "カラオケ",
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
        });

        TriggerHandlerChanged(c => c.Split());

        AssertHitObjects(notes =>
        {
            var actualNotes = notes.ToArray();
            Assert.That(actualNotes.Length, Is.EqualTo(2));
            var firstNote = actualNotes[0];
            var secondNote = actualNotes[1];
            Assert.That(firstNote.ReferenceLyric, Is.SameAs(secondNote.ReferenceLyric));
            Assert.That(firstNote.Text, Is.EqualTo("カラオケ"));
            Assert.That(firstNote.StartTime, Is.EqualTo(1000));
            Assert.That(firstNote.Duration, Is.EqualTo(500));
            Assert.That(secondNote.Text, Is.EqualTo("カラオケ"));
            Assert.That(secondNote.StartTime, Is.EqualTo(1500));
            Assert.That(secondNote.Duration, Is.EqualTo(500));
        });
    }

    [Test]
    public void TestCombine()
    {
        var referencedLyric = TestCaseNoteHelper.CreateLyricForNote(2, "カラオケ", 1000, 1000);

        // note that lyric and notes should in the selection.
        PrepareHitObject(() => referencedLyric);
        PrepareHitObjects(() => new[]
        {
            new Note
            {
                Text = "カラ",
                RubyText = "から",
                ReferenceLyricId = referencedLyric.ID,
                ReferenceLyric = referencedLyric,
                ReferenceTimeTagIndex = 0,
            },
            new Note
            {
                Text = "オケ",
                RubyText = "おけ",
                ReferenceLyricId = referencedLyric.ID,
                ReferenceLyric = referencedLyric,
                ReferenceTimeTagIndex = 0,
            },
        });

        TriggerHandlerChanged(c => c.Combine());

        AssertHitObjects(notes =>
        {
            var actualNotes = notes.ToArray();
            Assert.That(actualNotes.Length, Is.EqualTo(1));
            var combinedNote = actualNotes.First();
            Assert.That(combinedNote.Text, Is.EqualTo("カラ"));
            Assert.That(combinedNote.RubyText, Is.EqualTo("から"));
            Assert.That(combinedNote.StartTime, Is.EqualTo(1000));
            Assert.That(combinedNote.Duration, Is.EqualTo(1000));
        });
    }

    [Test]
    public void TestClear()
    {
        var referencedLyric = new Lyric();

        // note that lyric and notes should in the selection.
        PrepareHitObject(() => referencedLyric);
        PrepareHitObjects(() => new[]
        {
            new Note
            {
                Text = "カラ",
                RubyText = "から",
                ReferenceLyricId = referencedLyric.ID,
                ReferenceLyric = referencedLyric,
            },
            new Note
            {
                Text = "オケ",
                RubyText = "おけ",
                ReferenceLyricId = referencedLyric.ID,
                ReferenceLyric = referencedLyric,
            },
        }, false);

        TriggerHandlerChanged(c => c.Clear());

        AssertHitObjects(x => Assert.That(x, Is.Empty));
    }
}
