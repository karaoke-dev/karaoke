// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Utils
{
    public class LockStateUtilsTest
    {
        [TestCase("Text", LockState.Full)]
        public void TestGetLockState(string propertyName, LockState actual)
        {
            var state = LockStateUtils.GetLyricLockStateByPropertyName(propertyName);
            Assert.AreEqual(state, actual);
        }
    }
}
