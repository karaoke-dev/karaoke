// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TextTagsUtils
    {
        public static T[] Sort<T>(IEnumerable<T> textTags, Sorting sorting = Sorting.Asc) where T : ITextTag =>
            sorting switch
            {
                Sorting.Asc => textTags?.OrderBy(x => x.StartIndex).ThenBy(x => x.EndIndex).ToArray(),
                Sorting.Desc => textTags?.OrderByDescending(x => x.EndIndex).ThenByDescending(x => x.StartIndex).ToArray(),
                _ => throw new InvalidEnumArgumentException(nameof(sorting))
            };

        public static T[] FindOutOfRange<T>(IEnumerable<T> textTags, string lyric) where T : ITextTag
        {
            return textTags?.Where(x => TextTagUtils.OutOfRange(x, lyric)).ToArray();
        }

        public static T[] FindOverlapping<T>(T[] textTags, Sorting sorting = Sorting.Asc) where T : ITextTag
        {
            // check is null or empty
            if (textTags == null || textTags.Length == 0)
                return Array.Empty<T>();

            // todo : need to make sure is need to sort in here?
            var sortedTextTags = Sort(textTags, sorting);

            var invalidList = new List<T>();

            // check end is less or equal to start index
            invalidList.AddRange(sortedTextTags.Where(x => x.EndIndex <= x.StartIndex));

            // find other is smaller or bigger
            foreach (var textTag in sortedTextTags)
            {
                if (invalidList.Contains(textTag))
                    continue;

                var checkTags = sortedTextTags.Except(new[] { textTag });

                switch (sorting)
                {
                    case Sorting.Asc:
                        // start index within tne target
                        invalidList.AddRange(checkTags.Where(x => x.StartIndex >= textTag.StartIndex && x.StartIndex < textTag.EndIndex));
                        break;

                    case Sorting.Desc:
                        // end index within tne target
                        invalidList.AddRange(checkTags.Where(x => x.EndIndex > textTag.StartIndex && x.EndIndex <= textTag.EndIndex));
                        break;

                    default:
                        throw new InvalidEnumArgumentException(nameof(sorting));
                }
            }

            return Sort(invalidList.Distinct());
        }

        public static T[] FindEmptyText<T>(IEnumerable<T> textTags) where T : ITextTag
        {
            return textTags?.Where(TextTagUtils.EmptyText).ToArray();
        }

        public static T Combine<T>(T textTagA, T textTagB) where T : ITextTag, new()
        {
            return Combine(new[] { textTagA, textTagB });
        }

        public static T Combine<T>(T[] textTags) where T : ITextTag, new()
        {
            if (textTags == null || !textTags.Any())
                throw new ArgumentNullException(nameof(textTags));

            var sortingValue = Sort(textTags);
            var firstValue = sortingValue.FirstOrDefault();
            var lastValue = sortingValue.LastOrDefault();

            if (firstValue == null)
                throw new NoNullAllowedException(nameof(firstValue));

            if (lastValue == null)
                throw new NoNullAllowedException(nameof(lastValue));

            return new T
            {
                StartIndex = firstValue.StartIndex,
                EndIndex = lastValue.EndIndex,
                Text = string.Join("", sortingValue.Select(x => x.Text))
            };
        }

        public enum Sorting
        {
            /// <summary>
            /// Mark next time tag is error if conflict.
            /// </summary>
            Asc,

            /// <summary>
            /// Mark previous tag is error if conflict.
            /// </summary>
            Desc
        }
    }
}
