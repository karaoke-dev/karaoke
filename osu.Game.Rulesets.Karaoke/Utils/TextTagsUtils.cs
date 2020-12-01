// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects.Types;
using System.Collections.Generic;
using System.Linq;
using System;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TextTagsUtils
    {
        public static T[] Sort<T>(T[] textTags, Sorting sorting = Sorting.Asc) where T : ITextTag
        {
            switch (sorting)
            {
                case Sorting.Asc:
                    return textTags?.OrderBy(x => x.StartIndex).ThenBy(x => x.EndIndex).ToArray();
                case Sorting.Desc:
                    return textTags?.OrderByDescending(x => x.EndIndex).ThenByDescending(x => x.StartIndex).ToArray();
                default:
                    throw new ArgumentOutOfRangeException(nameof(sorting));
            }
        }

        public static T[] FindInvalid<T>(T[] textTags, string lyric, Sorting sorting = Sorting.Asc) where T : ITextTag
        {
            // check is null or empty
            if (textTags == null || textTags.Length == 0)
                return new T[] { };

            var sortedTextTags = Sort(textTags, sorting);

            var invalidList = new List<T>();

            // check invalid range
            invalidList.AddRange(sortedTextTags.Where(x => x.StartIndex < 0 || x.EndIndex > lyric.Length));

            // check end is less or equal to start index
            invalidList.AddRange(sortedTextTags.Where(x => x.EndIndex <= x.StartIndex));

            // find other is smaller or bigger
            foreach (var textTag in sortedTextTags)
            {
                switch (sorting)
                {
                    case Sorting.Asc:

                        break;

                    case Sorting.Desc:

                        break;
                }
            }

            return Sort(invalidList.Distinct().ToArray());
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
