// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics.Edit
{
    public class SingleLyricEditorTest
    {
        [Test]
        public void TestLockMessage()
        {
            var lyric = new Lyric();
            Assert.IsNull(InteractableLyric.GetLyricPropertyLockedReason(lyric, LyricEditorMode.View));
        }
    }
}
