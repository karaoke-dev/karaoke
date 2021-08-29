// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class TimeTag
    {
        public TimeTag(TextIndex index, double? time = null)
        {
            Index = index;
            Time = time;
        }

        /// <summary>
        /// Time tag's index.
        /// Notice that this index means index of characters.
        /// </summary>
        public TextIndex Index { get; }

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
