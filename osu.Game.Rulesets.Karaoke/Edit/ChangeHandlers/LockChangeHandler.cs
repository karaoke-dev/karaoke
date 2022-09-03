// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public class LockChangeHandler : HitObjectPropertyChangeHandler<KaraokeHitObject>, ILockChangeHandler
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

        protected override bool IsWritePropertyLocked(KaraokeHitObject hitObject)
        {
            // todo: implement.
            return hitObject switch
            {
                Lyric => false,
                Note => false,
                _ => throw new NotSupportedException()
            };
        }
    }
}
