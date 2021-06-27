// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets
{
    public abstract class DrawableCaret<TCaret> : CompositeDrawable where TCaret : ICaretPosition
    {
        protected readonly bool Preview;

        protected DrawableCaret(bool preview)
        {
            Preview = preview;
        }

        public abstract void Apply(TCaret caret);
    }
}
