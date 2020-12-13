// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class Note : KaraokeHitObject, IHasDuration, IHasText, IHasSingers
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
        public double Duration { get; set; }

        /// <summary>
        /// The time at which the HitObject end.
        /// </summary>
        [JsonIgnore]
        public double EndTime => StartTime + Duration;

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public Lyric ParentLyric { get; set; }

        public override Judgement CreateJudgement() => new KaraokeNoteJudgement();
    }
}
