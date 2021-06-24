// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;
        public override float ExtendWidth => 300;

        public TimeTagExtend()
        {
            Children = new Drawable[]
            {
                new TimeTagEditModeSection(),
                new TimeTagAutoGenerateSection(),
                new TimeTagConfigSection(),
                new TimeTagIssueSection(),
            };
        }
    }
}
