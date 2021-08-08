// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms
{
    /// <summary>
    /// User select to edit
    /// Preparing to double-click to edit text or delete.
    /// </summary>
    public class EditTextTagCaretPositionAlgorithm : TextTagCaretPositionAlgorithm<EditTextTagCaretPosition>
    {
        public EditTextTagCaretPositionAlgorithm(Lyric[] lyrics)
            : base(lyrics)
        {
        }

        public override EditTextTagCaretPosition MoveUp(EditTextTagCaretPosition currentPosition)
        {
            base.MoveUp(currentPosition);

            var currentTextTag = currentPosition.TextTag;

            // need to check is lyric in time-tag is valid.
            var currentLyric = tagInLyric(currentTextTag);
            if (currentLyric != currentPosition.Lyric)
                throw new ArgumentException(nameof(currentPosition.Lyric));

            // find previous lyric that contains ruby/romaji tags.
            var previousLyric = getPreviousLyricWithTextTag(currentLyric, currentTextTag);
            if (previousLyric == null)
                return null;

            // todo : might check is default struct
            var textTags = getRelatedTypeTextTag(previousLyric, currentPosition);
            var upTextTag = textTags.FirstOrDefault(x => x.StartIndex >= currentTextTag.StartIndex) ?? textTags.LastOrDefault();
            return new EditTextTagCaretPosition(previousLyric, upTextTag);
        }

        public override EditTextTagCaretPosition MoveDown(EditTextTagCaretPosition currentPosition)
        {
            base.MoveDown(currentPosition);

            var currentTextTag = currentPosition.TextTag;

            // need to check is lyric in text-tag is valid.
            var currentLyric = tagInLyric(currentTextTag);
            if (currentLyric != currentPosition.Lyric)
                throw new ArgumentException(nameof(currentPosition.Lyric));

            // find previous lyric that contains ruby/romaji tags.
            var nextLyric = getNextLyricWithTextTag(currentLyric, currentTextTag);
            if (nextLyric == null)
                return null;

            // todo : might check is default struct
            var textTags = getRelatedTypeTextTag(nextLyric, currentPosition);
            var downTextTag = textTags.FirstOrDefault(x => x.StartIndex >= currentTextTag.StartIndex) ?? textTags.LastOrDefault();
            return new EditTextTagCaretPosition(nextLyric, downTextTag);
        }

        public override EditTextTagCaretPosition MoveLeft(EditTextTagCaretPosition currentPosition)
        {
            base.MoveLeft(currentPosition);

            var currentTextTag = currentPosition.TextTag;

            var textTags = Lyrics.SelectMany(x => getRelatedTypeTextTag(x, currentPosition) ?? new ITextTag[] { }).ToArray();
            var previousTextTag = textTags.GetPrevious(currentTextTag);

            var currentLyric = currentPosition.Lyric;
            if (getRelatedTypeTextTag(currentLyric, currentPosition).Contains(previousTextTag))
                return new EditTextTagCaretPosition(currentLyric, previousTextTag);

            var previousLyric = getPreviousLyricWithTextTag(currentLyric, currentTextTag);
            if (previousLyric == null)
                return null;

            return new EditTextTagCaretPosition(previousLyric, previousTextTag);
        }

        public override EditTextTagCaretPosition MoveRight(EditTextTagCaretPosition currentPosition)
        {
            base.MoveRight(currentPosition);

            var currentTextTag = currentPosition.TextTag;

            var textTags = Lyrics.SelectMany(x => getRelatedTypeTextTag(x, currentPosition) ?? new ITextTag[] { }).ToArray();
            var nextTextTag = textTags.GetNext(currentTextTag);

            var currentLyric = currentPosition.Lyric;
            if (getRelatedTypeTextTag(currentLyric, currentPosition).Contains(nextTextTag))
                return new EditTextTagCaretPosition(currentLyric, nextTextTag);

            var nextLyric = getNextLyricWithTextTag(currentLyric, currentTextTag);
            if (nextLyric == null)
                return null;

            return new EditTextTagCaretPosition(nextLyric, nextTextTag);
        }

        public override EditTextTagCaretPosition MoveToFirst()
        {
            // might need to move to first ruby/romaji, but it can un-supported now.
            return null;
        }

        public override EditTextTagCaretPosition MoveToLast()
        {
            // might need to move to first ruby/romaji, but it can un-supported now.
            return null;
        }

        public override EditTextTagCaretPosition MoveToTarget(Lyric lyric)
        {
            // lazy to implement this algorithm because this algorithm haven't being used.
            return null;
        }

        private Lyric tagInLyric(ITextTag textTag)
        {
            return Lyrics.FirstOrDefault(x => getRelatedTypeTextTag(x, textTag)?.Contains(textTag) ?? false);
        }

        private ITextTag[] getRelatedTypeTextTag(Lyric lyric, EditTextTagCaretPosition sample)
            => getRelatedTypeTextTag(lyric, sample.TextTag);

        private ITextTag[] getRelatedTypeTextTag(Lyric lyric, ITextTag sample) =>
            sample switch
            {
                RubyTag _ => lyric.RubyTags?.OfType<ITextTag>().ToArray(),
                RomajiTag _ => lyric.RomajiTags?.OfType<ITextTag>().ToArray(),
                _ => throw new InvalidCastException(nameof(sample))
            };

        private Lyric getPreviousLyricWithTextTag(Lyric current, ITextTag textTag)
            => Lyrics.GetPreviousMatch(current, x => getRelatedTypeTextTag(x, textTag)?.Any() ?? false);

        private Lyric getNextLyricWithTextTag(Lyric current, ITextTag textTag)
            => Lyrics.GetNextMatch(current, x => getRelatedTypeTextTag(x, textTag)?.Any() ?? false);
    }
}
