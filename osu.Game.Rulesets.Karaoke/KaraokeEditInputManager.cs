﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Input.Bindings;

namespace osu.Game.Rulesets.Karaoke;

public partial class KaraokeEditInputManager : DatabasedKeyBindingContainer<KaraokeEditAction>
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
    MoveToPreviousLyric,

    [Description("Down")]
    MoveToNextLyric,

    [Description("First Lyric")]
    MoveToFirstLyric,

    [Description("Last Lyric")]
    MoveToLastLyric,

    [Description("Left")]
    MoveToPreviousIndex,

    [Description("Right")]
    MoveToNextIndex,

    [Description("First index")]
    MoveToFirstIndex,

    [Description("Last index")]
    MoveToLastIndex,

    // Switch edit mode.
    [Description("Previous edit mode")]
    PreviousEditMode,

    [Description("Next edit mode")]
    NextEditMode,

    // Edit Ruby tag.
    [Description("Reduce ruby-tag start index")]
    EditRubyTagReduceStartIndex,

    [Description("Increase ruby-tag start index")]
    EditRubyTagIncreaseStartIndex,

    [Description("Reduce ruby-tag end index")]
    EditRubyTagReduceEndIndex,

    [Description("Increase ruby-tag end index")]
    EditRubyTagIncreaseEndIndex,

    // Edit time-tag.
    [Description("Create start time-tag")]
    CreateStartTimeTag,

    [Description("Create end time-tag")]
    CreateEndTimeTag,

    [Description("Remove start time-tag")]
    RemoveStartTimeTag,

    [Description("Remove end time-tag")]
    RemoveEndTimeTag,

    [Description("Set time")]
    SetTime,

    [Description("Clear time")]
    ClearTime,

    // Action for compose mode.
    [Description("Increase font size.")]
    IncreasePreviewFontSize,

    [Description("Decrease font size.")]
    DecreasePreviewFontSize,
}
