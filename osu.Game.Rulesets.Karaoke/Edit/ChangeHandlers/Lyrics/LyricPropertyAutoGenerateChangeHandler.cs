// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Language;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.ReferenceLyric;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romajies;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

public partial class LyricPropertyAutoGenerateChangeHandler : LyricPropertyChangeHandler, ILyricPropertyAutoGenerateChangeHandler
{
    // should change this flag if wants to change property in the lyrics.
    // Not a good to waite a global property for that but there's no better choice.
    private AutoGenerateType? currentAutoGenerateType;

    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    public bool CanGenerate(AutoGenerateType type)
    {
        currentAutoGenerateType = type;

        switch (type)
        {
            case AutoGenerateType.DetectReferenceLyric:
                var referenceLyricDetector = getDetector<Lyric?, ReferenceLyricDetectorConfig>(HitObjects);
                return canDetect(referenceLyricDetector);

            case AutoGenerateType.DetectLanguage:
                var languageDetector = getDetector<CultureInfo, LanguageDetectorConfig>();
                return canDetect(languageDetector);

            case AutoGenerateType.AutoGenerateRubyTags:
                var rubyGenerator = getSelector<RubyTag[], RubyTagGeneratorConfig>();
                return canGenerate(rubyGenerator);

            case AutoGenerateType.AutoGenerateRomajiTags:
                var romajiGenerator = getSelector<RomajiTag[], RomajiTagGeneratorConfig>();
                return canGenerate(romajiGenerator);

            case AutoGenerateType.AutoGenerateTimeTags:
                var timeTagGenerator = getSelector<TimeTag[], TimeTagGeneratorConfig>();
                return canGenerate(timeTagGenerator);

            case AutoGenerateType.AutoGenerateTimeTagRomaji:
                var timeTagRomajiGenerator = getSelector<IReadOnlyDictionary<TimeTag, RomajiGenerateResult>, RomajiGeneratorConfig>();
                return canGenerate(timeTagRomajiGenerator);

            case AutoGenerateType.AutoGenerateNotes:
                var noteGenerator = getGenerator<Note[], NoteGeneratorConfig>();
                return canGenerate(noteGenerator);

            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }

        bool canDetect<T>(PropertyDetector<Lyric, T> detector)
            => HitObjects.Where(x => !IsWritePropertyLocked(x)).Any(detector.CanDetect);

        bool canGenerate<T>(PropertyGenerator<Lyric, T> generator)
            => HitObjects.Where(x => !IsWritePropertyLocked(x)).Any(generator.CanGenerate);
    }

    public IDictionary<Lyric, LocalisableString> GetGeneratorNotSupportedLyrics(AutoGenerateType type)
    {
        currentAutoGenerateType = type;

        switch (type)
        {
            case AutoGenerateType.DetectReferenceLyric:
                var referenceLyricDetector = getDetector<Lyric?, ReferenceLyricDetectorConfig>(HitObjects);
                return getInvalidMessageFromDetector(referenceLyricDetector);

            case AutoGenerateType.DetectLanguage:
                var languageDetector = getDetector<CultureInfo, LanguageDetectorConfig>();
                return getInvalidMessageFromDetector(languageDetector);

            case AutoGenerateType.AutoGenerateRubyTags:
                var rubyGenerator = getSelector<RubyTag[], RubyTagGeneratorConfig>();
                return getInvalidMessageFromGenerator(rubyGenerator);

            case AutoGenerateType.AutoGenerateRomajiTags:
                var romajiGenerator = getSelector<RomajiTag[], RomajiTagGeneratorConfig>();
                return getInvalidMessageFromGenerator(romajiGenerator);

            case AutoGenerateType.AutoGenerateTimeTags:
                var timeTagGenerator = getSelector<TimeTag[], TimeTagGeneratorConfig>();
                return getInvalidMessageFromGenerator(timeTagGenerator);

            case AutoGenerateType.AutoGenerateTimeTagRomaji:
                var timeTagRomajiGenerator = getSelector<IReadOnlyDictionary<TimeTag, RomajiGenerateResult>, RomajiGeneratorConfig>();
                return getInvalidMessageFromGenerator(timeTagRomajiGenerator);

            case AutoGenerateType.AutoGenerateNotes:
                var noteGenerator = getGenerator<Note[], NoteGeneratorConfig>();
                return getInvalidMessageFromGenerator(noteGenerator);

            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }

        IDictionary<Lyric, LocalisableString> getInvalidMessageFromDetector<T>(PropertyDetector<Lyric, T> detector)
            => HitObjects.Select(x => new KeyValuePair<Lyric, LocalisableString?>(x, detector.GetInvalidMessage(x) ?? getReferenceLyricInvalidMessage(x)))
                         .Where(x => x.Value != null)
                         .ToDictionary(k => k.Key, v => v.Value!.Value);

        IDictionary<Lyric, LocalisableString> getInvalidMessageFromGenerator<T>(PropertyGenerator<Lyric, T> generator)
            => HitObjects.Select(x => new KeyValuePair<Lyric, LocalisableString?>(x, generator.GetInvalidMessage(x) ?? getReferenceLyricInvalidMessage(x)))
                         .Where(x => x.Value != null)
                         .ToDictionary(k => k.Key, v => v.Value!.Value);

        LocalisableString? getReferenceLyricInvalidMessage(Lyric lyric)
        {
            bool locked = IsWritePropertyLocked(lyric);
            return locked ? "Cannot modify property because has reference lyric." : default(LocalisableString?);
        }
    }

    public void AutoGenerate(AutoGenerateType type)
    {
        currentAutoGenerateType = type;

        switch (type)
        {
            case AutoGenerateType.DetectReferenceLyric:
                var referenceLyricDetector = getDetector<Lyric?, ReferenceLyricDetectorConfig>(HitObjects);
                PerformOnSelection(lyric =>
                {
                    var referencedLyric = referenceLyricDetector.Detect(lyric);
                    lyric.ReferenceLyricId = referencedLyric?.ID;

                    // technically this property should be assigned by beatmap processor, but should be OK to assign here for testing purpose.
                    lyric.ReferenceLyric = referencedLyric;

                    if (lyric.ReferenceLyricId != null && lyric.ReferenceLyricConfig is not SyncLyricConfig)
                        lyric.ReferenceLyricConfig = new SyncLyricConfig();
                });
                break;

            case AutoGenerateType.DetectLanguage:
                var languageDetector = getDetector<CultureInfo, LanguageDetectorConfig>();
                PerformOnSelection(lyric =>
                {
                    var detectedLanguage = languageDetector.Detect(lyric);
                    lyric.Language = detectedLanguage;
                });
                break;

            case AutoGenerateType.AutoGenerateRubyTags:
                var rubyGenerator = getSelector<RubyTag[], RubyTagGeneratorConfig>();
                PerformOnSelection(lyric =>
                {
                    lyric.RubyTags = rubyGenerator.Generate(lyric);
                });
                break;

            case AutoGenerateType.AutoGenerateRomajiTags:
                var romajiGenerator = getSelector<RomajiTag[], RomajiTagGeneratorConfig>();
                PerformOnSelection(lyric =>
                {
                    lyric.RomajiTags = romajiGenerator.Generate(lyric);
                });
                break;

            case AutoGenerateType.AutoGenerateTimeTags:
                var timeTagGenerator = getSelector<TimeTag[], TimeTagGeneratorConfig>();
                PerformOnSelection(lyric =>
                {
                    lyric.TimeTags = timeTagGenerator.Generate(lyric);
                });
                break;

            case AutoGenerateType.AutoGenerateTimeTagRomaji:
                var timeTagRomajiGenerator = getSelector<IReadOnlyDictionary<TimeTag, RomajiGenerateResult>, RomajiGeneratorConfig>();
                PerformOnSelection(lyric =>
                {
                    var results = timeTagRomajiGenerator.Generate(lyric);

                    foreach (var (key, value) in results)
                    {
                        var matchedTimeTag = lyric.TimeTags.Single(x => x == key);
                        matchedTimeTag.InitialRomaji = value.InitialRomaji;
                        matchedTimeTag.RomajiText = value.RomajiText;
                    }
                });
                break;

            case AutoGenerateType.AutoGenerateNotes:
                var noteGenerator = getGenerator<Note[], NoteGeneratorConfig>();
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
                throw new ArgumentOutOfRangeException(nameof(type));
        }
    }

    public override bool IsSelectionsLocked()
        => throw new InvalidOperationException("Auto-generator does not support this check method.");

    protected override bool IsWritePropertyLocked(Lyric lyric) =>
        currentAutoGenerateType switch
        {
            AutoGenerateType.DetectReferenceLyric => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.ReferenceLyric), nameof(Lyric.ReferenceLyricConfig)),
            AutoGenerateType.DetectLanguage => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.Language)),
            AutoGenerateType.AutoGenerateRubyTags => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RubyTags)),
            AutoGenerateType.AutoGenerateRomajiTags => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RomajiTags)),
            AutoGenerateType.AutoGenerateTimeTags => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.TimeTags)),
            AutoGenerateType.AutoGenerateTimeTagRomaji => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.TimeTags)),
            AutoGenerateType.AutoGenerateNotes => HitObjectWritableUtils.IsCreateOrRemoveNoteLocked(lyric),
            _ => throw new ArgumentOutOfRangeException(),
        };

    #region Utililty

    [Resolved]
    private KaraokeRulesetEditGeneratorConfigManager? generatorConfigManager { get; set; }

    private LyricPropertyDetector<TProperty, TConfig> getDetector<TProperty, TConfig>()
        where TConfig : GeneratorConfig, new()
    {
        var config = getGeneratorConfig<TConfig>();
        return createInstance<LyricPropertyDetector<TProperty, TConfig>>(config);
    }

    private LyricPropertyDetector<TProperty, TConfig> getDetector<TProperty, TConfig>(IEnumerable<Lyric> lyrics)
        where TConfig : GeneratorConfig, new()
    {
        var config = getGeneratorConfig<TConfig>();
        return createInstance<LyricPropertyDetector<TProperty, TConfig>>(lyrics, config);
    }

    private LyricPropertyGenerator<TProperty, TConfig> getGenerator<TProperty, TConfig>()
        where TConfig : GeneratorConfig, new()
    {
        var config = getGeneratorConfig<TConfig>();
        return createInstance<LyricPropertyGenerator<TProperty, TConfig>>(config);
    }

    private LyricGeneratorSelector<TProperty, TBaseConfig> getSelector<TProperty, TBaseConfig>()
        where TBaseConfig : GeneratorConfig
    {
        return createInstance<LyricGeneratorSelector<TProperty, TBaseConfig>>(generatorConfigManager);
    }

    private static TType createInstance<TType>(params object?[]? args)
    {
        var generatedType = getChildType(typeof(TType));

        var instance = (TType?)Activator.CreateInstance(generatedType, args);
        if (instance == null)
            throw new InvalidOperationException();

        return instance;

        static Type getChildType(Type type)
        {
            // should get the assembly that the has the class GeneratorConfig.
            var assembly = typeof(GeneratorConfig).Assembly;
            return assembly.GetTypes()
                           .Single(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
        }
    }

    private TConfig getGeneratorConfig<TConfig>()
        where TConfig : GeneratorConfig, new()
    {
        if (generatorConfigManager == null)
            throw new InvalidOperationException();

        return generatorConfigManager.Get<TConfig>();
    }

    #endregion
}
