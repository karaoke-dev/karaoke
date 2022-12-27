// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
                string.Join("\n", encodeTranslate(output)),
            };

            return string.Join("\n\n", results.Where(x => !string.IsNullOrEmpty(x)));
        }

        private IEnumerable<string> encodeNote(Beatmap output)
        {
            var notes = output.HitObjects.OfType<Note>().ToList();
            var lyrics = output.HitObjects.OfType<Lyric>().ToList();
            return notes.GroupBy(x => x.ReferenceLyric).Select(g =>
            {
                var lyric = g.Key;
                if (lyric == null)
                    throw new ArgumentNullException();

                // Get note group
                var noteGroup = g.ToList().GroupBy(n => n.ReferenceTimeTagIndex);

                // Convert to group format
                string noteGroupStr = string.Join(",", noteGroup.Select(x =>
                {
                    if (x.Count() == 1)
                        return convertNote(x.First());

                    return "(" + string.Join("|", x.Select(convertNote)) + ")";
                }));

                return $"note{lyrics.IndexOf(lyric) + 1}={noteGroupStr}";
            }).ToList();

            // Convert single note
            static string convertNote(Note note)
            {
                if (!note.Display)
                    return "-";

                // TODO : Fill if customize ruby and percentage
                return convertTone(note.Tone);

                // Convert tone to string
                static string convertTone(Tone tone) => tone.Scale + (tone.Half ? "#" : string.Empty);
            }
        }

        private IEnumerable<string> encodeTranslate(Beatmap output)
        {
            if (!output.AnyTranslate())
                yield break;

            var lyrics = output.HitObjects.OfType<Lyric>().ToList();
            var availableTranslates = output.AvailableTranslates();

            foreach (var translate in availableTranslates)
            {
                foreach (var lyric in lyrics)
                {
                    string translateString = lyric.Translates.TryGetValue(translate, out string? value) ? value : string.Empty;
                    yield return $"@tr[{translate.Name}]={translateString}";
                }
            }
        }
    }
}
