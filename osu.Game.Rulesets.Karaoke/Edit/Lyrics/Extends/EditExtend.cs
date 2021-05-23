// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends
{
    public abstract class EditExtend : CompositeDrawable
    {
        public abstract ExtendDirection Direction { get; }

        public abstract float ExtendWidth { get; }
    }
}
