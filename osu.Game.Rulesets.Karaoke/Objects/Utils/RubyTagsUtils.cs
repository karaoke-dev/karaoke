// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Objects.Utils;

public static class RubyTagsUtils
{
    public static RubyTag[] Sort(IEnumerable<RubyTag> rubyTags, Sorting sorting = Sorting.Asc) =>
        sorting switch
        {
            Sorting.Asc => rubyTags.OrderBy(x => x.StartIndex).ThenBy(x => x.EndIndex).ToArray(),
            Sorting.Desc => rubyTags.OrderByDescending(x => x.EndIndex).ThenByDescending(x => x.StartIndex).ToArray(),
            _ => throw new InvalidEnumArgumentException(nameof(sorting)),
        };

    public static RubyTag[] FindOutOfRange(IEnumerable<RubyTag> rubyTags, string lyric)
    {
        return rubyTags.Where(x => RubyTagUtils.OutOfRange(x, lyric)).ToArray();
    }

    public static RubyTag[] FindOverlapping(IList<RubyTag> rubyTags, Sorting sorting = Sorting.Asc)
    {
        // check is null or empty
        if (!rubyTags.Any())
            return Array.Empty<RubyTag>();

        // todo : need to make sure is need to sort in here?
        var sortedRubyTags = Sort(rubyTags, sorting);

        var invalidList = new List<RubyTag>();

        // check end is less or equal to start index
        invalidList.AddRange(sortedRubyTags.Where(x => x.EndIndex < x.StartIndex));

        // find other is smaller or bigger
        foreach (var rubyTag in sortedRubyTags)
        {
            if (invalidList.Contains(rubyTag))
                continue;

            var checkTags = sortedRubyTags.Except(new[] { rubyTag });

            switch (sorting)
            {
                case Sorting.Asc:
                    // start index within tne target
                    invalidList.AddRange(checkTags.Where(x => x.StartIndex >= rubyTag.StartIndex && x.StartIndex <= rubyTag.EndIndex));
                    break;

                case Sorting.Desc:
                    // end index within tne target
                    invalidList.AddRange(checkTags.Where(x => x.EndIndex >= rubyTag.StartIndex && x.EndIndex <= rubyTag.EndIndex));
                    break;

                default:
                    throw new InvalidEnumArgumentException(nameof(sorting));
            }
        }

        return Sort(invalidList.Distinct());
    }

    public static RubyTag[] FindEmptyText(IEnumerable<RubyTag> rubyTags)
    {
        return rubyTags.Where(RubyTagUtils.EmptyText).ToArray();
    }

    public static RubyTag Combine(RubyTag rubyTagA, RubyTag rubyTagB)
    {
        return Combine(new[] { rubyTagA, rubyTagB });
    }

    public static RubyTag Combine(RubyTag[] rubyTags)
    {
        if (rubyTags == null || !rubyTags.Any())
            throw new ArgumentNullException(nameof(rubyTags));

        var sortingValue = Sort(rubyTags);
        var firstValue = sortingValue.FirstOrDefault();
        var lastValue = sortingValue.LastOrDefault();

        if (firstValue == null)
            throw new NoNullAllowedException(nameof(firstValue));

        if (lastValue == null)
            throw new NoNullAllowedException(nameof(lastValue));

        return new RubyTag
        {
            StartIndex = firstValue.StartIndex,
            EndIndex = lastValue.EndIndex,
            Text = string.Join(string.Empty, sortingValue.Select(x => x.Text)),
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
        Desc,
    }
}
