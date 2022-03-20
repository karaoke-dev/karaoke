// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Carets;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public class CaretLayer : CompositeDrawable
    {
        [Resolved]
        private EditorLyricPiece lyricPiece { get; set; }

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        [Resolved]
        private ILyricsChangeHandler lyricsChangeHandler { get; set; }

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

        private readonly Lyric lyric;

        public CaretLayer(Lyric lyric)
        {
            this.lyric = lyric;

            bindableMode.BindValueChanged(e =>
            {
                // initial default caret.
                InitializeCaret(e.NewValue);
            });
        }

        protected void InitializeCaret(LyricEditorMode mode)
        {
            ClearInternal();

            // create preview and real caret
            addCaret(false);
            addCaret(true);

            void addCaret(bool isPreview)
            {
                var caret = createCaret(mode, isPreview);
                if (caret == null)
                    return;

                caret.Hide();

                AddInternal(caret);
            }

            static DrawableCaret createCaret(LyricEditorMode mode, bool isPreview)
            {
                switch (mode)
                {
                    case LyricEditorMode.View:
                        return null;

                    case LyricEditorMode.Manage:
                        return new DrawableLyricSplitterCaret(isPreview);

                    case LyricEditorMode.Typing:
                        return new DrawableLyricInputCaret(isPreview);

                    case LyricEditorMode.Language:
                    case LyricEditorMode.EditRuby:
                    case LyricEditorMode.EditRomaji:
                        return null;

                    case LyricEditorMode.CreateTimeTag:
                        return new DrawableTimeTagEditCaret(isPreview);

                    case LyricEditorMode.RecordTimeTag:
                        return new DrawableTimeTagRecordCaret(isPreview);

                    case LyricEditorMode.AdjustTimeTag:
                    case LyricEditorMode.EditNote:
                    case LyricEditorMode.Singer:
                        return null;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (!lyricCaretState.CaretEnabled)
                return false;

            var mode = bindableMode.Value;
            float position = ToLocalSpace(e.ScreenSpaceMousePosition).X;

            switch (mode)
            {
                case LyricEditorMode.View:
                    break;

                case LyricEditorMode.Manage:
                    int cuttingLyricStringIndex = Math.Clamp(TextIndexUtils.ToStringIndex(lyricPiece.GetHoverIndex(position)), 0, lyric.Text.Length - 1);
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TextCaretPosition(lyric, cuttingLyricStringIndex));
                    break;

                case LyricEditorMode.Typing:
                    int typingStringIndex = TextIndexUtils.ToStringIndex(lyricPiece.GetHoverIndex(position));
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TextCaretPosition(lyric, typingStringIndex));
                    break;

                case LyricEditorMode.Language:
                    break;

                case LyricEditorMode.EditRuby:
                    lyricCaretState.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(lyric));
                    break;

                case LyricEditorMode.EditRomaji:
                    lyricCaretState.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(lyric));
                    break;

                case LyricEditorMode.CreateTimeTag:
                    var textIndex = lyricPiece.GetHoverIndex(position);
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TimeTagIndexCaretPosition(lyric, textIndex));
                    break;

                case LyricEditorMode.RecordTimeTag:
                    var timeTag = lyricPiece.GetHoverTimeTag(position);
                    lyricCaretState.MoveHoverCaretToTargetPosition(new TimeTagCaretPosition(lyric, timeTag));
                    break;

                case LyricEditorMode.AdjustTimeTag:
                case LyricEditorMode.EditNote:
                case LyricEditorMode.Singer:
                    lyricCaretState.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(lyric));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
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
                    if (mode == LyricEditorMode.Manage)
                        lyricsChangeHandler.Split(textCaretPosition.Index);
                    return true;

                default:
                    return false;
            }
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode.BindTo(state.BindableMode);
        }
    }
}
