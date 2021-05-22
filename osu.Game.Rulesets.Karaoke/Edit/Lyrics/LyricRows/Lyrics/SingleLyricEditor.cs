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
    public class SingleLyricEditor : Container
    {
        [Cached]
        private readonly EditorLyricPiece lyricPiece;

        private readonly Container timeTagContainer;
        private readonly Container caretContainer;

        [Resolved(canBeNull: true)]
        private LyricManager lyricManager { get; set; }

        [Resolved]
        private ILyricEditorState state { get; set; }

        public Lyric Lyric { get; }

        public SingleLyricEditor(Lyric lyric)
        {
            Lyric = lyric;
            CornerRadius = 5;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding { Bottom = 10 };
            Children = new Drawable[]
            {
                lyricPiece = new EditorLyricPiece(lyric),
                timeTagContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                },
                caretContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                }
            };

            lyricPiece.TimeTagsBindable.BindValueChanged(e =>
            {
                ScheduleAfterChildren(UpdateTimeTags);
            }, true);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (lyricManager == null)
                return false;

            if (!isTrigger(state.Mode))
                return false;

            var position = ToLocalSpace(e.ScreenSpaceMousePosition).X;
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
                    state.MoveHoverCaretToTargetPosition(new NavigateCaretPosition(Lyric));
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
                // initial default caret.
                InitializeCaret(e.NewValue);

                // Initial blueprint container.
                InitializeBlueprint(e.NewValue);
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

        protected void InitializeBlueprint(Mode mode)
        {
            // remove all exist blueprint container
            RemoveAll(x => x is RubyRomajiBlueprintContainer || x is TimeTagBlueprintContainer);

            // create preview and real caret
            var blueprintContainer = createBlueprintContainer(mode, Lyric);
            if (blueprintContainer == null)
                return;

            AddInternal(blueprintContainer);

            static Drawable createBlueprintContainer(Mode mode, Lyric lyric)
            {
                switch (mode)
                {
                    case Mode.RubyRomajiMode:
                        return new RubyRomajiBlueprintContainer(lyric);

                    // todo : might think is this really needed because it'll use cannot let user clicking time-tag.
                    // or just let it cannot interact.
                    case Mode.TimeTagEditMode:
                        return new TimeTagBlueprintContainer(lyric);

                    default:
                        return null;
                }
            }
        }

        protected void InitializeCaret(Mode mode)
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

                    case Mode.RubyRomajiMode:
                        return null;

                    case Mode.EditNoteMode:
                        return null;

                    case Mode.RecordMode:
                        return new DrawableTimeTagRecordCaret();

                    case Mode.TimeTagEditMode:
                        return new DrawableTimeTagEditCaret();

                    case Mode.Layout:
                    case Mode.Singer:
                    case Mode.Language:
                        return null;

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

            Vector2 caretPosition = getCaretPosition();

            // set position
            if (caret is DrawableLyricInputCaret inputCaret)
            {
                inputCaret.DisplayAt(caretPosition, null);
            }
            else if (caret is Drawable drawable)
            {
                drawable.Position = caretPosition;
            }

            // todo : should have a better way to set height to input or split caret
            if (caret is DrawableLyricInputCaret || caret is DrawableLyricSplitterCaret)
            {
                if (caret is Drawable drawable)
                {
                    var textHeight = lyricPiece.GetTextHeight();
                    drawable.Height = textHeight;
                }
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

            Vector2 getCaretPosition()
            {
                var textHeight = lyricPiece.GetTextHeight();

                switch (position)
                {
                    case TextCaretPosition textCaretPosition:
                        var originPosition = lyricPiece.GetTextIndexPosition(new TextIndex(textCaretPosition.Index));
                        return new Vector2(originPosition.X, originPosition.Y - textHeight);

                    case TimeTagIndexCaretPosition indexCaretPosition:
                        return lyricPiece.GetTextIndexPosition(indexCaretPosition.Index);

                    case TimeTagCaretPosition timeTagCaretPosition:
                        var timeTag = timeTagCaretPosition.TimeTag;
                        return lyricPiece.GetTimeTagPosition(timeTag);

                    default:
                        throw new NotSupportedException(nameof(position));
                }
            }
        }

        protected void UpdateTimeTags()
        {
            timeTagContainer.Clear();
            var timeTags = lyricPiece.TimeTagsBindable.Value;
            if (timeTags == null)
                return;

            foreach (var timeTag in timeTags)
            {
                var position = lyricPiece.GetTimeTagPosition(timeTag);
                timeTagContainer.Add(new DrawableTimeTag(new TimeTagCaretPosition(Lyric, timeTag))
                {
                    Position = position
                });
            }
        }

        private bool isTrigger(Mode mode)
            => mode == Mode.EditMode || mode == Mode.TypingMode || mode == Mode.EditNoteMode || mode == Mode.TimeTagEditMode;
    }
}
