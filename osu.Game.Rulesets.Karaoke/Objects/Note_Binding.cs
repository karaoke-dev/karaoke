// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Objects;

/// <summary>
/// Placing the binding-related logic.
/// </summary>
public partial class Note
{
    private void initInternalBindingEvent()
    {
        StartTimeOffsetBindable.ValueChanged += _ => syncStartTimeAndDurationFromTimeTag();
        EndTimeOffsetBindable.ValueChanged += _ => syncStartTimeAndDurationFromTimeTag();
        ReferenceTimeTagIndexBindable.ValueChanged += _ => syncStartTimeAndDurationFromTimeTag();
    }

    private void initReferenceLyricEvent()
    {
        ReferenceLyricBindable.ValueChanged += e =>
        {
            if (e.OldValue != null)
                e.OldValue.TimeTagsVersion.ValueChanged -= timeTagVersionChanged;

            if (e.NewValue != null)
                e.NewValue.TimeTagsVersion.ValueChanged += timeTagVersionChanged;

            syncStartTimeAndDurationFromTimeTag();
        };

        void timeTagVersionChanged(ValueChangedEvent<int> e) => syncStartTimeAndDurationFromTimeTag();
    }

    private void syncStartTimeAndDurationFromTimeTag()
    {
        var startTimeTag = StartReferenceTimeTag;
        var endTimeTag = EndReferenceTimeTag;

        double startTime = startTimeTag?.Time ?? 0;
        double endTime = endTimeTag?.Time ?? 0;
        double duration = endTime - startTime;

        StartTimeBindable.Value = startTimeTag == null ? 0 : startTime + StartTimeOffset;
        DurationBindable.Value = endTimeTag == null ? 0 : Math.Max(duration - StartTimeOffset + EndTimeOffset, 0);
    }
}
