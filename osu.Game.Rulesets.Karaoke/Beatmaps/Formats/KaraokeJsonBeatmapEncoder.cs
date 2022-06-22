// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Linq;
using Newtonsoft.Json;
using osu.Game.Beatmaps;
using osu.Game.IO.Serialization;
using osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaraokeJsonBeatmapEncoder
    {
        public string Encode(Beatmap output)
        {
            var globalSetting = JsonSerializableExtensions.CreateGlobalSettings();
            globalSetting.Converters.Add(new CultureInfoConverter());
            globalSetting.Converters.Add(new RomajiTagConverter());
            globalSetting.Converters.Add(new RomajiTagsConverter());
            globalSetting.Converters.Add(new RubyTagConverter());
            globalSetting.Converters.Add(new RubyTagsConverter());
            globalSetting.Converters.Add(new TimeTagConverter());
            globalSetting.Converters.Add(new TimeTagsConverter());
            globalSetting.Converters.Add(new ToneConverter());

            // should check that every parent in the note contains in the hit objects list.
            var lyrics = output.HitObjects.OfType<Lyric>();
            var notes = output.HitObjects.OfType<Note>();

            // make sure that every note has parent lyric.
            if (notes.Any(note => note.ParentLyric == null || !lyrics.Contains(note.ParentLyric)))
            {
                throw new InvalidOperationException();
            }

            // replace string stream.ReadToEnd().Serialize(output);
            string json = JsonConvert.SerializeObject(output, globalSetting);
            return "// karaoke json file format v1" + '\n' + json;
        }
    }
}
