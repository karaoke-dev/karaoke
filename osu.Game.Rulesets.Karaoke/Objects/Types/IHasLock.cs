// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.ComponentModel;

namespace osu.Game.Rulesets.Karaoke.Objects.Types
{
    public interface IHasLock
    {
        LockState Lock { get; set; }
    }

    public enum LockState
    {
        [Description("None")]
        None,

        [Description("Partial")]
        Partial,

        [Description("Full")]
        Full
    }
}
