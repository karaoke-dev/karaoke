// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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

        public CaretLayer(Lyric lyric)
            : base(lyric)
        {
            bindableCaretPosition.BindValueChanged(e =>
            {
                if (e.OldValue?.GetType() == e.NewValue?.GetType())
                    return;

                // initial default caret.
                initializeCaret();
            }, true);
        }

        private void initializeCaret()
        {
            ClearInternal();

            // create preview and real caret
            addCaret(false);
            addCaret(true);

            void addCaret(bool isPreview)
            {
                var caret = createCaret(bindableCaretPosition.Value, isPreview);
                if (caret == null)
                    return;

                caret.Hide();

                AddInternal(caret);
            }

            static DrawableCaret? createCaret(ICaretPosition? caretPositionAlgorithm, bool isPreview) =>
                caretPositionAlgorithm switch
                {
                    // cutting lyric
                    CuttingCaretPosition => new DrawableLyricSplitterCaret(isPreview),
                    // typing
                    TypingCaretPosition => new DrawableLyricInputCaret(isPreview),
                    // creat time-tag
                    TimeTagIndexCaretPosition => new DrawableTimeTagEditCaret(isPreview),
                    // record time-tag
                    TimeTagCaretPosition => new DrawableTimeTagRecordCaret(isPreview),
                    _ => null
                };
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
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
