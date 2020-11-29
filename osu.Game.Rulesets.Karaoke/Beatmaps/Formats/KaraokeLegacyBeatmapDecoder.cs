// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeLegacyBeatmapDecoder : LegacyBeatmapDecoder
    {
        public new const int LATEST_VERSION = 1;

        public new static void Register()
        {
            AddDecoder<Beatmap>(@"karaoke file format v", m => new KaraokeLegacyBeatmapDecoder(Parsing.ParseInt(m.Split('v').Last())));

            // a weird way to let all the fall-back beatmaps (including karaoke beatmaps) become karaoke beatmaps.
            SetFallbackDecoder<Beatmap>(() => new KaraokeLegacyBeatmapDecoder());
        }

        public KaraokeLegacyBeatmapDecoder(int version = LATEST_VERSION)
            : base(version)
        {
        }

        private readonly IList<string> lrcLines = new List<string>();
        private readonly IList<string> noteLines = new List<string>();
        private readonly IList<string> lyricStyles = new List<string>();
        private readonly IList<string> translates = new List<string>();

        protected override void ParseLine(Beatmap beatmap, Section section, string line)
        {
            if (section != Section.HitObjects)
            {
                base.ParseLine(beatmap, section, line);
                return;
            }

            if (line.ToLower().StartsWith("@ruby") || line.ToLower().StartsWith("@romaji"))
            {
                // lrc queue
                lrcLines.Add(line);
            }
            else if (line.ToLower().StartsWith("@note"))
            {
                // add tone line queue
                noteLines.Add(line);
            }
            else if (line.ToLower().StartsWith("@style"))
            {
                // add style queue
                lyricStyles.Add(line);
            }
            else if (line.ToLower().StartsWith("@tr"))
            {
                // add translate queue
                translates.Add(line);
            }
            else if (line.StartsWith("@"))
            {
                // Remove @ in time tag and add into lrc queue
                lrcLines.Add(line.Substring(1));
            }
            else if (line.ToLower() == "end")
            {
                // Start processing file
                using (var stream = new MemoryStream())
                using (var writer = new StreamWriter(stream))
                using (var reader = new LineBufferedReader(stream))
                {
                    // Create stream
                    writer.Write(string.Join("\n", lrcLines));
                    writer.Flush();
                    stream.Position = 0;

                    // Create lec decoder
                    var decoder = new LrcDecoder();
                    var lrcBeatmap = decoder.Decode(reader);

                    // Apply hitobjects
                    beatmap.HitObjects = lrcBeatmap.HitObjects;
                }

                processNotes(beatmap, noteLines);
                processTranslate(beatmap, translates);

                if (lyricStyles.Any())
                    processStyle(beatmap, lyricStyles);
            }
        }

        private void processNotes(Beatmap beatmap, IList<string> lines)
        {
            // Remove all karaoke note
            beatmap.HitObjects.RemoveAll(x => x is Note);

            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();

            for (int l = 0; l < lyrics.Count; l++)
            {
                var lyric = lyrics[l];
                var line = lines.ElementAtOrDefault(l)?.Split('=').Last();

                // Create default note if not exist
                if (string.IsNullOrEmpty(line))
                {
                    beatmap.HitObjects.AddRange(lyric.CreateDefaultNotes());
                    continue;
                }

                var notes = line.Split(',');
                var defaultNotes = lyric.CreateDefaultNotes().ToList();
                var minNoteNumber = Math.Min(notes.Length, defaultNotes.Count);

                // Process each note
                for (int i = 0; i < minNoteNumber; i++)
                {
                    var note = notes[i];
                    var defaultNote = defaultNotes[i];

                    // Support multiple notes in one time tag, format like ([1;0.5;か]|1#|...)
                    if (!note.StartsWith("(") || !note.EndsWith(")"))
                    {
                        // Process and add note
                        applyNote(defaultNote, note);
                        beatmap.HitObjects.Add(defaultNote);
                    }
                    else
                    {
                        float startPercentage = 0;
                        var rubyNotes = note.Replace("(", "").Replace(")", "").Split('|');

                        for (int j = 0; j < rubyNotes.Length; j++)
                        {
                            string rubyNote = rubyNotes[j];

                            string tone;
                            float percentage = (float)Math.Round((float)1 / rubyNotes.Length, 2, MidpointRounding.AwayFromZero);
                            string ruby = defaultNote.AlternativeText?.ElementAtOrDefault(j).ToString();

                            // Format like [1;0.5;か]
                            if (note.StartsWith("[") && note.EndsWith("]"))
                            {
                                var rubyNoteProperty = note.Replace("[", "").Replace("]", "").Split(';');

                                // Copy tome property
                                tone = rubyNoteProperty[0];

                                // Copy percentage property
                                if (rubyNoteProperty.Length >= 2)
                                    float.TryParse(rubyNoteProperty[1], out percentage);

                                // Copy text property
                                if (rubyNoteProperty.Length >= 3)
                                    ruby = rubyNoteProperty[2];
                            }
                            else
                            {
                                tone = rubyNote;
                            }

                            // Split note and apply it
                            var splitDefaultNote = defaultNote.CopyByPercentage(startPercentage, percentage);
                            startPercentage += percentage;
                            if (!string.IsNullOrEmpty(ruby))
                                splitDefaultNote.Text = ruby;

                            // Process and add note
                            applyNote(splitDefaultNote, tone);
                            beatmap.HitObjects.Add(splitDefaultNote);
                        }
                    }
                }
            }

            void applyNote(Note note, string noteStr, string ruby = null, double? duration = null)
            {
                if (noteStr == "-")
                    note.Display = false;
                else
                {
                    note.Display = true;
                    note.Tone = convertTone(noteStr);
                }

                if (!string.IsNullOrEmpty(ruby))
                    note.Text = ruby;

                if (duration != null)
                    note.Duration = duration.Value;

                //Support format : 1  1.  1.5  1+  1#
                Tone convertTone(string tone)
                {
                    var half = false;

                    if (tone.Contains(".") || tone.Contains("#"))
                    {
                        half = true;

                        // only get digit part
                        tone = tone.Split('.').FirstOrDefault()?.Split('#').FirstOrDefault();
                    }

                    if (!int.TryParse(tone, out int scale))
                        throw new ArgumentOutOfRangeException($"{tone} does not support in {nameof(KaraokeLegacyBeatmapDecoder)}");

                    return new Tone
                    {
                        Scale = scale,
                        Half = half
                    };
                }
            }
        }

        private void processStyle(Beatmap beatmap, IList<string> styleLines)
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();

            for (int l = 0; l < lyrics.Count; l++)
            {
                var lyric = lyrics[l];
                var line = styleLines.ElementAtOrDefault(l)?.Split('=').Last();

                // TODO : maybe create default layer and style index here?
                if (string.IsNullOrEmpty(line))
                    return;

                var layoutIndexStr = line.Split(',').FirstOrDefault();
                var styleIndexStr = line.Split(',').ElementAtOrDefault(1);

                if (int.TryParse(layoutIndexStr, out int layoutIndex))
                    lyric.LayoutIndex = layoutIndex;

                if (int.TryParse(styleIndexStr, out int styleIndex))
                    lyric.Singers = SingerUtils.GetSingersIndex(styleIndex);
            }
        }

        private void processTranslate(Beatmap beatmap, IEnumerable<string> translateLines)
        {
            var availableTranslates = new List<BeatmapSetOnlineLanguage>();

            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            var translates = translateLines.Select(translate => new
            {
                key = translate.Split('=').FirstOrDefault()?.Split('[').LastOrDefault()?.Split(']').FirstOrDefault(),
                value = translate.Split('=').LastOrDefault()
            }).GroupBy(x => x.key, y => y.value).ToList();

            for (int i = 0; i < translates.Count; i++)
            {
                var id = i + 1;
                var singleLanguage = translates[i];

                var key = singleLanguage.Key;
                var values = singleLanguage.ToList();

                var size = Math.Min(lyrics.Count, singleLanguage.Count());

                for (int j = 0; j < size; j++)
                {
                    lyrics[j].Translates.Add(id, values[j]);
                }

                availableTranslates.Add(new BeatmapSetOnlineLanguage
                {
                    Id = id,
                    Name = key
                });
            }

            var dictionary = new LegacyPropertyDictionary
            {
                AvailableTranslates = availableTranslates.ToArray()
            };

            beatmap.HitObjects.Add(dictionary);
        }
    }
}
