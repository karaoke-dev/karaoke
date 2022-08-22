// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricReferenceChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricReferenceChangeHandler, Lyric>
    {
        [Test]
        public void TestUpdateReferenceLyric()
        {
            var lyric = new Lyric
            {
                Text = "Referenced lyric"
            };

            PrepareHitObject(lyric, false);

            PrepareHitObject(new Lyric
            {
                Text = "I need the reference lyric."
            });

            TriggerHandlerChanged(c => c.UpdateReferenceLyric(lyric));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(lyric, h.ReferenceLyric);
            });
        }
    }
}
