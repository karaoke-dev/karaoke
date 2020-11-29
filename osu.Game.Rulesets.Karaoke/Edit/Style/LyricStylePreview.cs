// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    internal class LyricStylePreview : Container
    {
        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider, StyleManager manager)
        {
            Masking = true;
            CornerRadius = 15;
            FillMode = FillMode.Fit;
            FillAspectRatio = 1.25f;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background1,
                },
                new PreviewDrawableLyricLine(createDefaultLyricLine())
            };
        }

        private Lyric createDefaultLyricLine()
        {
            var startTime = Time.Current;
            const double duration = 1000000;

            return new Lyric
            {
                StartTime = startTime,
                Duration = duration,
                Text = "カラオケ！",
                TimeTags = TimeTagsUtils.ToTimeTagList(new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(0), startTime + 500 },
                    { new TimeTagIndex(1), startTime + 600 },
                    { new TimeTagIndex(2), startTime + 1000 },
                    { new TimeTagIndex(3), startTime + 1500 },
                    { new TimeTagIndex(4), startTime + 2000 },
                }),
                RubyTags = new[]
                {
                    new RubyTag
                    {
                        StartIndex = 0,
                        EndIndex = 1,
                        Text = "か"
                    },
                    new RubyTag
                    {
                        StartIndex = 2,
                        EndIndex = 3,
                        Text = "お"
                    }
                },
                RomajiTags = new[]
                {
                    new RomajiTag
                    {
                        StartIndex = 1,
                        EndIndex = 2,
                        Text = "ra"
                    },
                    new RomajiTag
                    {
                        StartIndex = 3,
                        EndIndex = 4,
                        Text = "ke"
                    }
                }
            };
        }

        public class PreviewDrawableLyricLine : DrawableLyric
        {
            private KaraokeFont style;

            public PreviewDrawableLyricLine(Lyric hitObject)
                : base(hitObject)
            {
            }

            /// <summary>
            /// A trick to add style, should eventually be removed
            /// </summary>
            public KaraokeFont PreviewStyle
            {
                get => style;
                set
                {
                    style = value;
                    ApplyFont(style);
                }
            }
        }
    }
}
