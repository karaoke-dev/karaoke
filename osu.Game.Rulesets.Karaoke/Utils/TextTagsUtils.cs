// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects.Types;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TextTagsUtils
    {
        public static T[] Sort<T>(T[] textTags) where T : ITextTag
        {
            return textTags?.OrderBy(x => x.StartIndex).ThenBy(x => x.EndIndex).ToArray();
        }

        public static T[] FindInvalid<T>(T[] textTags, GroupCheck other = GroupCheck.Asc, SelfCheck self = SelfCheck.BasedOnStart) where T : ITextTag
        {
            var sortedTextTags = Sort(textTags);
            var groupedTextTags = sortedTextTags.GroupBy(x => x.StartIndex);

            var invalidList = new List<T>();

            foreach (var groupedTimeTag in groupedTextTags)
            {
                // add invalid group into list.
                var groupInvalid = findGroupInvalid();
                if (groupInvalid != null)
                    invalidList.AddRange(groupInvalid);

                // add invalid self into list.
                var selfInvalid = findSelfInvalid();
                if (selfInvalid != null)
                    invalidList.AddRange(selfInvalid);

                List<T> findGroupInvalid()
                {
                    switch (other)
                    {
                        case GroupCheck.Asc:
                            // mark next is invalid if smaller then self
                            var groupMaxTime = groupedTimeTag.Max(x => x.Item2);
                            if (groupMaxTime == null)
                                return null;

                            return groupedTimeTag.Where(x => x.Index > groupedTimeTag.Key && x.Item2 < groupMaxTime).ToList();

                        case GroupCheck.Desc:
                            // mark previous is invalid if larger then self
                            var groupMinTime = groupedTimeTag.Min(x => x.Item2);
                            if (groupMinTime == null)
                                return null;

                            return groupedTimeTag.Where(x => x.Index < groupedTimeTag.Key && x.Item2 > groupMinTime).ToList();

                        default:
                            return null;
                    }
                }

                List<T> findSelfInvalid()
                {
                    switch (self)
                    {
                        case SelfCheck.BasedOnStart:
                            var maxStartTime = startTimeGroup.Max(x => x.Item2);
                            if (maxStartTime == null)
                                return null;

                            return endTimeGroup.Where(x => x.Item2.Value < maxStartTime.Value).ToList();

                        case SelfCheck.BasedOnEnd:
                            var minEndTime = endTimeGroup.Min(x => x.Item2);
                            if (minEndTime == null)
                                return null;

                            return startTimeGroup.Where(x => x.Item2.Value > minEndTime.Value).ToList();

                        default:
                            return null;
                    }
                }
            }

            return Sort(invalidList.Distinct().ToArray());
        }
    }
}
