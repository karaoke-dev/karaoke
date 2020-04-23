// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Components
{
    public class LyricControl : Container
    {
        public LyricControl(LyricLine lyric)
        {
            Masking = true;
            CornerRadius = 5;
            AutoSizeAxes = Axes.Y;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new DrawableLyricLine(lyric)
            };
        }

        public class DrawableEditorLyricLine : DrawableLyricLine
        {
            public DrawableEditorLyricLine(LyricLine lyric)
                : base(lyric)
            {
            }

            protected override void ApplyLayout(KaraokeLayout layout)
            {
                base.ApplyLayout(new KaraokeLayout
                {
                    Name = "Edit layout",
                    Alignment = Anchor.CentreLeft
                });
            }

            public override double LifetimeStart
            {
                get => base.LifetimeStart;
                set => base.LifetimeStart = value;
            }

            public override double LifetimeEnd
            {
                get => base.LifetimeEnd;
                set => base.LifetimeEnd = value;
            }
        }
    }
}
