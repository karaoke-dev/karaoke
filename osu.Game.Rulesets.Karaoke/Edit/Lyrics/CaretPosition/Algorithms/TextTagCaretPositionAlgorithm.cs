// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.ComponentModel;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public abstract class TextTagCaretPositionAlgorithm<T> : CaretPositionAlgorithm<T> where T : class, ITextTagCaretPosition
    {
        public EditArea EditArea { get; set; }

        protected TextTagCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override bool PositionMovable(T position)
        {
            if (!IsTextTagTypeValid(position))
                return false;

            // check text tag is in lyric
            return position.TextTag switch
            {
                RubyTag rubyTag => position.Lyric.RubyTags.Contains(rubyTag),
                RomajiTag romajiTag => position.Lyric.RomajiTags.Contains(romajiTag),
                _ => throw new InvalidCastException(nameof(position.TextTag))
            };
        }

        public override T? MoveUp(T currentPosition)
        {
            if (!IsTextTagTypeValid(currentPosition))
                throw new InvalidCastException(nameof(currentPosition.TextTag));

            return null;
        }

        public override T? MoveDown(T currentPosition)
        {
            if (!IsTextTagTypeValid(currentPosition))
                throw new InvalidCastException(nameof(currentPosition.TextTag));

            return null;
        }

        public override T? MoveLeft(T currentPosition)
        {
            if (!IsTextTagTypeValid(currentPosition))
                throw new InvalidCastException(nameof(currentPosition.TextTag));

            return null;
        }

        public override T? MoveRight(T currentPosition)
        {
            if (!IsTextTagTypeValid(currentPosition))
                throw new InvalidCastException(nameof(currentPosition.TextTag));

            return null;
        }

        protected bool IsTextTagTypeValid(T position) =>
            EditArea switch
            {
                EditArea.Ruby => position.TextTag is RubyTag,
                EditArea.Romaji => position.TextTag is RomajiTag,
                EditArea.Both => true,
                _ => throw new InvalidEnumArgumentException(nameof(EditArea))
            };
    }

    public enum EditArea
    {
        Ruby,

        Romaji,

        Both,
    }
}
