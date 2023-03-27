// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Extensions;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Objects.Workings;
using osu.Game.Rulesets.Karaoke.Scoring;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public partial class Lyric : KaraokeHitObject, IHasPage, IHasDuration, IHasSingers, IHasOrder, IHasLock, IHasPrimaryKey, IDeepCloneable<Lyric>
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public int ID { get; set; }

        [JsonIgnore]
        public readonly Bindable<string> TextBindable = new(string.Empty);

        /// <summary>
        /// Text of the lyric
        /// </summary>
        public string Text
        {
            get => TextBindable.Value;
            set => TextBindable.Value = value;
        }

        [JsonIgnore]
        public IBindable<int> TimeTagsVersion => timeTagsVersion;

        private readonly Bindable<int> timeTagsVersion = new();

        [JsonIgnore]
        public readonly BindableList<TimeTag> TimeTagsBindable = new();

        /// <summary>
        /// Time tags
        /// </summary>
        public IList<TimeTag> TimeTags
        {
            get => TimeTagsBindable;
            set
            {
                TimeTagsBindable.Clear();
                TimeTagsBindable.AddRange(value);
            }
        }

        [JsonIgnore]
        public IBindable<int> RubyTagsVersion => rubyTagsVersion;

        private readonly Bindable<int> rubyTagsVersion = new();

        [JsonIgnore]
        public readonly BindableList<RubyTag> RubyTagsBindable = new();

        /// <summary>
        /// List of ruby tags
        /// </summary>
        public IList<RubyTag> RubyTags
        {
            get => RubyTagsBindable;
            set
            {
                RubyTagsBindable.Clear();
                RubyTagsBindable.AddRange(value);
            }
        }

        [JsonIgnore]
        public IBindable<int> RomajiTagsVersion => romajiTagsVersion;

        private readonly Bindable<int> romajiTagsVersion = new();

        [JsonIgnore]
        public readonly BindableList<RomajiTag> RomajiTagsBindable = new();

        /// <summary>
        /// List of ruby tags
        /// </summary>
        public IList<RomajiTag> RomajiTags
        {
            get => RomajiTagsBindable;
            set
            {
                RomajiTagsBindable.Clear();
                RomajiTagsBindable.AddRange(value);
            }
        }

        [JsonIgnore]
        public readonly BindableList<int> SingersBindable = new();

        /// <summary>
        /// Singers
        /// </summary>
        public IList<int> Singers
        {
            get => SingersBindable;
            set
            {
                SingersBindable.Clear();
                SingersBindable.AddRange(value);
            }
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
        public readonly Bindable<CultureInfo?> LanguageBindable = new();

        /// <summary>
        /// Language
        /// </summary>
        public CultureInfo? Language
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

        private int? referenceLyricId;

        public int? ReferenceLyricId
        {
            get => referenceLyricId;
            set
            {
                referenceLyricId = value;
                InvalidateWorkingProperty(LyricWorkingProperty.ReferenceLyric);
            }
        }

        [JsonIgnore]
        public IBindable<int> ReferenceLyricConfigVersion => referenceLyricConfigVersion;

        private readonly Bindable<int> referenceLyricConfigVersion = new();

        [JsonIgnore]
        public readonly Bindable<IReferenceLyricPropertyConfig?> ReferenceLyricConfigBindable = new();

        /// <summary>
        /// Config for define the strategy to sync the property from the lyric.
        /// </summary>
        public IReferenceLyricPropertyConfig? ReferenceLyricConfig
        {
            get => ReferenceLyricConfigBindable.Value;
            set => ReferenceLyricConfigBindable.Value = value;
        }

        [JsonIgnore]
        public IBindable<int> LyricPropertyWritableVersion => lyricPropertyWritableVersion;

        private readonly Bindable<int> lyricPropertyWritableVersion = new();

        public Lyric()
        {
            workingPropertyValidator = new LyricWorkingPropertyValidator(this);

            initInternalBindingEvent();
            initReferenceLyricEvent();
        }

        public override Judgement CreateJudgement() => new KaraokeLyricJudgement();

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, IBeatmapDifficultyInfo difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            // Add because it will cause error on exit then enter gameplay.
            StartTimeBindable.UnbindAll();
        }

        protected override HitWindows CreateHitWindows() => new KaraokeLyricHitWindows();

        public Lyric DeepClone()
        {
            string serializeString = JsonConvert.SerializeObject(this, KaraokeJsonSerializableExtensions.CreateGlobalSettings());
            var lyric = JsonConvert.DeserializeObject<Lyric>(serializeString, KaraokeJsonSerializableExtensions.CreateGlobalSettings())!;

            lyric.StartTime = StartTime;
            lyric.Duration = Duration;
            lyric.ReferenceLyric = ReferenceLyric;

            return lyric;
        }
    }
}
