// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Carets;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Parts;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics
{
    public class LyricControl : Container
    {
        private const int time_tag_spacing = 8;

        private readonly EditorLyricPiece lyricPiece;
        private readonly Container timeTagContainer;
        private readonly Container caretContainer;

        [Resolved(canBeNull: true)]
        private LyricManager lyricManager { get; set; }

        [Resolved]
        private ILyricEditorState state { get; set; }

        public Lyric Lyric { get; }

        public LyricControl(Lyric lyric)
        {
            Lyric = lyric;
            CornerRadius = 5;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding { Bottom = 10 };
            Children = new Drawable[]
            {
                lyricPiece = new EditorLyricPiece(lyric)
                {
                    ApplyFontAction = font =>
                    {
                        // need to delay until karaoke text has been calculated.
                        ScheduleAfterChildren(UpdateTimeTags);
                        caretContainer.Height = font.LyricTextFontInfo.LyricTextFontInfo.CharSize * 1.7f;
                    }
                },
                timeTagContainer = new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                },
                caretContainer = new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                }
            };

            lyricPiece.RomajiTagsBindable.BindValueChanged(e =>
            {
                var displayRomaji = e?.NewValue?.Any() ?? false;
                var marginWidth = displayRomaji ? 30 : 15;
                timeTagContainer.Margin = new MarginPadding { Bottom = marginWidth };
                caretContainer.Margin = new MarginPadding { Bottom = marginWidth };
            }, true);

            lyricPiece.TimeTagsBindable.BindValueChanged(e =>
            {
                ScheduleAfterChildren(UpdateTimeTags);
            });
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (lyricManager == null)
                return false;

            if (!isTrigger(state.Mode))
                return false;

            var position = ToLocalSpace(e.ScreenSpaceMousePosition).X / 2;
            var index = lyricPiece.GetHoverIndex(position);

            switch (state.Mode)
            {
                case Mode.EditMode:
                case Mode.TypingMode:
                    state.MoveHoverCaretToTargetPosition(new TextCaretPosition(Lyric, TextIndexUtils.ToStringIndex(index)));
                    break;

                case Mode.TimeTagEditMode:

                    state.MoveHoverCaretToTargetPosition(new TimeTagIndexCaretPosition(Lyric, index));
                    break;

                case Mode.EditNoteMode:
                    state.MoveHoverCaretToTargetPosition(new EditNoteCaretPosition(Lyric));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state.Mode));
            }

            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!isTrigger(state.Mode))
                return;

            // lost hover caret and time-tag caret
            state.ClearHoverCaretPosition();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (!isTrigger(state.Mode))
                return false;

            // place hover caret to target position.
            var position = state.BindableHoverCaretPosition.Value;
            state.MoveCaretToTargetPosition(position);

            return true;
        }

        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            if (!isTrigger(state.Mode))
                return false;

            // todo : not really sure is ok to split time-tag by double click?
            // need to make an ux research.
            var position = state.BindableCaretPosition.Value;

            if (position is TextCaretPosition textCaretPosition)
            {
                lyricManager?.SplitLyric(Lyric, textCaretPosition.Index);
                return true;
            }

            throw new NotSupportedException(nameof(position));
        }

        [BackgroundDependencyLoader]
        private void load(EditorClock clock)
        {
            lyricPiece.Clock = clock;
            state.BindableMode.BindValueChanged(e =>
            {
                // initial default caret here
                CreateCaret(e.NewValue);
            }, true);

            // update change if caret changed.
            state.BindableHoverCaretPosition.BindValueChanged(e =>
            {
                UpdateCaretPosition(e.NewValue, true);
            });
            state.BindableCaretPosition.BindValueChanged(e =>
            {
                UpdateCaretPosition(e.NewValue, false);
            });
        }

        protected void CreateCaret(Mode mode)
        {
            caretContainer.Clear();

            // create preview and real caret
            addCaret(false);
            addCaret(true);

            void addCaret(bool isPreview)
            {
                var caret = createCaret(mode);
                if (caret == null)
                    return;

                caret.Hide();
                caret.Anchor = Anchor.BottomLeft;
                caret.Origin = Anchor.BottomLeft;

                if (caret is IDrawableCaret drawableCaret)
                    drawableCaret.Preview = isPreview;

                caretContainer.Add(caret);
            }

            static Drawable createCaret(Mode mode)
            {
                switch (mode)
                {
                    case Mode.ViewMode:
                        return null;

                    case Mode.EditMode:
                        return new DrawableLyricSplitterCaret();

                    case Mode.TypingMode:
                        return new DrawableLyricInputCaret();

                    case Mode.EditNoteMode:
                        return null;

                    case Mode.RecordMode:
                        return new DrawableTimeTagRecordCaret();

                    case Mode.TimeTagEditMode:
                        return new DrawableTimeTagEditCaret();

                    default:
                        throw new IndexOutOfRangeException(nameof(mode));
                }
            }
        }

        protected void UpdateCaretPosition(ICaretPosition position, bool hover)
        {
            if (position == null)
                return;

            var caret = caretContainer.OfType<IDrawableCaret>().FirstOrDefault(x => x.Preview == hover);
            if (caret == null)
                return;

            if (position.Lyric != Lyric)
            {
                caret.Hide();
                return;
            }

            float caretPosition = 0;

            switch (position)
            {
                case TextCaretPosition textCaretPosition:
                    caretPosition = textIndexPosition(textCaretPosition.Index);
                    break;

                case TimeTagIndexCaretPosition indexCaretPosition:
                    caretPosition = textIndexPosition(indexCaretPosition.Index);
                    break;

                case TimeTagCaretPosition timeTagCaretPosition:
                    var timeTag = timeTagCaretPosition.TimeTag;
                    caretPosition = textIndexPosition(timeTag.Index) + extraSpacing(timeTag);
                    break;

                default:
                    throw new NotSupportedException(nameof(position));
            }

            // set position
            if (caret is DrawableLyricInputCaret inputCaret)
            {
                inputCaret.DisplayAt(new Vector2(caretPosition, 0), null);
            }
            else if (caret is Drawable drawable)
            {
                drawable.X = caretPosition;
            }

            // set other property
            if (caret is IHasCaretPosition hasCaretPosition)
            {
                hasCaretPosition.CaretPosition = position;
            }
            else if (caret is IHasTimeTag hasTimeTag)
            {
                hasTimeTag.TimeTag = (position as TimeTagCaretPosition)?.TimeTag;
            }

            caret.Show();
        }

        protected void UpdateTimeTags()
        {
            timeTagContainer.Clear();
            var timeTags = lyricPiece.TimeTagsBindable.Value;
            if (timeTags == null)
                return;

            foreach (var timeTag in timeTags)
            {
                var spacing = textIndexPosition(timeTag.Index) + extraSpacing(timeTag);
                timeTagContainer.Add(new DrawableTimeTag(new TimeTagCaretPosition(Lyric, timeTag))
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    X = spacing
                });
            }
        }

        private float textIndexPosition(int textIndex)
        {
            var isEnd = Lyric.Text?.Length <= textIndex;
            var percentage = isEnd ? 1 : 0;
            var offset = isEnd ? 10 : -10; // todo : might have better way to get position.
            return lyricPiece.GetPercentageWidth(textIndex, textIndex + 1, percentage) * 2 + offset;
        }

        private float textIndexPosition(TextIndex textIndex)
        {
            var isStart = textIndex.State == TextIndex.IndexState.Start;
            var percentage = isStart ? 0 : 1;
            return lyricPiece.GetPercentageWidth(textIndex, textIndex, percentage) * 2;
        }

        private float extraSpacing(TimeTag timeTag)
        {
            var isStart = timeTag.Index.State == TextIndex.IndexState.Start;
            var timeTags = isStart ? lyricPiece.TimeTagsBindable.Value.Reverse() : lyricPiece.TimeTagsBindable.Value;
            var duplicatedTagAmount = timeTags.SkipWhile(t => t != timeTag).Count(x => x.Index == timeTag.Index) - 1;
            var spacing = duplicatedTagAmount * time_tag_spacing * (isStart ? 1 : -1);
            return spacing;
        }

        private bool isTrigger(Mode mode)
            => mode == Mode.EditMode || mode == Mode.TypingMode || mode == Mode.EditNoteMode || mode == Mode.TimeTagEditMode;
    }
}
