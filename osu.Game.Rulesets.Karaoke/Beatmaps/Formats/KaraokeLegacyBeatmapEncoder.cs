﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeLegacyBeatmapEncoder
    {
        public string Encode(Beatmap output)
        {
            var lrcEncoder = new LrcEncoder();
            var results = new List<string>
            {
                lrcEncoder.Encode(output),
                string.Join("\n", encodeNote(output)),
                string.Join("\n", encodeStyle(output)),
                string.Join("\n", encodeTranslate(output)),
            };

            return string.Join("\n\n", results.Where(x => !string.IsNullOrEmpty(x)));
        }

        private IEnumerable<string> encodeNote(Beatmap output)
        {
            var notes = output.HitObjects.OfType<Note>().ToList();
            var lyrics = output.HitObjects.OfType<LyricLine>().ToList();
            return notes.GroupBy(x => x.ParentLyric).Select(g =>
            {
                // Get note group
                var noteGroup = g.ToList().GroupBy(n => n.StartIndex);

                // Convert to group format
                var noteGroupStr = string.Join(",", noteGroup.Select(x =>
                {
                    if (x.Count() == 1)
                        return convertNote(x.FirstOrDefault());

                    return "(" + string.Join("|", x.Select(convertNote)) + ")";
                }));

                return $"note{lyrics.IndexOf(g.Key) + 1}={noteGroupStr}";
            }).ToList();

            // Convert single note
            static string convertNote(Note note)
            {
                if (!note.Display)
                    return "-";

                // TODO : Fill if customize ruby and percentage
                return convertTone(note.Tone);

                // Convert tone to string
                static string convertTone(Tone tone) => tone.Scale + (tone.Half ? "#" : "");
            }
        }

        private IEnumerable<string> encodeStyle(Beatmap output)
        {
            var lyricLines = output.HitObjects.OfType<LyricLine>().ToList();

            for (var i = 0; i < lyricLines.Count; i++)
            {
                var lyricLine = lyricLines[i];
                yield return $"@style{i}={lyricLine.LayoutIndex},{lyricLine.FontIndex}";
            }
        }

        private IEnumerable<string> encodeTranslate(Beatmap output)
        {
            if (!output.AnyTranslate())
                yield break;

            foreach (var (languageCode, translates) in output.GetProperty().Translates)
            {
                foreach (var translate in translates)
                {
                    yield return $"@tr[{languageCode}]={translate}";
                }
            }
        }
    }
}
