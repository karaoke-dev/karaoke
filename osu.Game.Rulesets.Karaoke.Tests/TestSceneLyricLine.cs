// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestSceneLyricLine : SkinnableTestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(DrawableLyricLine)
        };

        public TestSceneLyricLine()
        {
            AddStep("Default Lyric", () => SetContents(() => testSingle()));
        }

        private Drawable testSingle(bool auto = false, double timeOffset = 0)
        {
            var starttime = Time.Current + 1000 + timeOffset;
            var endTime = starttime + 2500;

            var lyric = new LyricLine
            {
                StartTime = starttime,
                EndTime = endTime,
                Text = "カラオケ！",
                TimeTags = new[]
                {
                    new TimeTag
                    {
                        StartTime = starttime + 500,
                    },
                    //カ
                    new TimeTag
                    {
                        StartTime = starttime + 600,
                    },
                    //ラ
                    new TimeTag
                    {
                        StartTime = starttime + 1000,
                    },
                    //オ
                    new TimeTag
                    {
                        StartTime = starttime + 1500,
                    },
                    //ケ
                    new TimeTag
                    {
                        StartTime = starttime + 2000,
                    },
                },
                RubyTags = new[]
                {
                    new RubyTag
                    {
                        StartIndex = 0,
                        EndIndex = 1,
                        Ruby = "か"
                    },
                    new RubyTag
                    {
                        StartIndex = 2,
                        EndIndex = 3,
                        Ruby = "お"
                    }
                },
                RomajiTags = new[]
                {
                    new RomajiTag
                    {
                        StartIndex = 1,
                        EndIndex = 2,
                        Romaji = "ra"
                    },
                    new RomajiTag
                    {
                        StartIndex = 3,
                        EndIndex = 4,
                        Romaji = "ke"
                    }
                }
            };

            lyric.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

            var drawable = CreateDrawableLyricLine(lyric, auto);

            foreach (var mod in Mods.Value.OfType<IApplicableToDrawableHitObjects>())
                mod.ApplyToDrawableHitObjects(new[] { drawable });

            return drawable;
        }

        private int depthIndex;

        protected virtual TestDrawableLyricLine CreateDrawableLyricLine(LyricLine lyric, bool auto)
            => new TestDrawableLyricLine(lyric, auto)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Depth = depthIndex++
            };

        protected class TestDrawableLyricLine : DrawableLyricLine
        {
            private readonly bool auto;

            public TestDrawableLyricLine(LyricLine h, bool auto)
                : base(h)
            {
                this.auto = auto;
            }
        }
    }
}
