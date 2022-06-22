// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class TimeTag
    {
        /// <summary>
        /// Invoked when any property of this <see cref="RubyTag"/> is changed.
        /// </summary>
        public event Action Changed;

        public TimeTag(TextIndex index, double? time = null)
        {
            Index = index;
            Time = time;

            TimeBindable.ValueChanged += _ => Changed?.Invoke();
        }

        /// <summary>
        /// Time tag's index.
        /// Notice that this index means index of characters.
        /// </summary>
        public TextIndex Index { get; }

        [JsonIgnore]
        public readonly Bindable<double?> TimeBindable = new();

        /// <summary>
        /// Time
        /// </summary>
        public double? Time
        {
            get => TimeBindable.Value;
            set => TimeBindable.Value = value;
        }
    }
}
