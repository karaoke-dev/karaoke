// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public class TypingCursorPositionAlgorithm : GenericCursorPositionAlgorithm
    {
        public TypingCursorPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected override TextIndex GetPreviousIndex(TextIndex currentIndex)
        {
            return new TextIndex(currentIndex.Index - 1, currentIndex.State);
        }

        protected override TextIndex GetNextIndex(TextIndex currentIndex)
        {
            return new TextIndex(currentIndex.Index + 1, currentIndex.State);
        }
    }
}
