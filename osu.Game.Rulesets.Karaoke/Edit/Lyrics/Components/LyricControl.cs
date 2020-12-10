// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components
{
    public class LyricControl : Container
    {
        private readonly DrawableEditorLyric drawableLyric;

        public Lyric Lyric { get; }

        public LyricControl(Lyric lyric)
        {
            Lyric = lyric;
            CornerRadius = 5;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding { Bottom = 10 };
            InternalChildren = new Drawable[]
            {
                drawableLyric = new DrawableEditorLyric(lyric)
            };
        }

        [BackgroundDependencyLoader(true)]
        private void load(IFrameBasedClock framedClock, TimeTagManager timeTagManager)
        {
            drawableLyric.Clock = framedClock;
            timeTagManager?.BindableCursorPosition.BindValueChanged(e =>
            {
                drawableLyric?.UpdateTimeTagCursoe(e.NewValue);
            }, true);
        }

        public class DrawableEditorLyric : DrawableLyric
        {
            private const int time_tag_spacing = 4;

            private readonly Container timeTagContainer;
            private readonly Container timeTagCursorContainer;

            public DrawableEditorLyric(Lyric lyric)
                : base(lyric)
            {
                AddRangeInternal(new[]
                {
                    timeTagContainer = new Container
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    timeTagCursorContainer = new Container
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                });

                DisplayRuby = true;
                DisplayRomaji = true;
            }

            public void UpdateTimeTagCursoe(TimeTag cursor)
            {
                timeTagCursorContainer.Clear();
                if (TimeTagsBindable.Value.Contains(cursor))
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

            protected override void LoadComplete()
            {
                base.LoadComplete();

                TimeTagsBindable.BindValueChanged(e =>
                {
                    UpdateTimeTags();
                }, true);
            }

            protected override void ApplyFont(KaraokeFont font)
            {
                base.ApplyFont(font);

                if (TimeTagsBindable.Value == null)
                    return;

                // todo : need to delay until karaoke text has been calculated.
                ScheduleAfterChildren(UpdateTimeTags);
            }

            protected override void ApplyLayout(KaraokeLayout layout)
            {
                base.ApplyLayout(layout);
                Padding = new MarginPadding(0);
            }

            protected override void UpdateStartTimeStateTransforms()
            {
                // Do not fade-in / fade-out while changing armed state.
            }

            public override double LifetimeStart
            {
                get => double.MinValue;
                set => base.LifetimeStart = double.MinValue;
            }

            public override double LifetimeEnd
            {
                get => double.MaxValue;
                set => base.LifetimeEnd = double.MaxValue;
            }

            protected void UpdateTimeTags()
            {
                timeTagContainer.Clear();
                var timeTags = TimeTagsBindable.Value;
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
                var index = Math.Min(timeTag.Index.Index, HitObject.Text.Length - 1);
                var percentage = timeTag.Index.State == TimeTagIndex.IndexState.Start ? 0 : 1;
                var position = karaokeText.GetPercentageWidth(index, index + 1, percentage);

                var timeTags = TimeTagsBindable.Value;
                var duplicatedTagAmount = timeTags.SkipWhile(t => t != timeTag).Count(x => x.Index == timeTag.Index) - 1;
                var spacing = duplicatedTagAmount * time_tag_spacing * (timeTag.Index.State == TimeTagIndex.IndexState.Start ? 1 : -1);
                return position + spacing;
            }
        }
    }
}
