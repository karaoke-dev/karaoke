// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Skinning.Groups
{
    public class GroupBySingerNumber : BaseGroup<Lyric>
    {
        public int SingerNumber { get; set; }

        protected override bool InTheGroup(Lyric hitObject)
        {
            if (SingerNumber == 0)
                return false;

            return hitObject.Singers.Count == SingerNumber;
        }
    }
}
