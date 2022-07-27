// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Texting
{
    public class TextingExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        public TextingExtend()
        {
            Children = new Drawable[]
            {
                new TextingEditModeSection(),
                new TextingSwitchSpecialActionSection()
            };
        }
    }
}
