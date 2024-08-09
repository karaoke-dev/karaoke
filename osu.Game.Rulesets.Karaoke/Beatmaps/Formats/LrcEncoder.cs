// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Integration.Formats;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats;

public class LrcEncoder
{
    public string Encode(Beatmap output)
    {
        var song = LrcParserUtils.ConvertToSong(output);
        return new LrcParser.Parser.Lrc.LrcParser().Encode(song);
    }
}
