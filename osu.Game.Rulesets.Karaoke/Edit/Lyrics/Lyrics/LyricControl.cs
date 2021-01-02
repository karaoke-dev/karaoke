// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Play;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Lyrics
{
    public class LyricControl : Container
    {
        private const int time_tag_spacing = 8;

        private readonly DrawableEditLyric drawableLyric;
        private readonly Container timeTagContainer;
        private readonly Container cursorContainer;

        [Resolved(canBeNull: true)]
        private LyricManager lyricManager { get; set; }

        [Resolved(canBeNull: true)]
        private TimeTagManager timeTagManager { get; set; }

        [Resolved]
        private LyricEditorStateManager stateManager { get; set; }

        public Lyric Lyric { get; }

        public LyricControl(Lyric lyric)
        {
            Lyric = lyric;
            CornerRadius = 5;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding { Bottom = 10 };
            Children = new Drawable[]
            {
                drawableLyric = new DrawableEditLyric(lyric)
                {
                    ApplyFontAction = font =>
                    {
                        // need to delay until karaoke text has been calculated.
                        ScheduleAfterChildren(UpdateTimeTags);
                        cursorContainer.Height = font.LyricTextFontInfo.LyricTextFontInfo.CharSize * 1.7f;
                    }
                },
                timeTagContainer = new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                },
                cursorContainer = new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.X,
                }
            };

            drawableLyric.RomajiTagsBindable.BindValueChanged(e =>
            {
                var displayRomaji = e?.NewValue?.Any() ?? false;
                var marginWidth = displayRomaji ? 30 : 15;
                timeTagContainer.Margin = new MarginPadding { Bottom = marginWidth };
                cursorContainer.Margin = new MarginPadding { Bottom = marginWidth };
            }, true);

            drawableLyric.TimeTagsBindable.BindValueChanged(e =>
            {
                ScheduleAfterChildren(UpdateTimeTags);
            });
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (lyricManager == null)
                return false;

            if (!isTrigger(stateManager.Mode))
                return false;

            // todo : get real index.
            var position = ToLocalSpace(e.ScreenSpaceMousePosition).X / 2;
            var index = drawableLyric.GetHoverIndex(position);
            stateManager.MoveHoverCursorToTargetPosition(Lyric, index);
            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!isTrigger(stateManager.Mode))
                return;

            // lost hover cursor and time-tag cursor
            stateManager.ClearHoverCursorPosition();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (!isTrigger(stateManager.Mode))
                return false;

            // place hover cursor to target position.
            var index = stateManager.BindableHoverCursorPosition.Value.Index;
            stateManager.MoveCursorToTargetPosition(Lyric, index);

            return true;
        }

        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            if (!isTrigger(stateManager.Mode))
                return false;

            // todo : not really sure is ok to split time-tag by double click?
            // need to make an ux research.
            var position = stateManager.BindableHoverCursorPosition.Value;

            switch (stateManager.Mode)
            {
                case Mode.EditMode:
                    var splitPosition = TextIndexUtils.ToLyricIndex(position.Index);
                    lyricManager?.SplitLyric(Lyric, splitPosition);
                    return true;

                default:
                    return base.OnDoubleClick(e);
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            //drawableLyric.Clock = clock;
            stateManager.BindableMode.BindValueChanged(e =>
            {
                // initial default cursor here
                CreateCursor(e.NewValue);
            }, true);

            // update change if cursor changed.
            stateManager.BindableHoverCursorPosition.BindValueChanged(e =>
            {
                UpdateCursor(e.NewValue, true);
            });
            stateManager.BindableCursorPosition.BindValueChanged(e =>
            {
                UpdateCursor(e.NewValue, false);
            });

            // update change if record cursor changed.
            stateManager.BindableHoverRecordCursorPosition.BindValueChanged(e =>
            {
                UpdateTimeTagCursor(e.NewValue, true);
            }, true);
            stateManager.BindableRecordCursorPosition.BindValueChanged(e =>
            {
                UpdateTimeTagCursor(e.NewValue, false);
            }, true);
        }

        protected void CreateCursor(Mode mode)
        {
            cursorContainer.Clear();

            // create preview and real cursor
            addCursor(false);
            addCursor(true);

            void addCursor(bool isPreview)
            {
                var cursor = createCursor(mode);
                if (cursor == null)
                    return;

                cursor.Hide();
                cursor.Anchor = Anchor.BottomLeft;
                cursor.Origin = Anchor.BottomLeft;

                if (cursor is IDrawableCursor drawableCursor)
                    drawableCursor.Preview = isPreview;

                cursorContainer.Add(cursor);
            }

            static Drawable createCursor(Mode mode)
            {
                switch (mode)
                {
                    case Mode.ViewMode:
                        return null;

                    case Mode.EditMode:
                        return new DrawableLyricSplitterCursor();

                    case Mode.TypingMode:
                        return new DrawableLyricInputCursor();

                    case Mode.RecordMode:
                        return new DrawableTimeTagRecordCursor();

                    case Mode.TimeTagEditMode:
                        return new DrawableTimeTagEditCursor();

                    default:
                        throw new IndexOutOfRangeException(nameof(mode));
                }
            }
        }

        protected void UpdateTimeTags()
        {
            timeTagContainer.Clear();
            var timeTags = drawableLyric.TimeTagsBindable.Value;
            if (timeTags == null)
                return;

            foreach (var timeTag in timeTags)
            {
                var spacing = textIndexPosition(timeTag.Index) + extraSpacing(timeTag);
                timeTagContainer.Add(new DrawableTimeTag(timeTag)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    X = spacing
                });
            }
        }

        protected void UpdateTimeTagCursor(TimeTag timeTag, bool preview)
        {
            var cursor = cursorContainer.OfType<DrawableTimeTagRecordCursor>().FirstOrDefault(x => x.Preview == preview);
            if (cursor == null)
                return;

            if (!drawableLyric.TimeTagsBindable.Value.Contains(timeTag))
            {
                cursor.Hide();
                return;
            }

            cursor.Show();

            var spacing = textIndexPosition(timeTag.Index) + extraSpacing(timeTag);
            cursor.X = spacing;
            cursor.TimeTag = timeTag;
        }

        protected void UpdateCursor(CursorPosition position, bool preview)
        {
            var cursor = cursorContainer.OfType<IDrawableCursor>().FirstOrDefault(x => x.Preview == preview);
            if (cursor == null)
                return;

            if (position.Lyric != Lyric)
            {
                cursor.Hide();
                return;
            }

            if (!(cursor is Drawable drawableCursor))
                return;

            var index = position.Index;
            if (stateManager.Mode == Mode.EditMode || stateManager.Mode == Mode.TypingMode)
                index = new TextIndex(TextIndexUtils.ToLyricIndex(index));

            var offset = 0;
            if (stateManager.Mode == Mode.EditMode || stateManager.Mode == Mode.TypingMode)
                offset = -10;

            var pos = new Vector2(textIndexPosition(index) + offset, 0);

            if (cursor is DrawableLyricInputCursor inputCursor)
            {
                inputCursor.DisplayAt(pos, null);
            }
            else
            {
                drawableCursor.Position = pos;
            }

            if (cursor is IHasCursorPosition cursorPosition)
            {
                cursorPosition.CursorPosition = position;
            }

            // show after cursor position has been ready.
            cursor.Show();
        }

        private float textIndexPosition(TextIndex textIndex)
        {
            var index = Math.Min(textIndex.Index, Lyric.Text.Length - 1);
            var isStart = textIndex.State == TextIndex.IndexState.Start;
            var percentage = isStart ? 0 : 1;
            return drawableLyric.GetPercentageWidth(index, index + 1, percentage) * 2;
        }

        private float extraSpacing(TimeTag timeTag)
        {
            var isStart = timeTag.Index.State == TextIndex.IndexState.Start;
            var timeTags = isStart ? drawableLyric.TimeTagsBindable.Value.Reverse() : drawableLyric.TimeTagsBindable.Value;
            var duplicatedTagAmount = timeTags.SkipWhile(t => t != timeTag).Count(x => x.Index == timeTag.Index) - 1;
            var spacing = duplicatedTagAmount * time_tag_spacing * (isStart ? 1 : -1);
            return spacing;
        }

        private bool isTrigger(Mode mode)
            => mode == Mode.EditMode || mode == Mode.TypingMode || mode == Mode.TimeTagEditMode;
    }
}
