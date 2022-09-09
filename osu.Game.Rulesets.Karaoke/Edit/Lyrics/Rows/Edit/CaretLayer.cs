// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Edit.Carets;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Edit
{
    public class CaretLayer : BaseLayer
    {
        [Resolved, AllowNull]
        private EditorKaraokeSpriteText karaokeSpriteText { get; set; }

        [Resolved, AllowNull]
        private ILyricCaretState lyricCaretState { get; set; }

        [Resolved, AllowNull]
        private ILyricsChangeHandler lyricsChangeHandler { get; set; }

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
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
        private void load(ILyricEditorState state)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableCaretPositionAlgorithm.BindTo(lyricCaretState.BindableCaretPositionAlgorithm);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (!lyricCaretState.CaretEnabled)
                return false;

            float position = ToLocalSpace(e.ScreenSpaceMousePosition).X;

            switch (bindableCaretPositionAlgorithm.Value)
            {
                case CuttingCaretPositionAlgorithm:
                    int cuttingLyricStringIndex = Math.Clamp(TextIndexUtils.ToStringIndex(karaokeSpriteText.GetHoverIndex(position)), 0, Lyric.Text.Length - 1);
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TextCaretPosition(Lyric, cuttingLyricStringIndex));
                    break;

                case TypingCaretPositionAlgorithm:
                    int typingStringIndex = TextIndexUtils.ToStringIndex(karaokeSpriteText.GetHoverIndex(position));
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TextCaretPosition(Lyric, typingStringIndex));
                    break;

                case NavigateCaretPositionAlgorithm:
                    lyricCaretState.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(Lyric));
                    break;

                case TimeTagIndexCaretPositionAlgorithm:
                    var textIndex = karaokeSpriteText.GetHoverIndex(position);
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TimeTagIndexCaretPosition(Lyric, textIndex));
                    break;

                case TimeTagCaretPositionAlgorithm:
                    var timeTag = karaokeSpriteText.GetHoverTimeTag(position);
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TimeTagCaretPosition(Lyric, timeTag));
                    break;
            }

            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!lyricCaretState.CaretEnabled)
                return;

            // lost hover caret and time-tag caret
            lyricCaretState.ClearHoverCaretPosition();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (!lyricCaretState.CaretEnabled)
                return false;

            // place hover caret to target position.
            var position = lyricCaretState.BindableHoverCaretPosition.Value;
            if (position == null)
                return false;

            lyricCaretState.MoveCaretToTargetPosition(position);

            return true;
        }

        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            var mode = bindableMode.Value;
            var position = lyricCaretState.BindableCaretPosition.Value;

            switch (position)
            {
                case TextCaretPosition textCaretPosition:
                    if (mode == LyricEditorMode.Texting)
                        lyricsChangeHandler.Split(textCaretPosition.Index);
                    return true;

                default:
                    return false;
            }
        }
    }
}
