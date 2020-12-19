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
            var timeTagIndex = stateManager.BindableHoverCursorPosition.Value.Index;
            stateManager.MoveCursorToTargetPosition(Lyric, timeTagIndex);

            return true;
        }

        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            if (!isTrigger(stateManager.Mode))
                return false;

            // todo : not really sure is ok to split time-tag by double cliek?
            // need to make an ux research.
            var position = stateManager.BindableHoverCursorPosition.Value;

            switch (stateManager.Mode)
            {
                case Mode.EditMode:
                    var splitPosition = TimeTagIndexUtils.ToLyricIndex(position.Index);
                    lyricManager?.SplitLyric(Lyric, splitPosition);
                    return true;

                default:
                    return base.OnDoubleClick(e);
            }
        }

        [BackgroundDependencyLoader]
        private void load(IFrameBasedClock framedClock)
        {
            drawableLyric.Clock = framedClock;
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
            addCursor(mode, false);
            addCursor(mode, true);

            void addCursor(Mode mode, bool isPreview)
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
                var spacing = timeTagIndexPosition(timeTag.Index) + extraSpacing(timeTag);
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

            var spacing = timeTagIndexPosition(timeTag.Index) + extraSpacing(timeTag);
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

            cursor.Show();

            if (cursor is Drawable drawableCursor)
            {
                var index = position.Index;
                if (stateManager.Mode == Mode.EditMode)
                    index = new TimeTagIndex(TimeTagIndexUtils.ToLyricIndex(index));

                var offset = 0;
                if (stateManager.Mode == Mode.EditMode)
                    offset = -10;

                drawableCursor.X = timeTagIndexPosition(index) + offset;
            }

            if (cursor is IHasCursorPosition cursorPosition)
            {
                cursorPosition.CursorPosition = position;
            }
        }

        private float timeTagIndexPosition(TimeTagIndex timeTagIndex)
        {
            var index = Math.Min(timeTagIndex.Index, Lyric.Text.Length - 1);
            var isStart = timeTagIndex.State == TimeTagIndex.IndexState.Start;
            var percentage = isStart ? 0 : 1;
            return drawableLyric.GetPercentageWidth(index, index + 1, percentage) * 2;
        }

        private float extraSpacing(TimeTag timeTag)
        {
            var isStart = timeTag.Index.State == TimeTagIndex.IndexState.Start;
            var timeTags = isStart ? drawableLyric.TimeTagsBindable.Value.Reverse() : drawableLyric.TimeTagsBindable.Value;
            var duplicatedTagAmount = timeTags.SkipWhile(t => t != timeTag).Count(x => x.Index == timeTag.Index) - 1;
            var spacing = duplicatedTagAmount * time_tag_spacing * (isStart ? 1 : -1);
            return spacing;
        }

        private bool isTrigger(Mode mode)
            => mode == Mode.EditMode || mode == Mode.TimeTagEditMode;
    }
}
