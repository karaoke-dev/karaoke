// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Objects.Properties
{
    public class ReferenceLyricConfig : IReferenceLyricPropertyConfig
    {
        public event Action? Changed;

        public ReferenceLyricConfig()
        {
            OffsetTimeBindable.ValueChanged += _ => Changed?.Invoke();
        }

        [JsonIgnore]
        public readonly Bindable<double> OffsetTimeBindable = new BindableDouble();

        public double OffsetTime
        {
            get => OffsetTimeBindable.Value;
            set => OffsetTimeBindable.Value = value;
        }
    }
}
