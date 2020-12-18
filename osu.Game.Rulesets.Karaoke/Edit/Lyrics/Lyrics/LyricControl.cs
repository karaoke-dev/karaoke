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

            // todo : get real index.
            var position = ToLocalSpace(e.ScreenSpaceMousePosition).X / 2;
            var index = drawableLyric.GetHoverIndex(position);
            stateManager.MoveHoverCursorToTargetPosition(Lyric, index);
            return base.OnMouseMove(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            // lost hover cursor and time-tag cursor
            stateManager.ClearHoverCursorPosition();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            var timeTagIndex = stateManager.BindableCursorPosition.Value.Index;
            var splitPosition = timeTagIndex.Index + timeTagIndex.State == TimeTagIndex.IndexState.End ? 1 : 0;

            // get index then cut.
            lyricManager?.SplitLyric(Lyric, splitPosition);
            return base.OnClick(e);
        }

        [BackgroundDependencyLoader(true)]
        private void load(IFrameBasedClock framedClock, TimeTagManager timeTagManager)
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
            cursorContainer.Add(createCursor(mode, false).With(e =>
            {
                e.Hide();
            }));
            cursorContainer.Add(createCursor(mode, true).With(e =>
            {
                e.Hide();
            }));

            static Drawable createCursor(Mode mode, bool isPreview)
            {
                switch (mode)
                {
                    case Mode.ViewMode:
                        return null;
                    case Mode.EditMode:
                        return new DrawableLyricSplitterCursor();
                    case Mode.RecordMode:
                        return new DrawableTimeTagCursor();
                    case Mode.TimeTagEditMode:
                        return new DrawableTimeTagRecordCursor();
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
            if (!drawableLyric.TimeTagsBindable.Value.Contains(timeTag))
                return;

            var cursor = cursorContainer.OfType<DrawableTimeTagRecordCursor>().FirstOrDefault(x => x.Preview == preview);
            if (cursor == null)
                return;

            var spacing = timeTagIndexPosition(timeTag.Index) + extraSpacing(timeTag);
            cursor.X = spacing;
            cursor.TimeTag = timeTag;
        }

        protected void UpdateCursor(CursorPosition position, bool preview)
        {
            if (position.Lyric != Lyric)
                return;

            var cursor = cursorContainer.OfType<IDrawableCursor>().FirstOrDefault(x => x.Preview == preview);
            if (cursor == null)
                return;

            var spacing = timeTagIndexPosition(position.Index) - 10;
            if (cursor is Drawable drawableCursor)
            {
                drawableCursor.X = spacing;
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
    }
}
