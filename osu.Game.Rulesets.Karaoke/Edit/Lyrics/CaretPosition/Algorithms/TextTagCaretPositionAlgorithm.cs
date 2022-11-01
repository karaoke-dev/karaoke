// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    public abstract class TextTagCaretPositionAlgorithm<TCaretPosition, TTextTag> : IndexCaretPositionAlgorithm<TCaretPosition>
        where TCaretPosition : struct, ITextTagCaretPosition
        where TTextTag : ITextTag
    {
        protected TextTagCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        protected sealed override void Validate(TCaretPosition input)
        {
            var textTagInCaret = GetTextTagByCaret(input);
            var textTagsInLyric = GetTextTagsByLyric(input.Lyric);

            Debug.Assert(textTagInCaret != null);
            Debug.Assert(textTagsInLyric.Any());
            Debug.Assert(textTagsInLyric.Contains(textTagInCaret));
        }

        protected sealed override bool PositionMovable(TCaretPosition position)
        {
            return true;
        }

        protected sealed override TCaretPosition? MoveToPreviousLyric(TCaretPosition currentPosition)
        {
            return null;
        }

        protected sealed override TCaretPosition? MoveToNextLyric(TCaretPosition currentPosition)
        {
            return null;
        }

        protected sealed override TCaretPosition? MoveToFirstLyric()
        {
            return null;
        }

        protected sealed override TCaretPosition? MoveToLastLyric()
        {
            return null;
        }

        protected sealed override TCaretPosition? MoveToTargetLyric(Lyric lyric)
        {
            var textTag = GetTextTagsByLyric(lyric).FirstOrDefault();
            if (textTag == null)
                return null;

            return GenerateCaretPosition(lyric, textTag, CaretGenerateType.TargetLyric);
        }

        protected sealed override TCaretPosition? MoveToPreviousIndex(TCaretPosition currentPosition)
        {
            return null;
        }

        protected sealed override TCaretPosition? MoveToNextIndex(TCaretPosition currentPosition)
        {
            return null;
        }

        protected sealed override TCaretPosition? MoveToFirstIndex(Lyric lyric)
        {
            return null;
        }

        protected sealed override TCaretPosition? MoveToLastIndex(Lyric lyric)
        {
            return null;
        }

        protected abstract TTextTag GetTextTagByCaret(TCaretPosition position);

        protected abstract IList<TTextTag> GetTextTagsByLyric(Lyric lyric);

        protected abstract TCaretPosition GenerateCaretPosition(Lyric lyric, TTextTag textTag, CaretGenerateType generateType);
    }
}
