// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
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

            Debug.Assert(textTagInCaret == null || textTagsInLyric.Contains(textTagInCaret));
        }

        protected sealed override bool PositionMovable(TCaretPosition position)
        {
            return true;
        }

        protected sealed override TCaretPosition? MoveToPreviousLyric(TCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetPrevious(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var textTag = GetTextTagsByLyric(lyric).FirstOrDefault();

            return GenerateCaretPosition(lyric, textTag, CaretGenerateType.Action);
        }

        protected sealed override TCaretPosition? MoveToNextLyric(TCaretPosition currentPosition)
        {
            var lyric = Lyrics.GetNext(currentPosition.Lyric);
            if (lyric == null)
                return null;

            var textTag = GetTextTagsByLyric(lyric).FirstOrDefault();

            return GenerateCaretPosition(lyric, textTag, CaretGenerateType.Action);
        }

        protected sealed override TCaretPosition? MoveToFirstLyric()
        {
            var lyric = Lyrics.FirstOrDefault();
            if (lyric == null)
                return null;

            var textTag = GetTextTagsByLyric(lyric).FirstOrDefault();

            return GenerateCaretPosition(lyric, textTag, CaretGenerateType.Action);
        }

        protected sealed override TCaretPosition? MoveToLastLyric()
        {
            var lyric = Lyrics.LastOrDefault();
            if (lyric == null)
                return null;

            var textTag = GetTextTagsByLyric(lyric).FirstOrDefault();

            return GenerateCaretPosition(lyric, textTag, CaretGenerateType.Action);
        }

        protected sealed override TCaretPosition? MoveToTargetLyric(Lyric lyric)
        {
            var textTag = GetTextTagsByLyric(lyric).FirstOrDefault();

            return MoveToTargetLyric(lyric, textTag);
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

        protected override TCaretPosition? MoveToTargetLyric<TIndex>(Lyric lyric, TIndex? index) where TIndex : default
        {
            if (index is null)
                return GenerateCaretPosition(lyric, default, CaretGenerateType.TargetLyric);

            if (index is not TTextTag textTag)
                throw new InvalidCastException();

            return GenerateCaretPosition(lyric, textTag, CaretGenerateType.TargetLyric);
        }

        protected abstract TTextTag? GetTextTagByCaret(TCaretPosition position);

        protected abstract IList<TTextTag> GetTextTagsByLyric(Lyric lyric);

        protected abstract TCaretPosition GenerateCaretPosition(Lyric lyric, TTextTag? textTag, CaretGenerateType generateType);
    }
}
