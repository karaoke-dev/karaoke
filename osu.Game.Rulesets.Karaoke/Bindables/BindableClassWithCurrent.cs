// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Bindables
{
    public class BindableClassWithCurrent<T> : BindableWithCurrent<T> where T : class
    {
        /// <summary>
        /// Raise <see cref="Bindable{T}.ValueChanged"/> and <see cref="Bindable{T}.DisabledChanged"/> once, without any changes actually occurring.
        /// This does not propagate to any outward bound bindables.
        /// </summary>
        public void TriggerOtherChange()
        {
            // will trigger other bindable
            TriggerValueChange(Value, this);
            TriggerDisabledChange(this, false);
        }
    }
}
