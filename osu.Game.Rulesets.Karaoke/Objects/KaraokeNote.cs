// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Objects.Types;
using System;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class KaraokeNote : KaraokeHitObject, IHasEndTime, IHasText
    {
        /// <summary>
        /// Text display on the note
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Font index
        /// </summary>
        public int FontIndex { get; set; }

        /// <summary>
        /// Display this note
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// Tone of this note
        /// </summary>
        public int Tone { get; set; }

        /// <summary>
        /// Half tone
        /// </summary>
        public bool Half { get; set; }

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

        public KaraokeNote CopyByPercentage(double startPercentage = 0,double durationPercentage = 0.5)
        {
            if (startPercentage < 0 || startPercentage + durationPercentage > 1)
                throw new ArgumentOutOfRangeException($"{nameof(KaraokeNote)} cannot assign split range of start from {startPercentage} and duration {durationPercentage}");

            var startTime = StartTime + Duration * startPercentage;
            var duration = Duration * durationPercentage;

            return CopyByTime(startTime, duration);
        }

        public KaraokeNote CopyByTime(double startTime, double duration)
        {
            if (startTime < StartTime || startTime + duration > EndTime)
                throw new ArgumentOutOfRangeException($"{nameof(KaraokeNote)} cannot assign split range of start from {startTime} and duration {duration}");

            return new KaraokeNote
            {
                StartTime = startTime,
                EndTime = startTime + duration,
                Text = Text,
                FontIndex = FontIndex,
                Display = Display,
                Tone = Tone,
                Half = Half,
            };
        }
    }
}
