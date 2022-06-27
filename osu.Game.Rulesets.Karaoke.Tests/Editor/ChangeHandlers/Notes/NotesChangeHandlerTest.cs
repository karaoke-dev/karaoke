// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Notes
{
    public class NotesChangeHandlerTest : BaseHitObjectChangeHandlerTest<NotesChangeHandler, Note>
    {
        [Test]
        public void TestSplit()
        {
            PrepareHitObject(new Note
            {
                Text = "カラオケ",
                StartTime = 1000,
                Duration = 1000,
            });

            TriggerHandlerChanged(c => c.Split());

            AssertHitObjects(notes =>
            {
                var actualNotes = notes.ToArray();
                Assert.AreEqual(2, actualNotes.Length);

                var firstNote = actualNotes[0];
                var secondNote = actualNotes[1];

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
            var lyric = new Lyric();

            // note that lyric and notes should in the selection.
            PrepareHitObject(lyric);
            PrepareHitObjects(new[]
            {
                new Note
                {
                    Text = "カラ",
                    RubyText = "から",
                    StartTime = 1000,
                    Duration = 500,
                    ParentLyric = lyric,
                },
                new Note
                {
                    Text = "オケ",
                    RubyText = "おけ",
                    StartTime = 1500,
                    Duration = 500,
                    ParentLyric = lyric,
                }
            });

            TriggerHandlerChanged(c => c.Combine());

            AssertHitObjects(notes =>
            {
                var actualNotes = notes.ToArray();
                Assert.AreEqual(1, actualNotes.Length);

                var combinedNote = actualNotes.FirstOrDefault();
                Assert.IsNotNull(combinedNote);
                Assert.AreEqual("カラ", combinedNote.Text);
                Assert.AreEqual(null, combinedNote.RubyText);
                Assert.AreEqual(1000, combinedNote.StartTime);
                Assert.AreEqual(1000, combinedNote.Duration);
            });
        }

        [Test]
        public void TestChangeText()
        {
            PrepareHitObject(new Note
            {
                Text = "カラオケ",
            });

            TriggerHandlerChanged(c => c.ChangeText("からおけ"));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual("からおけ", h.Text);
            });
        }

        [Test]
        public void TestChangeRubyText()
        {
            PrepareHitObject(new Note
            {
                RubyText = "からおけ",
            });

            TriggerHandlerChanged(c => c.ChangeRubyText("カラオケ"));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual("カラオケ", h.RubyText);
            });
        }

        [Test]
        public void TestChangeDisplayStateToVisible()
        {
            PrepareHitObject(new Note());

            TriggerHandlerChanged(c => c.ChangeDisplayState(true));

            AssertSelectedHitObject(h =>
            {
                Assert.IsTrue(h.Display);
            });
        }

        [Test]
        public void TestChangeDisplayStateToNonVisible()
        {
            PrepareHitObject(new Note
            {
                Display = true,
                Tone = new Tone(3)
            });

            TriggerHandlerChanged(c => c.ChangeDisplayState(false));

            AssertSelectedHitObject(h =>
            {
                Assert.IsFalse(h.Display);
                Assert.AreEqual(new Tone(), h.Tone);
            });
        }

        [Test]
        public void TestClear()
        {
            var lyric = new Lyric();

            // note that lyric and notes should in the selection.
            PrepareHitObject(lyric);
            PrepareHitObjects(new[]
            {
                new Note
                {
                    Text = "カラ",
                    RubyText = "から",
                    StartTime = 1000,
                    Duration = 500,
                    ParentLyric = lyric,
                },
                new Note
                {
                    Text = "オケ",
                    RubyText = "おけ",
                    StartTime = 1500,
                    Duration = 500,
                    ParentLyric = lyric,
                }
            }, false);

            TriggerHandlerChanged(c => c.Clear());

            AssertHitObjects(Assert.IsEmpty);
        }
    }
}
