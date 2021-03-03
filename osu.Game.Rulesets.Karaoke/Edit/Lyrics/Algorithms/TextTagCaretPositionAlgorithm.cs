// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms
{
    public abstract class TextTagCaretPositionAlgorithm<T> : CaretPositionAlgorithm<T> where T : ITextTagCaretPosition
    {
        protected readonly EditArea EditArea;
        public TextTagCaretPositionAlgorithm(Lyric[] lyrics, EditArea editArea)
            : base(lyrics)
        {
            EditArea = editArea;
        }

        public override bool PositionMovable(T position)
        {
            if (!IsTextTagTypeValid(position))
                return false;

            // check text tag is in lyric
            switch (position.TextTag)
            {
                case RubyTag rubyTag:
                    return position.Lyric.RubyTags.Contains(rubyTag);
                case RomajiTag romajiTag:
                    return position.Lyric.RomajiTags.Contains(romajiTag);
                default:
                    throw new InvalidCastException(nameof(position.TextTag));
            }
        }

        protected bool IsTextTagTypeValid(T position)
        {
            switch (EditArea)
            {
                case EditArea.Ruby:
                    return position.TextTag is RubyTag;
                case EditArea.Romaji:
                    return position.TextTag is RomajiTag;
                case EditArea.Both:
                    return true;
                default:
                    throw new IndexOutOfRangeException(nameof(position.TextTag));
            }
        }
    }

    public enum EditArea
    {
        Ruby,

        Romaji,

        Both,
    }
}
