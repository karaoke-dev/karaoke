// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Objects;
using System.IO;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class KaroakeLegacyBeatmapDecoder : LegacyBeatmapDecoder
    {
        public new const int LATEST_VERSION = 1;

        public new static void Register()
        {
            AddDecoder<Beatmap>(@"karaoke file format v", m => new KaroakeLegacyBeatmapDecoder(Parsing.ParseInt(m.Split('v').Last())));

            // use this weird way to let all the fall-back beatmap(include karaoke beatmap) become karaoke beatmap.
            SetFallbackDecoder<Beatmap>(() => new KaroakeLegacyBeatmapDecoder());
        }

        public KaroakeLegacyBeatmapDecoder(int version = LATEST_VERSION)
            : base(version)
        {
        }

        private string lines = "";
        private string toneLines = "";

        protected override void ParseLine(Beatmap beatmap, Section section, string line)
        {
            if (section == Section.HitObjects)
            {
                // End of the .lrc file
                if (line.ToLower() == "end")
                {
                    using (var stream = new MemoryStream())
                    using (var writer = new StreamWriter(stream))
                    using (var reader = new LineBufferedReader(stream))
                    {
                        // Create stream
                        writer.Write(lines);
                        writer.Flush();
                        stream.Position = 0;

                        // Create lec decoder
                        var decoder = new LrcDecoder();
                        var lrcBeatmap = decoder.Decode(reader);

                        // Apply hitobjects
                        beatmap.HitObjects = lrcBeatmap.HitObjects.OfType<HitObject>().ToList();

                        // TODO : how to get ruby
                    }

                    if (!string.IsNullOrEmpty(toneLines))
                    {
                        using (var stream = new MemoryStream())
                        using (var writer = new StreamWriter(stream))
                        using (var reader = new LineBufferedReader(stream))
                        {
                            // Create stream
                            writer.Write(toneLines);
                            writer.Flush();
                            stream.Position = 0;

                            // Create karaoke note decoder
                            var ToneDecoder = new KaraokeNoteDecoder(beatmap);
                            var lrcBeatmap = ToneDecoder.Decode(reader);
                        }
                    }
                }
                else if (line.StartsWith("@["))
                {
                    // Remove @ in timetag
                    lines += line.Substring(1) + '\n';
                }
                else if (line.StartsWith("@note"))
                {
                    // Process tone line
                    toneLines += line + '\n';
                }
                else
                {
                    // Add to line
                    lines += line + '\n';
                }
            }
            else
            {
                base.ParseLine(beatmap, section, line);
            }
        }
    }
}
