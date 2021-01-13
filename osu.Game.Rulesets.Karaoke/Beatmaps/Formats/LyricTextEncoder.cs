// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats
{
    public class LyricTextEncoder
    {
        public string Encode(IBeatmap output)
        {
            var lyrics = output.HitObjects.OfType<Lyric>();
            var lyricTexts = lyrics.Select(x => x.Text).Where(x => !string.IsNullOrWhiteSpace(x));
            return string.Join('\n', lyricTexts);
        }
    }
}
