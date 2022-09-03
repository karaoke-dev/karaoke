// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Language;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Generator.ReferenceLyric;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricAutoGenerateChangeHandler : LyricPropertyChangeHandler, ILyricAutoGenerateChangeHandler
    {
        [Resolved, AllowNull]
        private KaraokeRulesetEditGeneratorConfigManager generatorConfigManager { get; set; }

        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

        // should change this flag if wants to change property in the lyrics.
        // Not a good to waite a global property for that but there's no better choice.
        private LyricAutoGenerateProperty? currentAutoGenerateProperty;

        public bool CanGenerate(LyricAutoGenerateProperty autoGenerateProperty)
        {
            currentAutoGenerateProperty = autoGenerateProperty;

            switch (autoGenerateProperty)
            {
                case LyricAutoGenerateProperty.DetectReferenceLyric:
                    var referenceLyricDetector = createLyricDetector<Lyric>();
                    return canDetect(referenceLyricDetector);

                case LyricAutoGenerateProperty.DetectLanguage:
                    var languageDetector = createLyricDetector<CultureInfo>();
                    return canDetect(languageDetector);

                case LyricAutoGenerateProperty.AutoGenerateRubyTags:
                    var rubyGenerator = createLyricGenerator<RubyTag[]>();
                    return canGenerate(rubyGenerator);

                case LyricAutoGenerateProperty.AutoGenerateRomajiTags:
                    var romajiGenerator = createLyricGenerator<RomajiTag[]>();
                    return canGenerate(romajiGenerator);

                case LyricAutoGenerateProperty.AutoGenerateTimeTags:
                    var timeTagGenerator = createLyricGenerator<TimeTag[]>();
                    return canGenerate(timeTagGenerator);

                case LyricAutoGenerateProperty.AutoGenerateNotes:
                    var noteGenerator = createLyricGenerator<Note[]>();
                    return canGenerate(noteGenerator);

                default:
                    throw new ArgumentOutOfRangeException(nameof(autoGenerateProperty));
            }

            bool canDetect<T>(ILyricPropertyDetector<T> detector)
                => HitObjects.Where(x => !IsWritePropertyLocked(x)).Any(detector.CanDetect);

            bool canGenerate<T>(ILyricPropertyGenerator<T> generator)
                => HitObjects.Where(x => !IsWritePropertyLocked(x)).Any(generator.CanGenerate);
        }

        public IDictionary<Lyric, LocalisableString> GetNotGeneratableLyrics(LyricAutoGenerateProperty autoGenerateProperty)
        {
            currentAutoGenerateProperty = autoGenerateProperty;

            switch (autoGenerateProperty)
            {
                case LyricAutoGenerateProperty.DetectReferenceLyric:
                    var referenceLyricDetector = createLyricDetector<Lyric>();
                    return getInvalidMessageFromDetector(referenceLyricDetector);

                case LyricAutoGenerateProperty.DetectLanguage:
                    var languageDetector = createLyricDetector<CultureInfo>();
                    return getInvalidMessageFromDetector(languageDetector);

                case LyricAutoGenerateProperty.AutoGenerateRubyTags:
                    var rubyGenerator = createLyricGenerator<RubyTag[]>();
                    return getInvalidMessageFromGenerator(rubyGenerator);

                case LyricAutoGenerateProperty.AutoGenerateRomajiTags:
                    var romajiGenerator = createLyricGenerator<RomajiTag[]>();
                    return getInvalidMessageFromGenerator(romajiGenerator);

                case LyricAutoGenerateProperty.AutoGenerateTimeTags:
                    var timeTagGenerator = createLyricGenerator<TimeTag[]>();
                    return getInvalidMessageFromGenerator(timeTagGenerator);

                case LyricAutoGenerateProperty.AutoGenerateNotes:
                    var noteGenerator = createLyricGenerator<Note[]>();
                    return getInvalidMessageFromGenerator(noteGenerator);

                default:
                    throw new ArgumentOutOfRangeException(nameof(autoGenerateProperty));
            }

            IDictionary<Lyric, LocalisableString> getInvalidMessageFromDetector<T>(ILyricPropertyDetector<T> detector)
                => HitObjects.Select(x => new KeyValuePair<Lyric, LocalisableString?>(x, detector.GetInvalidMessage(x) ?? getReferenceLyricInvalidMessage(x)))
                             .Where(x => x.Value != null)
                             .ToDictionary(k => k.Key, v => v.Value!.Value);

            IDictionary<Lyric, LocalisableString> getInvalidMessageFromGenerator<T>(ILyricPropertyGenerator<T> generator)
                => HitObjects.Select(x => new KeyValuePair<Lyric, LocalisableString?>(x, generator.GetInvalidMessage(x) ?? getReferenceLyricInvalidMessage(x)))
                             .Where(x => x.Value != null)
                             .ToDictionary(k => k.Key, v => v.Value!.Value);

            LocalisableString? getReferenceLyricInvalidMessage(Lyric lyric)
            {
                bool locked = IsWritePropertyLocked(lyric);
                return locked ? "Cannot modify property because has reference lyric." : default(LocalisableString?);
            }
        }

        public void AutoGenerate(LyricAutoGenerateProperty autoGenerateProperty)
        {
            currentAutoGenerateProperty = autoGenerateProperty;

            switch (autoGenerateProperty)
            {
                case LyricAutoGenerateProperty.DetectReferenceLyric:
                    var referenceLyricDetector = createLyricDetector<Lyric>();
                    PerformOnSelection(lyric =>
                    {
                        var detectedLanguage = referenceLyricDetector.Detect(lyric);
                        lyric.ReferenceLyric = detectedLanguage;

                        if (lyric.ReferenceLyric != null && lyric.ReferenceLyricConfig is not SyncLyricConfig)
                            lyric.ReferenceLyricConfig = new SyncLyricConfig();
                    });
                    break;

                case LyricAutoGenerateProperty.DetectLanguage:
                    var languageDetector = createLyricDetector<CultureInfo>();
                    PerformOnSelection(lyric =>
                    {
                        var detectedLanguage = languageDetector.Detect(lyric);
                        lyric.Language = detectedLanguage;
                    });
                    break;

                case LyricAutoGenerateProperty.AutoGenerateRubyTags:
                    var rubyGenerator = createLyricGenerator<RubyTag[]>();
                    PerformOnSelection(lyric =>
                    {
                        lyric.RubyTags = rubyGenerator.Generate(lyric);
                    });
                    break;

                case LyricAutoGenerateProperty.AutoGenerateRomajiTags:
                    var romajiGenerator = createLyricGenerator<RomajiTag[]>();
                    PerformOnSelection(lyric =>
                    {
                        lyric.RomajiTags = romajiGenerator.Generate(lyric);
                    });
                    break;

                case LyricAutoGenerateProperty.AutoGenerateTimeTags:
                    var timeTagGenerator = createLyricGenerator<TimeTag[]>();
                    PerformOnSelection(lyric =>
                    {
                        lyric.TimeTags = timeTagGenerator.Generate(lyric);
                    });
                    break;

                case LyricAutoGenerateProperty.AutoGenerateNotes:
                    var noteGenerator = createLyricGenerator<Note[]>();
                    PerformOnSelection(lyric =>
                    {
                        // clear exist notes if from those
                        var matchedNotes = EditorBeatmapUtils.GetNotesByLyric(beatmap, lyric);
                        RemoveRange(matchedNotes);

                        var notes = noteGenerator.Generate(lyric);
                        AddRange(notes);
                    });
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(autoGenerateProperty));
            }
        }

        private ILyricPropertyDetector<T> createLyricDetector<T>()
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(Lyric):
                    var lyrics = beatmap.HitObjects.OfType<Lyric>().ToArray();
                    var referenceLyricDetectorConfig = generatorConfigManager.Get<ReferenceLyricDetectorConfig>(KaraokeRulesetEditGeneratorSetting.ReferenceLyricDetectorConfig);
                    return (ILyricPropertyDetector<T>)new ReferenceLyricDetector(lyrics, referenceLyricDetectorConfig);

                case Type t when t == typeof(CultureInfo):
                    var languageDetectorConfig = generatorConfigManager.Get<LanguageDetectorConfig>(KaraokeRulesetEditGeneratorSetting.LanguageDetectorConfig);
                    return (ILyricPropertyDetector<T>)new LanguageDetector(languageDetectorConfig);

                default:
                    throw new NotSupportedException();
            }
        }

        private ILyricPropertyGenerator<T> createLyricGenerator<T>()
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(RubyTag[]):
                    return (ILyricPropertyGenerator<T>)new RubyTagGeneratorSelector(generatorConfigManager);

                case Type t when t == typeof(RomajiTag[]):
                    return (ILyricPropertyGenerator<T>)new RomajiTagGeneratorSelector(generatorConfigManager);

                case Type t when t == typeof(TimeTag[]):
                    return (ILyricPropertyGenerator<T>)new TimeTagGeneratorSelector(generatorConfigManager);

                case Type t when t == typeof(Note[]):
                    var config = generatorConfigManager.Get<NoteGeneratorConfig>(KaraokeRulesetEditGeneratorSetting.NoteGeneratorConfig);
                    return (ILyricPropertyGenerator<T>)new NoteGenerator(config);

                default:
                    throw new NotSupportedException();
            }
        }

        protected override bool IsWritePropertyLocked(Lyric lyric) =>
            currentAutoGenerateProperty switch
            {
                LyricAutoGenerateProperty.DetectReferenceLyric => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.ReferenceLyric), nameof(Lyric.ReferenceLyricConfig)),
                LyricAutoGenerateProperty.DetectLanguage => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.Language)),
                LyricAutoGenerateProperty.AutoGenerateRubyTags => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RubyTags)),
                LyricAutoGenerateProperty.AutoGenerateRomajiTags => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RomajiTags)),
                LyricAutoGenerateProperty.AutoGenerateTimeTags => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.TimeTags)),
                LyricAutoGenerateProperty.AutoGenerateNotes => HitObjectWritableUtils.IsCreateOrRemoveNoteLocked(lyric),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
