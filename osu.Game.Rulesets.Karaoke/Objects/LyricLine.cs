// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class LyricLine : KaraokeHitObject, IHasEndTime
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
        public readonly Bindable<IReadOnlyDictionary<TimeTagIndex, double>> TimeTagsBindable = new Bindable<IReadOnlyDictionary<TimeTagIndex, double>>();

        /// <summary>
        /// Time tags
        /// </summary>
        public IReadOnlyDictionary<TimeTagIndex, double> TimeTags
        {
            get => TimeTagsBindable.Value;
            set => TimeTagsBindable.Value = value;
        }

        public double LyricStartTime => TimeTags?.FirstOrDefault().Value ?? StartTime;

        public double LyricEndTime => TimeTags?.LastOrDefault().Value ?? EndTime;

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
        /// Duration
        /// </summary>
        public double Duration => EndTime - StartTime;

        /// <summary>
        /// The time at which the HitObject end.
        /// </summary>
        public double EndTime { get; set; }

        [JsonIgnore]
        public readonly Bindable<int> FontIndexBindable = new Bindable<int>();

        /// <summary>
        /// Font index
        /// </summary>
        public int FontIndex
        {
            get => FontIndexBindable.Value;
            set => FontIndexBindable.Value = value;
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

        /// <summary>
        /// Translate text
        /// </summary>
        [JsonIgnore]
        public string TranslateText
        {
            get => TranslateTextBindable.Value;
            set => TranslateTextBindable.Value = value;
        }

        /// <summary>
        /// Get all nasted note by this list
        /// </summary>
        public IReadOnlyList<Note> Notes => NestedHitObjects.OfType<Note>().ToList();

        /// <summary>
        /// Created default nasted note
        /// </summary>
        public void CreateDefaultNotes()
        {
            foreach (var timeTag in TimeTags)
            {
                var (key, endTime) = TimeTags.GetNext(timeTag);

                if (key.Index <= 0)
                    continue;

                var startTime = timeTag.Value;

                int startIndex = timeTag.Key.Index;
                int endIndex = key.Index;

                var text = Text.Substring(startIndex, endIndex - startIndex);
                var ruby = RubyTags?.Where(x => x.StartIndex == startIndex && x.EndIndex == endIndex).FirstOrDefault().Text;

                if (!string.IsNullOrEmpty(text))
                {
                    AddNested(new Note
                    {
                        StartTime = startTime,
                        EndTime = endTime,
                        StartIndex = startIndex,
                        EndIndex = endIndex,
                        Text = text,
                        AlternativeText = ruby,
                        ParentLyric = this
                    });
                }
            }
        }

        private List<Note> tempNotes = new List<Note>();

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            tempNotes.AddRange(Notes);
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);
        }

        protected override void CreateNestedHitObjects()
        {
            foreach (var note in tempNotes)
            {
                AddNested(note);
            }
            base.CreateNestedHitObjects();
        }

        public override Judgement CreateJudgement() => new KaraokeLyricJudgement();
    }
}
