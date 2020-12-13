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
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Lyrics
{
    public class LyricControl : Container
    {
        private const int time_tag_spacing = 8;

        private readonly DrawableEditLyric drawableLyric;
        private readonly Container timeTagContainer;
        private readonly Container timeTagCursorContainer;
        private readonly Container splitCursorContainer;

        [Resolved(canBeNull: true)]
        private LyricManager lyricManager { get; set; }

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
                    ApplyFontAction = () =>
                    {
                        // todo : need to delay until karaoke text has been calculated.
                        ScheduleAfterChildren(UpdateTimeTags);
                    }
                },
                timeTagContainer = new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                },
                timeTagCursorContainer = new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                },
                splitCursorContainer = new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                }
            };

            drawableLyric.TimeTagsBindable.BindValueChanged(e =>
            {
                ScheduleAfterChildren(UpdateTimeTags);
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            // todo : get real index.
            lyricManager?.UpdateSplitCursorPosition(Lyric, 2);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            lyricManager?.ClearSplitCursorPosition();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            // todo : get real index.
            lyricManager?.SplitLyric(Lyric, 2);
            return base.OnClick(e);
        }

        [BackgroundDependencyLoader(true)]
        private void load(IFrameBasedClock framedClock, TimeTagManager timeTagManager)
        {
            drawableLyric.Clock = framedClock;
            timeTagManager?.BindableCursorPosition.BindValueChanged(e =>
            {
                UpdateTimeTagCursoe(e.NewValue);
            }, true);
            lyricManager?.BindableSplitLyric.BindValueChanged(e =>
            {
                UpdateSplitter();
            });
            lyricManager?.BindableSplitPosition.BindValueChanged(e =>
            {
                UpdateSplitter();
            });
        }

        protected void UpdateTimeTagCursoe(TimeTag cursor)
        {
            timeTagCursorContainer.Clear();
            if (drawableLyric.TimeTagsBindable.Value.Contains(cursor))
            {
                var spacing = timeTagIndexPosition(cursor.Index) + extraSpacing(cursor);
                timeTagCursorContainer.Add(new DrawableTimeTagCursor(cursor)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    X = spacing
                });
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

        protected void UpdateSplitter()
        {
            splitCursorContainer.Clear();
            var lyric = lyricManager?.BindableSplitLyric.Value;
            var index = lyricManager?.BindableSplitPosition.Value;
            if (lyric != Lyric || index == null)
                return;

            
            var spacing = textIndexPosition(index.Value);
            splitCursorContainer.Add(new DrawableLyricSplitterCursor
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                X = spacing,
            });
        }

        private float textIndexPosition(int index)
            => timeTagIndexPosition(new TimeTagIndex(index)) - 10;

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
