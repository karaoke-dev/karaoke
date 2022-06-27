// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public class LockChangeHandler : HitObjectChangeHandler<KaraokeHitObject>, ILockChangeHandler
    {
        public void Lock(LockState lockState)
        {
            PerformOnSelection(h =>
            {
                if (h is IHasLock hasLock)
                    hasLock.Lock = lockState;
            });
        }

        public void Unlock()
        {
            Lock(LockState.None);
        }
    }
}
