// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Integration.Formats;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats;

public class LrcDecoder : Decoder<Beatmap>
{
    public static void Register()
    {
        // Lrc decoder looks like [mm:ss:__]
        AddDecoder<Beatmap>("[", _ => new LrcDecoder());
    }

    protected override void ParseStreamInto(LineBufferedReader stream, Beatmap output)
    {
        string lyricText = stream.ReadToEnd();
        var song = new LrcParser.Parser.Lrc.LrcParser().Decode(lyricText);

        var newLyrics = LrcParserUtils.ConvertToLyrics(song);

        // Clear all hitobjects
        output.HitObjects.Clear();
        output.HitObjects.AddRange(newLyrics);
    }
}
