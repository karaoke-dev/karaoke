// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricTextChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricTextChangeHandler, Lyric>
    {
        [Test]
        public void TestInsertText()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラ"
            });

            TriggerHandlerChanged(c => c.InsertText(2, "オケ"));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual("カラオケ", h.Text);
            });
        }

        [Test]
        public void TestDeleteLyricText()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ"
            });

            TriggerHandlerChanged(c => c.DeleteLyricText(4));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual("カラオ", h.Text);
            });
        }
    }
}
