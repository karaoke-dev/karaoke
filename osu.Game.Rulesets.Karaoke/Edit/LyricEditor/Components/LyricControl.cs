// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Components
{
    public class LyricControl : Container
    {
        private readonly DrawableLyric drawableLyric;

        public Lyric Lyric { get; }

        public LyricControl(Lyric lyric)
        {
            Lyric = lyric;
            CornerRadius = 5;
            AutoSizeAxes = Axes.Y;
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
            public DrawableEditorLyric(Lyric lyric)
                : base(lyric)
            {
                DisplayRuby = true;
                DisplayRomaji = true;
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
        }
    }
}
