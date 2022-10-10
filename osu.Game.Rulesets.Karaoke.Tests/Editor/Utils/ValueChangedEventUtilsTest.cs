// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Utils
{
    public class ValueChangedEventUtilsTest
    {
        [Test]
        public void TestLyricChangedWithSameLyric()
        {
            var lyric1 = new Lyric
            {
                Text = "lyric 1"
            };

            var oldCaret = new ClickingCaretPosition(lyric1);
            var newCaret = new ClickingCaretPosition(lyric1);

            Assert.IsFalse(ValueChangedEventUtils.LyricChanged(new ValueChangedEvent<ICaretPosition?>(oldCaret, newCaret)));
        }

        [Test]
        public void TestLyricChangedWithDifferentLyric()
        {
            var lyric1 = new Lyric
            {
                Text = "lyric 1"
            };

            var lyric2 = new Lyric
            {
                Text = "lyric 2"
            };

            var oldCaret = new ClickingCaretPosition(lyric1);
            var newCaret = new ClickingCaretPosition(lyric2);

            Assert.IsTrue(ValueChangedEventUtils.LyricChanged(new ValueChangedEvent<ICaretPosition?>(oldCaret, newCaret)));
        }

        [Test]
        public void TestLyricChangedWithSameLyricButDifferentCaretPosiiton()
        {
            var lyric1 = new Lyric
            {
                Text = "lyric 1"
            };

            var oldCaret = new ClickingCaretPosition(lyric1);
            var newCaret = new TimeTagCaretPosition(lyric1, new TimeTag(new TextIndex(1)));

            Assert.IsFalse(ValueChangedEventUtils.LyricChanged(new ValueChangedEvent<ICaretPosition?>(oldCaret, newCaret)));
        }

        [Test]
        public void TestEditModeChangedWithDefaultValue()
        {
            var oldMode = default(ModeWithSubMode);
            var newMode = new ModeWithSubMode
            {
                Mode = LyricEditorMode.View
            };

            Assert.IsTrue(ValueChangedEventUtils.EditModeChanged(new ValueChangedEvent<ModeWithSubMode>(oldMode, newMode)));
        }

        [Test]
        public void TestEditModeChanged()
        {
            var oldMode = new ModeWithSubMode
            {
                Mode = LyricEditorMode.View
            };
            var newMode = new ModeWithSubMode
            {
                Mode = LyricEditorMode.View
            };

            Assert.IsFalse(ValueChangedEventUtils.EditModeChanged(new ValueChangedEvent<ModeWithSubMode>(oldMode, newMode)));
        }
    }
}
