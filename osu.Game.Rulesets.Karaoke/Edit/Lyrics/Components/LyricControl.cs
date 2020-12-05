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

        [BackgroundDependencyLoader]
        private void load(IFrameBasedClock framedClock)
        {
            drawableLyric.Clock = framedClock;
        }

        public class DrawableEditorLyric : DrawableLyric
        {
            private readonly Container timeTagContainer;

            public DrawableEditorLyric(Lyric lyric)
                : base(lyric)
            {
                AddInternal(timeTagContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                });

                DisplayRuby = true;
                DisplayRomaji = true;
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
                UpdateTimeTags();
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
                foreach (var timeTag in TimeTagsBindable.Value)
                {
                    var index = Math.Min(timeTag.Item1.Index, HitObject.Text.Length - 1);
                    var percentage = timeTag.Item1.State == TimeTagIndex.IndexState.Start ? 0 : 1;
                    var position = karaokeText.GetPercentageWidth(index, index + 1, percentage);

                    timeTagContainer.Add(new DrawableTimeTag(timeTag)
                    {
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        X = position
                    });
                }
            }
        }
    }
}
