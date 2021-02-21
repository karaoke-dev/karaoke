// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public abstract class CaretPositionAlgorithm
    {
        // Lyrics is not lock and can be accessible.
        protected readonly Lyric[] Lyrics;

        protected CaretPositionAlgorithm(Lyric[] lyrics)
        {
            Lyrics = LyricsUtils.FindUnlockLyrics(OrderUtils.Sorted(lyrics));
        }
    }
}
