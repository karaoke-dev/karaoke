// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Manage
{
    public class ManageExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        public ManageExtend()
        {
            Children = new[]
            {
                new ManageSwitchSpecialActionSection()
            };
        }
    }
}
