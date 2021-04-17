// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Checker.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks
{
    public class LyricCheckerConfig : IHasConfig<LyricCheckerConfig>
    {
        public GroupCheck TimeTagTimeGroupCheck { get; set; }

        public SelfCheck TimeTagTimeSelfCheck { get; set; }

        public TextTagsUtils.Sorting RubyPositionSorting { get; set; }

        public TextTagsUtils.Sorting RomajiPositionSorting { get; set; }

        public LyricCheckerConfig CreateDefaultConfig()
        {
            return new LyricCheckerConfig
            {
                TimeTagTimeGroupCheck = GroupCheck.Asc,
                TimeTagTimeSelfCheck = SelfCheck.BasedOnStart,
                RubyPositionSorting = TextTagsUtils.Sorting.Asc,
                RomajiPositionSorting = TextTagsUtils.Sorting.Asc,
            };
        }
    }
}
