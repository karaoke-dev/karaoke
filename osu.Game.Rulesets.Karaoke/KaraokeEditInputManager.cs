// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Input.Bindings;

namespace osu.Game.Rulesets.Karaoke
{
    public class KaraokeEditInputManager : DatabasedKeyBindingContainer<KaraokeEditAction>
    {
        public KaraokeEditInputManager(RulesetInfo ruleset)
            : base(ruleset, 2, SimultaneousBindingMode.Unique, KeyCombinationMatchingMode.Modifiers)
        {
        }

        protected override IEnumerable<Drawable> KeyBindingInputQueue
        {
            get
            {
                var queue = base.KeyBindingInputQueue;
                return queue.OrderBy(x => x is IHasIKeyBindingHandlerOrder keyBindingHandlerOrder
                    ? keyBindingHandlerOrder.KeyBindingHandlerOrder
                    : int.MaxValue);
            }
        }
    }

    public interface IHasIKeyBindingHandlerOrder
    {
        int KeyBindingHandlerOrder { get; }
    }

    public enum KaraokeEditAction
    {
        // moving
        [Description("Up")]
        MoveUp,

        [Description("Down")]
        MoveDown,

        [Description("Left")]
        MoveLeft,

        [Description("Right")]
        MoveRight,

        [Description("First")]
        MoveToFirst,

        [Description("Last")]
        MoveToLast,

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
        CreateTimeTag,

        [Description("Remove")]
        RemoveTimeTag,

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
