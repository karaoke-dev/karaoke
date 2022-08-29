// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Objects.Properties
{
    public class SyncLyricConfig : IReferenceLyricPropertyConfig
    {
        public event Action? Changed;

        public SyncLyricConfig()
        {
            OffsetTimeBindable.ValueChanged += _ => Changed?.Invoke();
            SyncSingerPropertyBindable.ValueChanged += _ => Changed?.Invoke();
            SyncTimeTagPropertyBindable.ValueChanged += _ => Changed?.Invoke();
        }

        [JsonIgnore]
        public readonly Bindable<double> OffsetTimeBindable = new BindableDouble();

        public double OffsetTime
        {
            get => OffsetTimeBindable.Value;
            set => OffsetTimeBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<bool> SyncSingerPropertyBindable = new BindableBool(true);

        /// <summary>
        /// Sync the singer from referenced lyric.
        /// </summary>
        [DefaultValue(true)]
        public bool SyncSingerProperty
        {
            get => SyncSingerPropertyBindable.Value;
            set => SyncSingerPropertyBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<bool> SyncTimeTagPropertyBindable = new BindableBool(true);

        /// <summary>
        /// Sync the time-tags from referenced lyric.
        /// </summary>
        [DefaultValue(true)]
        public bool SyncTimeTagProperty
        {
            get => SyncTimeTagPropertyBindable.Value;
            set => SyncTimeTagPropertyBindable.Value = value;
        }
    }
}
