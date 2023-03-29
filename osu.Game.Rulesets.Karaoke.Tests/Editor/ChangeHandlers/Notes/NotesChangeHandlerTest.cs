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
            ReferenceLyric = referencedLyric
        });

        TriggerHandlerChanged(c => c.Split());

        AssertHitObjects(notes =>
        {
            var actualNotes = notes.ToArray();
            Assert.AreEqual(2, actualNotes.Length);

            var firstNote = actualNotes[0];
            var secondNote = actualNotes[1];

            Assert.AreSame(firstNote.ReferenceLyric, secondNote.ReferenceLyric);

            Assert.AreEqual("カラオケ", firstNote.Text);
            Assert.AreEqual(1000, firstNote.StartTime);
            Assert.AreEqual(500, firstNote.Duration);

            Assert.AreEqual("カラオケ", secondNote.Text);
            Assert.AreEqual(1500, secondNote.StartTime);
            Assert.AreEqual(500, secondNote.Duration);
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
                ReferenceTimeTagIndex = 0
            },
            new Note
            {
                Text = "オケ",
                RubyText = "おけ",
                ReferenceLyricId = referencedLyric.ID,
                ReferenceLyric = referencedLyric,
                ReferenceTimeTagIndex = 0
            }
        });

        TriggerHandlerChanged(c => c.Combine());

        AssertHitObjects(notes =>
        {
            var actualNotes = notes.ToArray();
            Assert.AreEqual(1, actualNotes.Length);

            var combinedNote = actualNotes.First();
            Assert.AreEqual("カラ", combinedNote.Text);
            Assert.AreEqual("から", combinedNote.RubyText);
            Assert.AreEqual(1000, combinedNote.StartTime);
            Assert.AreEqual(1000, combinedNote.Duration);
        });
    }

    [Test]
    public void TestClear()
    {
        var referencedLyric = new Lyric { ID = 2 };

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
            }
        }, false);

        TriggerHandlerChanged(c => c.Clear());

        AssertHitObjects(Assert.IsEmpty);
    }
}
