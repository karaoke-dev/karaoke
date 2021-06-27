// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;

        public override float ExtendWidth => 300;

        public NoteExtend()
        {
            Children = new Drawable[]
            {
                new NoteEditModeSection(),
                new NoteConfigSection(),
                new NoteAutoGenerateSection(),
                new NoteIssueSection()
            };
        }
    }
}
