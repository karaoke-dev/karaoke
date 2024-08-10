// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Integration.Formats;

public class KarEncoder : IEncoder<Beatmap>
{
    public string Encode(Beatmap source)
    {
        var song = LrcParserUtils.ConvertToSong(source);
        return new LrcParser.Parser.Kar.KarParser().Encode(song);
    }
}
