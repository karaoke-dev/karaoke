// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Objects;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeNoteDecoder : Decoder<Beatmap>
    {
        public static void Register()
        {
            // Karaoke note decoder looks like @tone1=...
            AddDecoder<Beatmap>("@note", m => new KaraokeNoteDecoder(null));
        }

        private readonly Beatmap beatmap;

        protected override Beatmap CreateTemplateObject() => beatmap;

        public KaraokeNoteDecoder(Beatmap beatmap)
        {
            this.beatmap = beatmap;
        }

        protected override void ParseStreamInto(LineBufferedReader stream, Beatmap output)
        {
            // Remove all karaoke note
            output.HitObjects.RemoveAll(x => x is KaraokeNote);

            var lyricLines = output.HitObjects.OfType<LyricLine>().ToList();
            foreach (var lyricLine in lyricLines)
            {
                var line = stream.ReadLine()?.Split('=').Last();
                if (string.IsNullOrEmpty(line))
                    return;

                var notes = line.Split(',');
                var defaultNotes = lyricLine.CreateDefaultNotes().ToList();
                var minNoteNumber = Math.Min(notes.Length, defaultNotes.Count());

                // Process each note
                for (int i = 0; i < minNoteNumber; i++)
                {
                    var note = notes[i];
                    var defaultNote = defaultNotes[i];

                    // Support multi note in one timetag, format like ([1;0.5;か]|1#|...)
                    if (note.StartsWith("(") && note.EndsWith(")"))
                    {
                        float startPercentage = 0;
                        var rubyNotes = note.Replace("(", "").Replace(")", "").Split('|');

                        foreach (var rubyNote in rubyNotes)
                        {
                            string tone = "0";
                            float percentage = (float)Math.Round((float)1 / rubyNotes.Length, 2, MidpointRounding.AwayFromZero);
                            string ruby = null;

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
                            output.HitObjects.Add(splitDefaultNote);
                        }
                    }
                    else
                    {
                        // Process and add note
                        applyNote(defaultNote, note);
                        output.HitObjects.Add(defaultNote);
                    }
                }
            }
        }

        private void applyNote(KaraokeNote karaokeNote, string note, string ruby = null, double? duration = null)
        {
            var convertedTone = convertTone(note);
            karaokeNote.Display = true;
            karaokeNote.Tone = convertedTone.Item1;
            karaokeNote.Half = convertedTone.Item2;

            if (!string.IsNullOrEmpty(ruby))
                karaokeNote.Text = ruby;

            if (duration != null)
                karaokeNote.EndTime = karaokeNote.StartTime + duration.Value;
        }

        /// <summary>
        /// Support format :
        /// 1  1.  1.5  1+  1#
        /// Should all be return <1,true>
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        private Tuple<int, bool> convertTone(string note)
        {
            var half = false;
            if (note.Contains(".") || note.Contains("#"))
            {
                half = true;

                // only get digit part
                note = note.Split('.').FirstOrDefault().Split('#').FirstOrDefault();
            }

            if (int.TryParse(note, out int tone))
            {
                return new Tuple<int, bool>(tone, half);
            }

            throw new ArgumentOutOfRangeException($"{note} does not support in {nameof(KaraokeNoteDecoder)}");
        }
    }
}
