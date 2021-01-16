// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Input.Bindings;
using osu.Game.Input.Bindings;

namespace osu.Game.Rulesets.Karaoke
{
    public class KaraokeEditInputManager : DatabasedKeyBindingContainer<KaraokeEditAction>
    {
        public KaraokeEditInputManager(RulesetInfo ruleset)
            : base(ruleset, 2, SimultaneousBindingMode.Unique)
        {
        }
    }

    public enum KaraokeEditAction
    {
        // moving
        [Description("Up")]
        Up,

        [Description("Down")]
        Down,

        [Description("Left")]
        Left,

        [Description("Right")]
        Right,

        [Description("First")]
        First,

        [Description("Last")]
        Last,

        // Edit
        [Description("Create new")]
        Create,

        [Description("Remove")]
        Remove,

        [Description("Set time")]
        SetTime,

        [Description("Clear time")]
        ClearTime,
    }
}
