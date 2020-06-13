﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeLegacyBeatmapDecoder : LegacyBeatmapDecoder
    {
        public new const int LATEST_VERSION = 1;

        public new static void Register()
        {
            AddDecoder<Beatmap>(@"karaoke file format v", m => new KaraokeLegacyBeatmapDecoder(Parsing.ParseInt(m.Split('v').Last())));

            // use this weird way to let all the fall-back beatmap(include karaoke beatmap) become karaoke beatmap.
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

        private void processNotes(Beatmap beatmap, IList<string> noteLines)
        {
            // Remove all karaoke note
            beatmap.HitObjects.RemoveAll(x => x is Note);

            var lyricLines = beatmap.HitObjects.OfType<LyricLine>().ToList();

            for (int l = 0; l < lyricLines.Count; l++)
            {
                var lyricLine = lyricLines[l];
                var line = noteLines.ElementAtOrDefault(l)?.Split('=').Last();

                // Create default note if not exist
                if (string.IsNullOrEmpty(line))
                {
                    beatmap.HitObjects.AddRange(lyricLine.CreateDefaultNotes());
                    continue;
                }

                var notes = line.Split(',');
                var defaultNotes = lyricLine.CreateDefaultNotes().ToList();
                var minNoteNumber = Math.Min(notes.Length, defaultNotes.Count);

                // Process each note
                for (int i = 0; i < minNoteNumber; i++)
                {
                    var note = notes[i];
                    var defaultNote = defaultNotes[i];

                    // Support multi note in one time tag, format like ([1;0.5;か]|1#|...)
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

                            // Split note and apply them
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
                    note.EndTime = note.StartTime + duration.Value;

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
            var lyricLines = beatmap.HitObjects.OfType<LyricLine>().ToList();

            for (int l = 0; l < lyricLines.Count; l++)
            {
                var lyricLine = lyricLines[l];
                var line = styleLines.ElementAtOrDefault(l)?.Split('=').Last();

                // TODO : maybe create default layer and style index here?
                if (string.IsNullOrEmpty(line))
                    return;

                var layoutIndexStr = line.Split(',').FirstOrDefault();
                var fontIndexStr = line.Split(',').ElementAtOrDefault(1);

                if (int.TryParse(layoutIndexStr, out int layoutIndex))
                    lyricLine.LayoutIndex = layoutIndex;

                if (int.TryParse(fontIndexStr, out int fontIndex))
                    lyricLine.FontIndex = fontIndex;
            }
        }

        private void processTranslate(Beatmap beatmap, IEnumerable<string> translateLines)
        {
            var dictionary = new PropertyDictionary();

            foreach (var translateLine in translateLines)
            {
                // format should like @tr[en-US]=First translate
                var translateLanguage = translateLine.Split('=').FirstOrDefault()?.Split('[').LastOrDefault()?.Split(']').FirstOrDefault();
                var translateStr = translateLine.Split('=').LastOrDefault();

                // Add into dictionary
                if (dictionary.Translates.TryGetValue(translateLanguage, out var list))
                {
                    list.Add(translateStr);
                }
                else
                {
                    dictionary.Translates.Add(translateLanguage, new List<string>
                    {
                        translateStr
                    });
                }
            }

            beatmap.HitObjects.Add(dictionary);
        }
    }
}
