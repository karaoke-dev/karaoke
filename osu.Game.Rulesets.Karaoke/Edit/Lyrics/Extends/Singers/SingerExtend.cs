// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Singers
{
    public class SingerExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Left;

        public override float ExtendWidth => 300;

        public SingerExtend()
        {
            Children = new[]
            {
                new SingerEditSection(),
            };
        }
    }
}
