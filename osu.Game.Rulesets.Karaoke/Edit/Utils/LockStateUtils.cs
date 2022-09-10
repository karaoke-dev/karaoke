// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Utils
{
    public static class LockStateUtils
    {
        public static TLock[] FindUnlockObjects<TLock>(IEnumerable<TLock> objects) where TLock : IHasLock
            => objects.Where(x => x.Lock == LockState.None).ToArray();
    }
}
