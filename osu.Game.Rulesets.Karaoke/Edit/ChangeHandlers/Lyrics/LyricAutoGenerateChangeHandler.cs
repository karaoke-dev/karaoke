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
                    return HitObjects.Any(x => languageDetector.CanDetect(x));

                case LyricAutoGenerateProperty.AutoGenerateRubyTags:
                    var rubyGenerator = createLyricGenerator<RubyTag[]>();
                    return HitObjects.Any(x => rubyGenerator.CanGenerate(x));

                case LyricAutoGenerateProperty.AutoGenerateRomajiTags:
                    var romajiGenerator = createLyricGenerator<RomajiTag[]>();
                    return HitObjects.Any(x => romajiGenerator.CanGenerate(x));

                case LyricAutoGenerateProperty.AutoGenerateTimeTags:
                    var timeTagGenerator = createLyricGenerator<TimeTag[]>();
                    return HitObjects.Any(x => timeTagGenerator.CanGenerate(x));

                case LyricAutoGenerateProperty.AutoGenerateNotes:
                    var noteGenerator = createLyricGenerator<Note[]>();
                    return HitObjects.Any(x => noteGenerator.CanGenerate(x));

                default:
                    throw new ArgumentOutOfRangeException(nameof(autoGenerateProperty));
            }
        }

        public IDictionary<Lyric, LocalisableString> GetNotGeneratableLyrics(LyricAutoGenerateProperty autoGenerateProperty)
        {
            switch (autoGenerateProperty)
            {
                case LyricAutoGenerateProperty.DetectLanguage:
                    var languageDetector = createLyricDetector<CultureInfo>();
                    return HitObjects.Where(x => !languageDetector.CanDetect(x))
                                     .ToDictionary(k => k, _ => new LocalisableString("Should have text in lyric."));

                case LyricAutoGenerateProperty.AutoGenerateRubyTags:
                    var rubyGenerator = createLyricGenerator<RubyTag[]>();
                    return HitObjects.Where(x => !rubyGenerator.CanGenerate(x))
                                     .ToDictionary(k => k, _ => new LocalisableString("Before generate ruby-tag, need to assign language first."));

                case LyricAutoGenerateProperty.AutoGenerateRomajiTags:
                    var romajiGenerator = createLyricGenerator<RomajiTag[]>();
                    return HitObjects.Where(x => !romajiGenerator.CanGenerate(x))
                                     .ToDictionary(k => k, _ => new LocalisableString("Before generate romaji-tag, need to assign language first."));

                case LyricAutoGenerateProperty.AutoGenerateTimeTags:
                    var timeTagGenerator = createLyricGenerator<TimeTag[]>();
                    return HitObjects.Where(x => !timeTagGenerator.CanGenerate(x))
                                     .ToDictionary(k => k, _ => new LocalisableString("Before generate time-tag, need to assign language first."));

                case LyricAutoGenerateProperty.AutoGenerateNotes:
                    var noteGenerator = createLyricGenerator<Note[]>();
                    return HitObjects.Where(x => !noteGenerator.CanGenerate(x))
                                     .ToDictionary(k => k, _ => new LocalisableString("Check the time-tag first."));

                default:
                    throw new ArgumentOutOfRangeException(nameof(autoGenerateProperty));
            }
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
