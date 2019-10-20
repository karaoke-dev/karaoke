// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class LyricLine : KaraokeHitObject, IHasEndTime, IHasText
    {
        /// <summary>
        /// Text of the lyric
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// List of time tags
        /// </summary>
        public TimeTag[] TimeTags { get; set; }

        /// <summary>
        /// Filter time tag that has value
        /// </summary>
        [JsonIgnore]
        protected TimeTag[] AvailableTimeTags => TimeTags?.Where(x => x.StartTime > 0).ToArray();

        /// <summary>
        /// List of ruby tags
        /// </summary>
        public RubyTag[] RubyTags { get; set; }

        /// <summary>
        /// List of ruby tags
        /// </summary>
        public RomajiTag[] RomajiTags { get; set; }

        /// <summary>
        /// Duration
        /// </summary>
        public double Duration => EndTime - StartTime;

        /// <summary>
        /// The time at which the HitObject end.
        /// </summary>
        public double EndTime { get; set; }

        /// <summary>
        /// Font index
        /// </summary>
        public int FontIndex { get; set; }

        /// <summary>
        /// Layout index
        /// </summary>
        [JsonIgnore]
        public int LayoutIndex { get; set; }

        /// <summary>
        /// Translate text
        /// </summary>
        [JsonIgnore]
        public string TranslateText { get; set; }

        public DisplayPercentage GetPercentageByTime(double time)
        {
            if (time < StartTime)
                return new DisplayPercentage(endIndex: 0);

            if (time > EndTime)
                return new DisplayPercentage(startIndex: int.MaxValue);

            var startTagTime = AvailableTimeTags?.LastOrDefault(x => x.StartTime < time) ?? AvailableTimeTags?.FirstOrDefault();
            var endTagTime = AvailableTimeTags?.FirstOrDefault(x => x.StartTime > time) ?? AvailableTimeTags?.LastOrDefault();

            int startIndex = getTextIndexByTimeTag(startTagTime);
            int endIndex = getTextIndexByTimeTag(endTagTime);

            var precentage = (time - startTagTime.StartTime) / (endTagTime.StartTime - startTagTime.StartTime);
            if (precentage > 1)
                precentage = 1;

            return new DisplayPercentage
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                TextPercentage = precentage
            };
        }

        public IEnumerable<KaraokeNote> CreateDefaultNotes()
        {
            for (int i = 0; i < AvailableTimeTags.Length - 1; i++)
            {
                var startTag = AvailableTimeTags[i];
                var endTag = AvailableTimeTags[i + 1];

                int startIndex = getTextIndexByTimeTag(startTag);
                int endIndex = getTextIndexByTimeTag(endTag);

                var text = Text.Substring(startIndex, endIndex - startIndex);
                if (!string.IsNullOrEmpty(text))
                    yield return new KaraokeNote
                    {
                        StartTime = startTag.StartTime,
                        EndTime = endTag.StartTime,
                        Text = text,
                    };
            }
        }

        private int getTextIndexByTimeTag(TimeTag timeTag) => (int)Math.Ceiling((double)(Array.IndexOf(TimeTags, timeTag) - 1) / 2);

        public struct DisplayPercentage
        {
            public DisplayPercentage(int startIndex = -1, int endIndex = int.MaxValue, float percentage = 0)
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
                TextPercentage = percentage;
            }

            public int StartIndex { get; set; }

            public int EndIndex { get; set; }

            public double TextPercentage { get; set; }

            public bool Available => StartIndex >= 0 && EndIndex < int.MaxValue;
        }
    }
}
