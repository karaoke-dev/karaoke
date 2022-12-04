// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.Edit
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
