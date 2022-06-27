// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags;
using osu.Game.Rulesets.Karaoke.Edit.Generator.RubyTags;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricAutoGenerateChangeHandler : HitObjectChangeHandler<Lyric>, ILyricAutoGenerateChangeHandler
    {
        [Resolved]
        private KaraokeRulesetEditGeneratorConfigManager generatorConfigManager { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public bool CanGenerate(LyricAutoGenerateProperty autoGenerateProperty)
        {
            switch (autoGenerateProperty)
            {
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
                => HitObjects.Any(detector.CanDetect);

            bool canGenerate<T>(ILyricPropertyGenerator<T> generator)
                => HitObjects.Any(generator.CanGenerate);
        }

        public IDictionary<Lyric, LocalisableString> GetNotGeneratableLyrics(LyricAutoGenerateProperty autoGenerateProperty)
        {
            switch (autoGenerateProperty)
            {
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
                => HitObjects.Select(x => new KeyValuePair<Lyric, LocalisableString?>(x, detector.GetInvalidMessage(x)))
                             .Where(x => x.Value != null)
                             .ToDictionary(k => k.Key, v => v.Value!.Value);

            IDictionary<Lyric, LocalisableString> getInvalidMessageFromGenerator<T>(ILyricPropertyGenerator<T> generator)
                => HitObjects.Select(x => new KeyValuePair<Lyric, LocalisableString?>(x, generator.GetInvalidMessage(x)))
                             .Where(x => x.Value != null)
                             .ToDictionary(k => k.Key, v => v.Value!.Value);
        }

        public void AutoGenerate(LyricAutoGenerateProperty autoGenerateProperty)
        {
            switch (autoGenerateProperty)
            {
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
                        var rubyTags = rubyGenerator.Generate(lyric);
                        lyric.RubyTags = rubyTags ?? Array.Empty<RubyTag>();
                    });
                    break;

                case LyricAutoGenerateProperty.AutoGenerateRomajiTags:
                    var romajiGenerator = createLyricGenerator<RomajiTag[]>();
                    PerformOnSelection(lyric =>
                    {
                        var romajiTags = romajiGenerator.Generate(lyric);
                        lyric.RomajiTags = romajiTags ?? Array.Empty<RomajiTag>();
                    });
                    break;

                case LyricAutoGenerateProperty.AutoGenerateTimeTags:
                    var timeTagGenerator = createLyricGenerator<TimeTag[]>();
                    PerformOnSelection(lyric =>
                    {
                        var timeTags = timeTagGenerator.Generate(lyric);
                        lyric.TimeTags = timeTags ?? Array.Empty<TimeTag>();
                    });
                    break;

                case LyricAutoGenerateProperty.AutoGenerateNotes:
                    var noteGenerator = createLyricGenerator<Note[]>();
                    PerformOnSelection(lyric =>
                    {
                        // clear exist notes if from those
                        var matchedNotes = beatmap.HitObjects.OfType<Note>().Where(x => x.ParentLyric == lyric).ToArray();
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
                case Type t when t == typeof(CultureInfo):
                    var config = generatorConfigManager.Get<LanguageDetectorConfig>(KaraokeRulesetEditGeneratorSetting.LanguageDetectorConfig);
                    return new LanguageDetector(config) as ILyricPropertyDetector<T>;

                default:
                    throw new NotSupportedException();
            }
        }

        private ILyricPropertyGenerator<T> createLyricGenerator<T>()
        {
            switch (typeof(T))
            {
                case Type t when t == typeof(RubyTag[]):
                    return new RubyTagGeneratorSelector(generatorConfigManager) as ILyricPropertyGenerator<T>;

                case Type t when t == typeof(RomajiTag[]):
                    return new RomajiTagGeneratorSelector(generatorConfigManager) as ILyricPropertyGenerator<T>;

                case Type t when t == typeof(TimeTag[]):
                    return new TimeTagGeneratorSelector(generatorConfigManager) as ILyricPropertyGenerator<T>;

                case Type t when t == typeof(Note[]):
                    var config = generatorConfigManager.Get<NoteGeneratorConfig>(KaraokeRulesetEditGeneratorSetting.NoteGeneratorConfig);
                    return new NoteGenerator(config) as ILyricPropertyGenerator<T>;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
