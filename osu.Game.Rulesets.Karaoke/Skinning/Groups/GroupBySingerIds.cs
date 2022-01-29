// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Skinning.Groups
{
    public class GroupBySingerIds : BaseGroup<Lyric>
    {
        public IReadOnlyList<int> SingerIds { get; set; }

        protected override bool InTheGroup(Lyric hitObject)
        {
            if (SingerIds == null)
                return false;

            return SingerIds.Any(x => hitObject.Singers.Contains(x));
        }
    }
}
