// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public abstract class LyricTextTagsChangeHandler<TTextTag> : HitObjectChangeHandler<Lyric>, ILyricTextTagsChangeHandler<TTextTag> where TTextTag : class, ITextTag, new()
    {
        public void Add(TTextTag textTag)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                bool containsInLyric = ContainsInLyric(lyric, textTag);
                if (containsInLyric)
                    throw new InvalidOperationException($"{nameof(textTag)} already in the lyric");

                AddToLyric(lyric, textTag);
            });
        }

        public void Remove(TTextTag textTag)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                bool containsInLyric = ContainsInLyric(lyric, textTag);
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(textTag)} is not in the lyric");

                RemoveFromLyric(lyric, textTag);
            });
        }

        public void RemoveAll(IEnumerable<TTextTag> textTags)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                // should convert to array because enumerable might change while deleting.
                foreach (var textTag in textTags.ToArray())
                {
                    bool containsInLyric = ContainsInLyric(lyric, textTag);
                    if (!containsInLyric)
                        throw new InvalidOperationException($"{nameof(textTag)} is not in the lyric");

                    RemoveFromLyric(lyric, textTag);
                }
            });
        }

        public void SetIndex(TTextTag textTag, int? startIndex, int? endIndex)
        {
            CheckExactlySelectedOneHitObject();

            // note: it's ok not sort the text tag by index.
            PerformOnSelection(lyric =>
            {
                bool containsInLyric = ContainsInLyric(lyric, textTag);
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(textTag)} is not in the lyric");

                if (startIndex != null)
                    textTag.StartIndex = startIndex.Value;

                if (endIndex != null)
                    textTag.EndIndex = endIndex.Value;

                // after change the index, should check if index is valid.
                if (TextTagUtils.OutOfRange(textTag, lyric.Text))
                    throw new InvalidOperationException($"{nameof(startIndex)} or {nameof(endIndex)} is not valid");
            });
        }

        public void ShiftingIndex(IEnumerable<TTextTag> textTags, int offset)
        {
            CheckExactlySelectedOneHitObject();

            // note: it's ok not sort the text tag by index.
            PerformOnSelection(lyric =>
            {
                foreach (var textTag in textTags)
                {
                    bool containsInLyric = ContainsInLyric(lyric, textTag);
                    if (!containsInLyric)
                        throw new InvalidOperationException($"{nameof(textTag)} is not in the lyric");

                    (int startIndex, int endIndex) = TextTagUtils.GetShiftingIndex(textTag, lyric.Text, offset);
                    textTag.StartIndex = startIndex;
                    textTag.EndIndex = endIndex;
                }
            });
        }

        public void SetText(TTextTag textTag, string text)
        {
            CheckExactlySelectedOneHitObject();

            PerformOnSelection(lyric =>
            {
                bool containsInLyric = ContainsInLyric(lyric, textTag);
                if (!containsInLyric)
                    throw new InvalidOperationException($"{nameof(textTag)} is not in the lyric");

                textTag.Text = text;
            });
        }

        protected abstract bool ContainsInLyric(Lyric lyric, TTextTag textTag);

        protected abstract void AddToLyric(Lyric lyric, TTextTag textTag);

        protected abstract void RemoveFromLyric(Lyric lyric, TTextTag textTag);
    }
}
