// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets
{
    public abstract class DrawableCaret<TCaret> : DrawableCaret where TCaret : class, ICaretPosition
    {
        private Bindable<ICaretPosition> caretPosition;

        protected DrawableCaret(bool preview)
            : base(preview)
        {
        }

        [BackgroundDependencyLoader]
        private void load(LyricCaretState lyricCaretState, EditorLyricPiece lyricPiece)
        {
            caretPosition = Preview ? lyricCaretState.BindableHoverCaretPosition.GetBoundCopy() : lyricCaretState.BindableCaretPosition.GetBoundCopy();
            caretPosition.BindValueChanged(e =>
            {
                var position = e.NewValue;
                if (position == null)
                    return;

                if (position.Lyric != lyricPiece.HitObject)
                {
                    Hide();
                    return;
                }

                Show();
                Apply(position as TCaret);
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
    }
}
