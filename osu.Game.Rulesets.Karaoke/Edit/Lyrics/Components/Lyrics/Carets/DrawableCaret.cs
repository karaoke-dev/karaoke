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

        protected DrawableCaret(bool preview)
            : base(preview)
        {
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState, EditorKaraokeSpriteText karaokeSpriteText)
        {
            caretPosition = Preview ? lyricCaretState.BindableHoverCaretPosition.GetBoundCopy() : lyricCaretState.BindableCaretPosition.GetBoundCopy();
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

                if (position is not TCaret tCaret)
                    throw new InvalidCastException();

                Apply(tCaret);
            });
        }

        protected abstract void Apply(TCaret caret);
    }

    public abstract class DrawableCaret : CompositeDrawable
    {
        protected readonly bool Preview;

        protected DrawableCaret(bool preview)
        {
            Preview = preview;
        }

        public abstract void TriggerDisallowEditEffect(LyricEditorMode editorMode);
    }
}
