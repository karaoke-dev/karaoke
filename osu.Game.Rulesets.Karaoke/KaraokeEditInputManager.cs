// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

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

        // Switch edit mode.
        [Description("Previous edit mode")]
        PreviousEditMode,

        [Description("Next edit mode")]
        NextEditMode,

        // Edit Ruby / romaji tag.
        [Description("Reduce start index")]
        EditTextTagReduceStartIndex,

        [Description("Increase start index")]
        EditTextTagIncreaseStartIndex,

        [Description("Reduce end index")]
        EditTextTagReduceEndIndex,

        [Description("Increase end index")]
        EditTextTagIncreaseEndIndex,

        // Edit time-tag.
        [Description("Create new")]
        Create,

        [Description("Remove")]
        Remove,

        [Description("Shift the time-tag left.")]
        ShiftTheTimeTagLeft,

        [Description("Shift the time-tag right.")]
        ShiftTheTimeTagRight,

        [Description("Shift the time-tag state left.")]
        ShiftTheTimeTagStateLeft,

        [Description("Shift the time-tag state right.")]
        ShiftTheTimeTagStateRight,

        [Description("Set time")]
        SetTime,

        [Description("Clear time")]
        ClearTime,
    }
}
