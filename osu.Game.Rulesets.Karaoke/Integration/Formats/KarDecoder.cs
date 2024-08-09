// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Integration.Formats;

public class KarDecoder : IDecoder<Lyric[]>
{
    public Lyric[] Decode(string source)
    {
        var song = new LrcParser.Parser.Kar.KarParser().Decode(source);
        return LrcParserUtils.ConvertToLyrics(song).ToArray();
    }
}
