// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Integration.Formats;

public class LyricTextDecoder : IDecoder<Lyric[]>
{
    public Lyric[] Decode(string source)
    {
        return source.Split('\n').Select((text, index) => new Lyric
        {
            Order = index + 1, // should create default order.
            Text = text,
        }).ToArray();
    }
}
