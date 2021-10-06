// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using NuGet.Packaging;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class Lyric : KaraokeHitObject, IHasDuration, IHasSingers, IHasOrder, IHasLock
    {
        [JsonIgnore]
        public readonly Bindable<string> TextBindable = new();

        /// <summary>
        /// Text of the lyric
        /// </summary>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<TimeTag[]> TimeTagsBindable = new();

        /// <summary>
        /// Time tags
        /// </summary>
        public TimeTag[] TimeTags
        {
            get => TimeTagsBindable.Value;
            set
            {
                TimeTagsBindable.Value = value;
                LyricStartTime = TimeTagsUtils.GetStartTime(TimeTags) ?? StartTime;
                LyricEndTime = TimeTagsUtils.GetEndTime(TimeTags) ?? EndTime;
            }
        }

        [JsonIgnore]
        public double LyricStartTime { get; private set; }

        [JsonIgnore]
        public double LyricEndTime { get; private set; }

        [JsonIgnore]
        public double LyricDuration => LyricEndTime - LyricStartTime;

        [JsonIgnore]
        public readonly Bindable<RubyTag[]> RubyTagsBindable = new();

        /// <summary>
        /// List of ruby tags
        /// </summary>
        public RubyTag[] RubyTags
        {
            get => RubyTagsBindable.Value;
            set => RubyTagsBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<RomajiTag[]> RomajiTagsBindable = new();

        /// <summary>
        /// List of ruby tags
        /// </summary>
        public RomajiTag[] RomajiTags
        {
            get => RomajiTagsBindable.Value;
            set => RomajiTagsBindable.Value = value;
        }

        /// <summary>
        /// Lyric's start time is created from <see cref="KaraokeBeatmapProcessor"/> and should not be saved.
        /// </summary>
        public override double StartTime
        {
            get => base.StartTime;
            set => base.StartTime = value;
        }

        /// <summary>
        /// Lyric's duration is created from <see cref="KaraokeBeatmapProcessor"/> and should not be saved.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// The time at which the HitObject end.
        /// </summary>
        public double EndTime => StartTime + Duration;

        [JsonIgnore]
        public readonly Bindable<int[]> SingersBindable = new();

        /// <summary>
        /// Singers
        /// </summary>
        public int[] Singers
        {
            get => SingersBindable.Value;
            set => SingersBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<int> LayoutIndexBindable = new();

        /// <summary>
        /// Layout index
        /// </summary>
        [JsonIgnore]
        public int LayoutIndex
        {
            get => LayoutIndexBindable.Value;
            set => LayoutIndexBindable.Value = value;
        }

        [JsonIgnore]
        public readonly BindableDictionary<CultureInfo, string> TranslateTextBindable = new();

        /// <summary>
        /// Translates
        /// </summary>
        public IDictionary<CultureInfo, string> Translates
        {
            get => TranslateTextBindable;
            set
            {
                TranslateTextBindable.Clear();
                TranslateTextBindable.AddRange(value);
            }
        }

        [JsonIgnore]
        public readonly Bindable<CultureInfo> LanguageBindable = new();

        /// <summary>
        /// Language
        /// </summary>
        public CultureInfo Language
        {
            get => LanguageBindable.Value;
            set => LanguageBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<int> OrderBindable = new();

        /// <summary>
        /// Order
        /// </summary>
        public int Order
        {
            get => OrderBindable.Value;
            set => OrderBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<LockState> LockBindable = new();

        /// <summary>
        /// Lock
        /// </summary>
        public LockState Lock
        {
            get => LockBindable.Value;
            set => LockBindable.Value = value;
        }

        public override Judgement CreateJudgement() => new KaraokeLyricJudgement();

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, IBeatmapDifficultyInfo difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            // Add because it will cause error on exit then enter gameplay.
            StartTimeBindable.UnbindAll();

            // Initial working start and end time.
            InitialWorkingTime();
        }

        public void InitialWorkingTime()
        {
            StartTime = LyricStartTime;
            Duration = LyricDuration;
        }
    }
}
