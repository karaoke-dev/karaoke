// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public abstract class TextTagCaretPositionAlgorithm<T> : CaretPositionAlgorithm<T> where T : ICaretPosition
    {
        protected readonly EditArea EditArea;
        public TextTagCaretPositionAlgorithm(Lyric[] lyrics, EditArea editArea)
            : base(lyrics)
        {
            EditArea = editArea;
        }
    }

    public enum EditArea
    {
        Ruby,

        Romaji,

        Both,
    }
}
