// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Objects.Types
{
    public interface IHasLock
    {
        LockState Lock { get; set; }
    }

    public enum LockState
    {
        None,

        Partial,

        Full
    }
}
