// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Newtonsoft.Json;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.IO.Serialization;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeJsonBeatmapEncoder
    {
        public string Encode(Beatmap output)
        {
            var globalSetting = KaraokeJsonSerializableExtensions.CreateGlobalSettings();

            // should check that every parent in the note contains in the hit objects list.
            var lyrics = output.HitObjects.OfType<Lyric>();
            var notes = output.HitObjects.OfType<Note>();

            // make sure that every note has parent lyric.
            if (notes.Any(note => note.ReferenceLyric == null || !lyrics.Contains(note.ReferenceLyric)))
            {
                throw new InvalidOperationException();
            }

            // create id if object is by reference.
            globalSetting.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            // replace string stream.ReadToEnd().Serialize(output);
            string json = JsonConvert.SerializeObject(output, globalSetting);
            return "// karaoke json file format v1" + '\n' + json;
        }
    }
}
