// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Objects.Types;
using System;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class Note : KaraokeHitObject, IHasEndTime, IHasText
    {
        public readonly Bindable<string> TextBindable = new Bindable<string>();

        /// <summary>
        /// Text display on the note
        /// </summary>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        public readonly Bindable<string> AlternativeTextBindable = new Bindable<string>();

        /// <summary>
        /// Will be display if <see cref="KaraokeRulesetSetting.DisplayAlternativeText"/> is true
        /// </summary>
        public string AlternativeText
        {
            get => AlternativeTextBindable.Value;
            set => AlternativeTextBindable.Value = value;
        }

        public readonly Bindable<int> StyleIndexBindable = new Bindable<int>();

        public int StyleIndex
        {
            get => StyleIndexBindable.Value;
            set => StyleIndexBindable.Value = value;
        }

        public readonly Bindable<bool> DisplayBindable = new Bindable<bool>();

        /// <summary>
        /// Display this note
        /// </summary>
        public bool Display
        {
            get => DisplayBindable.Value;
            set => DisplayBindable.Value = value;
        }

        public readonly Bindable<Tone> ToneBindable = new Bindable<Tone>();

        /// <summary>
        /// Tone of this note
        /// </summary>
        public virtual Tone Tone
        {
            get => ToneBindable.Value;
            set => ToneBindable.Value = value;
        }

        /// <summary>
        /// Duration
        /// </summary>
        [JsonIgnore]
        public double Duration => EndTime - StartTime;

        /// <summary>
        /// The time at which the HitObject end.
        /// </summary>
        [JsonIgnore]
        public double EndTime { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public LyricLine ParentLyric { get; set; }

        public Note CopyByPercentage(double startPercentage = 0, double durationPercentage = 0.5)
        {
            if (startPercentage < 0 || startPercentage + durationPercentage > 1)
                throw new ArgumentOutOfRangeException($"{nameof(Note)} cannot assign split range of start from {startPercentage} and duration {durationPercentage}");

            var startTime = StartTime + Duration * startPercentage;
            var duration = Duration * durationPercentage;

            return CopyByTime(startTime, duration);
        }

        public Note CopyByTime(double startTime, double duration)
        {
            if (startTime < StartTime || startTime + duration > EndTime)
                throw new ArgumentOutOfRangeException($"{nameof(Note)} cannot assign split range of start from {startTime} and duration {duration}");

            return new Note
            {
                StartTime = startTime,
                EndTime = startTime + duration,
                StartIndex = StartIndex,
                EndIndex = EndIndex,
                Text = Text,
                StyleIndex = StyleIndex,
                Display = Display,
                Tone = Tone,
                ParentLyric = ParentLyric
            };
        }

        public override Judgement CreateJudgement() => new NoteJudgement();
    }
}
