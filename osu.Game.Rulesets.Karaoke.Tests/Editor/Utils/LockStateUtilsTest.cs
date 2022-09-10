// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Utils
{
    public class LockStateUtilsTest
    {
        [TestCase(new[] { LockState.Full, LockState.Partial, LockState.None }, 1)]
        [TestCase(new LockState[] { }, 0)]
        public void TestFindUnlockObjects(LockState[] lockStates, int? expected)
        {
            var lyrics = lockStates.Select(x => new Lyric
            {
                Text = "karaoke",
                Lock = x
            });

            int actual = LockStateUtils.FindUnlockObjects(lyrics).Length;
            Assert.AreEqual(expected, actual);
        }
    }
}
