// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Carets
{
    public abstract class DrawableCaret<TCaret> : DrawableCaret where TCaret : class, ICaretPosition
    {
        protected DrawableCaret(DrawableCaretType type)
            : base(type)
        {
        }

        protected static float GetAlpha(DrawableCaretType type) =>
            type switch
            {
                DrawableCaretType.Caret => 1,
                DrawableCaretType.HoverCaret => 0.5f,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

        public override void Apply(ICaretPosition caret)
        {
            if (caret is not TCaret tCaret)
                throw new InvalidCastException();

            Apply(tCaret);
        }

        protected abstract void Apply(TCaret caret);
    }

    public abstract class DrawableCaret : CompositeDrawable
    {
        public readonly DrawableCaretType Type;

        protected DrawableCaret(DrawableCaretType type)
        {
            Type = type;
        }

        public abstract void Apply(ICaretPosition caret);

        public abstract void TriggerDisallowEditEffect(LyricEditorMode editorMode);
    }
}
