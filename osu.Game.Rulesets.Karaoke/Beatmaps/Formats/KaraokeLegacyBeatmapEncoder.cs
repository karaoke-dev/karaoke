// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Integration.Formats;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats;

public class KaraokeLegacyBeatmapEncoder
{
    public string Encode(Beatmap output)
    {
        var encoder = new KarEncoder();
        var results = new List<string>
        {
            encoder.Encode(output),
            string.Join("\n", encodeNotes(output)),
            string.Join("\n", encodeTranslations(output)),
        };

        return string.Join("\n\n", results.Where(x => !string.IsNullOrEmpty(x)));
    }

    private IEnumerable<string> encodeNotes(Beatmap output)
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
            return !note.Display
                ? "-"
                : convertTone(note.Tone);

            // Convert tone to string
            static string convertTone(Tone tone) => tone.Scale + (tone.Half ? "#" : string.Empty);
        }
    }

    private IEnumerable<string> encodeTranslations(Beatmap output)
    {
        if (!output.AnyTranslation())
            yield break;

        var lyrics = output.HitObjects.OfType<Lyric>().ToList();
        var availableTranslations = output.AvailableTranslationLanguages();

        foreach (var translation in availableTranslations)
        {
            foreach (var lyric in lyrics)
            {
                string translationString = lyric.Translations.TryGetValue(translation, out string? value) ? value : string.Empty;
                yield return $"@tr[{translation.Name}]={translationString}";
            }
        }
    }
}
