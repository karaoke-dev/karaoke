// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Carets;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics
{
    public class CaretLayer : BaseLayer
    {
        private readonly IBindable<ICaretPositionAlgorithm?> bindableCaretPositionAlgorithm = new Bindable<ICaretPositionAlgorithm?>();

        public CaretLayer(Lyric lyric)
            : base(lyric)
        {
            bindableCaretPositionAlgorithm.BindValueChanged(e =>
            {
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
                var caret = createCaret(bindableCaretPositionAlgorithm.Value, isPreview);
                if (caret == null)
                    return;

                caret.Hide();

                AddInternal(caret);
            }

            static DrawableCaret? createCaret(ICaretPositionAlgorithm? caretPositionAlgorithm, bool isPreview) =>
                caretPositionAlgorithm switch
                {
                    // cutting lyric
                    CuttingCaretPositionAlgorithm => new DrawableLyricSplitterCaret(isPreview),
                    // typing
                    TypingCaretPositionAlgorithm => new DrawableLyricInputCaret(isPreview),
                    // creat time-tag
                    TimeTagIndexCaretPositionAlgorithm => new DrawableTimeTagEditCaret(isPreview),
                    // record time-tag
                    TimeTagCaretPositionAlgorithm => new DrawableTimeTagRecordCaret(isPreview),
                    _ => null
                };
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            bindableCaretPositionAlgorithm.BindTo(lyricCaretState.BindableCaretPositionAlgorithm);
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
