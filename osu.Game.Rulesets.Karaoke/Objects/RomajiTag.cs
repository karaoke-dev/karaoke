// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class RomajiTag : ITextTag
    {
        /// <summary>
        /// Invoked when any property of this <see cref="RomajiTag"/> is changed.
        /// </summary>
        public event Action Changed;

        public RomajiTag()
        {
            TextBindable.ValueChanged += _ => Changed?.Invoke();
            StartIndexBindable.ValueChanged += _ => Changed?.Invoke();
            EndIndexBindable.ValueChanged += _ => Changed?.Invoke();
        }

        [JsonIgnore]
        public readonly Bindable<string> TextBindable = new();

        /// <summary>
        /// If kanji Matched, then apply romaji
        /// </summary>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        [JsonIgnore]
        public readonly BindableInt StartIndexBindable = new();

        /// <summary>
        /// Start index
        /// </summary>
        public int StartIndex
        {
            get => StartIndexBindable.Value;
            set => StartIndexBindable.Value = value;
        }

        [JsonIgnore]
        public readonly BindableInt EndIndexBindable = new();

        /// <summary>
        /// End index
        /// </summary>
        public int EndIndex
        {
            get => EndIndexBindable.Value;
            set => EndIndexBindable.Value = value;
        }
    }
}
