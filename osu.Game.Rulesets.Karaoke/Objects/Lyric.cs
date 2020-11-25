// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
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
    public class Lyric : KaraokeHitObject, IHasDuration, IHasSingers
    {
        public readonly Bindable<string> TextBindable = new Bindable<string>();

        /// <summary>
        /// Text of the lyric
        /// </summary>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<Tuple<TimeTagIndex, double?>[]> TimeTagsBindable = new Bindable<Tuple<TimeTagIndex, double?>[]>();

        /// <summary>
        /// Time tags
        /// </summary>
        public Tuple<TimeTagIndex, double?>[] TimeTags
        {
            get => TimeTagsBindable.Value;
            set => TimeTagsBindable.Value = value;
        }

        public double LyricStartTime => TimeTagsUtils.GetStartTime(TimeTags) ?? StartTime;

        public double LyricEndTime => TimeTagsUtils.GetEndTime(TimeTags) ?? EndTime;

        public double LyricDuration => LyricEndTime - LyricStartTime;

        [JsonIgnore]
        public readonly Bindable<RubyTag[]> RubyTagsBindable = new Bindable<RubyTag[]>();

        /// <summary>
        /// List of ruby tags
        /// </summary>
        public RubyTag[] RubyTags
        {
            get => RubyTagsBindable.Value;
            set => RubyTagsBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<RomajiTag[]> RomajiTagsBindable = new Bindable<RomajiTag[]>();

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
        public readonly Bindable<int[]> SingersBindable = new Bindable<int[]>();

        /// <summary>
        /// Singers
        /// </summary>
        public int[] Singers
        {
            get => SingersBindable.Value;
            set => SingersBindable.Value = value;
        }

        [JsonIgnore]
        public readonly Bindable<int> LayoutIndexBindable = new Bindable<int>();

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
        public readonly Bindable<string> TranslateTextBindable = new Bindable<string>();

        public CultureInfo Language { get; set; }

        /// <summary>
        /// Translates
        /// </summary>
        public IDictionary<int, string> Translates { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// Display target translate
        /// </summary>
        /// <param name="id"></param>
        public bool ApplyDisplayTranslate(int id)
        {
            if (Translates.TryGetValue(id, out string translate))
            {
                TranslateTextBindable.Value = translate;
                return true;
            }

            return false;
        }

        public IEnumerable<Note> CreateDefaultNotes()
        {
            var timeTags = TimeTagsUtils.ToDictionary(TimeTags);

            foreach (var timeTag in timeTags)
            {
                var (key, endTime) = timeTags.GetNext(timeTag);

                if (key.Index <= 0)
                    continue;

                var startTime = timeTag.Value;

                int startIndex = timeTag.Key.Index;
                int endIndex = key.Index;

                var text = Text.Substring(startIndex, endIndex - startIndex);
                var ruby = RubyTags?.Where(x => x.StartIndex == startIndex && x.EndIndex == endIndex).FirstOrDefault().Text;

                if (!string.IsNullOrEmpty(text))
                {
                    yield return new Note
                    {
                        StartTime = startTime,
                        Duration = endTime - startTime,
                        StartIndex = startIndex,
                        EndIndex = endIndex,
                        Text = text,
                        AlternativeText = ruby,
                        ParentLyric = this
                    };
                }
            }
        }

        public override Judgement CreateJudgement() => new KaraokeLyricJudgement();

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
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
