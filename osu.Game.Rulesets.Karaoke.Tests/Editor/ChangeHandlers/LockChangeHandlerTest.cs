// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers
{
    public class LockChangeHandlerTest : BaseHitObjectPropertyChangeHandlerTest<LockChangeHandler, KaraokeHitObject>
    {
        [Test]
        public void TestLock()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Lock = LockState.None,
            });

            PrepareHitObject(new Note
            {
                Text = "カラオケ",
            });

            TriggerHandlerChanged(c => c.Lock(LockState.Full));

            AssertSelectedHitObject(h =>
            {
                if (h is IHasLock hasLock)
                    Assert.AreEqual(LockState.Full, hasLock.Lock);
            });
        }

        [Test]
        public void TestUnlock()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Lock = LockState.Full,
            });

            PrepareHitObject(new Note
            {
                Text = "カラオケ",
            });

            TriggerHandlerChanged(c => c.Unlock());

            AssertSelectedHitObject(h =>
            {
                if (h is IHasLock hasLock)
                    Assert.AreEqual(LockState.None, hasLock.Lock);
            });
        }

        [Test]
        public void TestLockToReferenceLyric()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                ReferenceLyric = new Lyric(),
                ReferenceLyricConfig = new SyncLyricConfig(),
            });

            TriggerHandlerChangedWithChangeForbiddenException(c => c.Lock(LockState.Full));
        }
    }
}
