// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets
{
    public abstract class DrawableCaret : CompositeDrawable
    {
        public readonly bool Preview;

        protected DrawableCaret(bool preview)
        {
            Preview = preview;
        }
    }
}
