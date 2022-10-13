// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Carets;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics
{
    public class CaretLayer : BaseLayer
    {
        private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();
        private readonly IBindable<ICaretPosition?> bindableHoverCaretPosition = new Bindable<ICaretPosition?>();

        public CaretLayer(Lyric lyric)
            : base(lyric)
        {
            bindableCaretPosition.BindValueChanged(e =>
            {
                if (e.OldValue?.GetType() != e.NewValue?.GetType())
                    updateDrawableCaret(DrawableCaretType.Caret);

                applyTheCaretPosition(e.NewValue, DrawableCaretType.Caret);
            }, true);

            bindableHoverCaretPosition.BindValueChanged(e =>
            {
                if (e.OldValue?.GetType() != e.NewValue?.GetType())
                    updateDrawableCaret(DrawableCaretType.HoverCaret);

                applyTheCaretPosition(e.NewValue, DrawableCaretType.HoverCaret);
            }, true);
        }

        private void updateDrawableCaret(DrawableCaretType type)
        {
            var oldCaret = InternalChildren.OfType<DrawableCaret>().FirstOrDefault(x => x.Type == type);
            if (oldCaret != null)
                RemoveInternal(oldCaret, true);

            var caret = createCaret(bindableCaretPosition.Value, type);
            if (caret == null)
                return;

            caret.Hide();

            AddInternal(caret);

            static DrawableCaret? createCaret(ICaretPosition? caretPositionAlgorithm, DrawableCaretType type) =>
                caretPositionAlgorithm switch
                {
                    // cutting lyric
                    CuttingCaretPosition => new DrawableLyricSplitterCaret(type),
                    // typing
                    TypingCaretPosition => new DrawableLyricInputCaret(type),
                    // creat time-tag
                    TimeTagIndexCaretPosition => new DrawableTimeTagEditCaret(type),
                    // record time-tag
                    TimeTagCaretPosition => new DrawableTimeTagRecordCaret(type),
                    _ => null
                };
        }

        private void applyTheCaretPosition(ICaretPosition? position, DrawableCaretType type)
        {
            if (position == null)
                return;

            var caret = InternalChildren.OfType<DrawableCaret>().FirstOrDefault(x => x.Type == type);
            if (caret == null)
                throw new NullReferenceException();

            if (position.Lyric != Lyric)
            {
                caret.Hide();
                return;
            }

            caret.Show();
            caret.ApplyCaretPosition(position);
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
            bindableHoverCaretPosition.BindTo(lyricCaretState.BindableHoverCaretPosition);
        }

        public override void UpdateDisableEditState(bool editable)
        {
            this.FadeTo(editable ? 1 : 0.7f, 100);
        }

        public override void TriggerDisallowEditEffect(LyricEditorMode editorMode)
        {
            InternalChildren.OfType<DrawableCaret>().ForEach(x => x.TriggerDisallowEditEffect(editorMode));
        }
    }
}
