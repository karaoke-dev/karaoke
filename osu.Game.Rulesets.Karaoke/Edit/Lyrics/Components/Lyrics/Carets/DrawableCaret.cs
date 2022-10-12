// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Carets
{
    public abstract class DrawableCaret<TCaret> : DrawableCaret where TCaret : class, ICaretPosition
    {
        private IBindable<ICaretPosition> caretPosition;

        protected DrawableCaret(DrawableCaretType type)
            : base(type)
        {
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState, InteractableKaraokeSpriteText karaokeSpriteText)
        {
            caretPosition = Type == DrawableCaretType.HoverCaret ? lyricCaretState.BindableHoverCaretPosition.GetBoundCopy() : lyricCaretState.BindableCaretPosition.GetBoundCopy();
            caretPosition.BindValueChanged(e =>
            {
                var position = e.NewValue;
                if (position == null)
                    return;

                if (position.Lyric != karaokeSpriteText.HitObject)
                {
                    Hide();
                    return;
                }

                Show();
                Apply(position);
            });
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
        protected readonly DrawableCaretType Type;

        protected DrawableCaret(DrawableCaretType type)
        {
            Type = type;
        }

        public abstract void Apply(ICaretPosition caret);

        public abstract void TriggerDisallowEditEffect(LyricEditorMode editorMode);
    }
}
