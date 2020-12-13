// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Lyrics
{
    public class LyricControl : Container
    {
        private const int time_tag_spacing = 4;

        private readonly DrawableEditLyric drawableLyric;
        private readonly Container timeTagContainer;
        private readonly Container timeTagCursorContainer;

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
                    Scale = new Vector2(2)
                },
                timeTagCursorContainer = new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativeSizeAxes = Axes.Both,
                    Scale = new Vector2(2)
                }
            };

            drawableLyric.TimeTagsBindable.BindValueChanged(e =>
            {
                ScheduleAfterChildren(UpdateTimeTags);
            });
        }

        [BackgroundDependencyLoader(true)]
        private void load(IFrameBasedClock framedClock, TimeTagManager timeTagManager)
        {
            drawableLyric.Clock = framedClock;
            timeTagManager?.BindableCursorPosition.BindValueChanged(e =>
            {
                UpdateTimeTagCursoe(e.NewValue);
            }, true);
        }

        public void UpdateTimeTagCursoe(TimeTag cursor)
        {
            timeTagCursorContainer.Clear();
            if (drawableLyric.TimeTagsBindable.Value.Contains(cursor))
            {
                var spacing = timeTagPosition(cursor);
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
                var spacing = timeTagPosition(timeTag);
                timeTagContainer.Add(new DrawableTimeTag(timeTag)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    X = spacing
                });
            }
        }

        private float timeTagPosition(TimeTag timeTag)
        {
            var index = Math.Min(timeTag.Index.Index, Lyric.Text.Length - 1);
            var isStart = timeTag.Index.State == TimeTagIndex.IndexState.Start;
            var percentage = isStart ? 0 : 1;
            var position = drawableLyric.GetPercentageWidth(index, index + 1, percentage);

            var timeTags = isStart ? drawableLyric.TimeTagsBindable.Value.Reverse() : drawableLyric.TimeTagsBindable.Value;
            var duplicatedTagAmount = timeTags.SkipWhile(t => t != timeTag).Count(x => x.Index == timeTag.Index) - 1;
            var spacing = duplicatedTagAmount * time_tag_spacing * (isStart ? 1 : -1);
            return position + spacing;
        }
    }
}
