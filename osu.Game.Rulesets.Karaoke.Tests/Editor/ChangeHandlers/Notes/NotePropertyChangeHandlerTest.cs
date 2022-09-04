// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Notes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Notes
{
    public class NotePropertyChangeHandlerTest : BaseHitObjectPropertyChangeHandlerTest<NotePropertyChangeHandler, Note>
    {
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
    }
}
