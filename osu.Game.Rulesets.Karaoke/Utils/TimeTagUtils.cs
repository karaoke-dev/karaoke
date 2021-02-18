// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TimeTagUtils
    {
        /// <summary>
        /// Shifting time-tag.
        /// </summary>
        /// <param name="timeTag"></param>
        /// <param name="shifting"></param>
        /// <returns></returns>
        public static TimeTag ShiftingTimeTag(TimeTag timeTag, int shifting)
        {
            var index = TextIndexUtils.ShiftingIndex(timeTag.Index, shifting);
            var time = timeTag.Time;
            return new TimeTag(index, time);
        }

        /// <summary>
        /// Display string with time format
        /// </summary>
        /// <example>
        /// 02:32:155
        /// --:--:---
        /// </example>
        /// <param name="timeTag"></param>
        /// <returns></returns>
        public static string FormattedString(TimeTag timeTag)
        {
            return timeTag?.Time?.ToEditorFormattedString() ?? "--:--:---";
        }
    }
}
