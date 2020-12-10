// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    [TestFixture]
    public class TestSceneLyric : KaraokeSkinnableTestScene
    {
        public TestSceneLyric()
        {
            AddStep("Default Lyric", () => SetContents(() => testSingle()));
        }

        private Drawable testSingle(bool auto = false, double timeOffset = 0)
        {
            var startTime = Time.Current + 1000 + timeOffset;
            const double duration = 2500;

            var lyric = new Lyric
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
                },
            };

            var defaultLanguage = new CultureInfo("en-US");
            lyric.Translates.Add(defaultLanguage, "karaoke");

            lyric.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());

            var drawable = CreateDrawableLyric(lyric, auto);
            drawable.DisplayTranslateLanguage = defaultLanguage;

            foreach (var mod in SelectedMods.Value.OfType<IApplicableToDrawableHitObjects>())
                mod.ApplyToDrawableHitObjects(new[] { drawable });

            return drawable;
        }

        private int depthIndex;

        protected virtual TestDrawableLyric CreateDrawableLyric(Lyric lyric, bool auto)
            => new TestDrawableLyric(lyric, auto)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Depth = depthIndex++
            };

        protected class TestDrawableLyric : DrawableLyric
        {
            public TestDrawableLyric(Lyric h, bool auto)
                : base(h)
            {
            }
        }
    }
}
