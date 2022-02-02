// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Layouts
{
    public class LayoutGenerator
    {
        protected LayoutGeneratorConfig Config { get; }

        private readonly IDictionary<int, LyricLayout[]> layouts = new Dictionary<int, LyricLayout[]>();

        public LayoutGenerator(LayoutGeneratorConfig config)
        {
            Config = config;

            // todo: implement the better algorithm.
            layouts.Add(new KeyValuePair<int, LyricLayout[]>(1, new[]
            {
                new LyricLayout(),
                new LyricLayout()
            }));

            layouts.Add(new KeyValuePair<int, LyricLayout[]>(2, new[]
            {
                new LyricLayout(),
                new LyricLayout(),
                new LyricLayout()
            }));

            layouts.Add(new KeyValuePair<int, LyricLayout[]>(3, new[]
            {
                new LyricLayout(),
                new LyricLayout(),
                new LyricLayout(),
                new LyricLayout()
            }));
        }

        public void ApplyLayout(Lyric[] lyrics, LocalLayout layout = LocalLayout.CycleTwo)
        {
            switch (layout)
            {
                case LocalLayout.CycleTwo:
                    ApplyLayout(lyrics, layouts[1]);
                    return;

                case LocalLayout.CycleThree:
                    ApplyLayout(lyrics, layouts[2]);
                    return;

                case LocalLayout.CycleFour:
                    ApplyLayout(lyrics, layouts[3]);
                    return;

                default:
                    throw new ArgumentOutOfRangeException(nameof(layout));
            }
        }

        public void ApplyLayout(Lyric[] lyrics, LyricLayout[] layouts)
        {
            if (lyrics == null)
                return;

            if (layouts == null)
                throw new ArgumentNullException(nameof(layouts));

            assignLayoutArrangement(lyrics, layouts);
            assignLyricTime(lyrics, layouts);
        }

        private void assignLayoutArrangement(IList<Lyric> lyrics, LyricLayout[] layouts)
        {
            // todo : use layout setting
            // Apply layout index
            for (int i = 0; i < lyrics.Count; i++)
            {
                var previousLyric = i >= 1 ? lyrics[i - 1] : null;
                var lyric = lyrics[i];

                // Force change layout
                if (lyric.StartTime - previousLyric?.EndTime > Config.NewLyricLineTime)
                    lyric.LayoutIndex = 1;
                // Change to next layout
                else if (previousLyric?.LayoutIndex == 1)
                    lyric.LayoutIndex = 2;
                // Change to first layout index
                else
                    lyric.LayoutIndex = 1;
            }
        }

        private void assignLyricTime(IList<Lyric> lyrics, LyricLayout[] layouts)
        {
            // Reset working time
            lyrics.ForEach(h => h.InitialWorkingTime());

            // Apply start time
            int numberOfLine = layouts.Length;

            for (int i = 0; i < lyrics.Count; i++)
            {
                var lastLyric = i >= numberOfLine ? lyrics[i - numberOfLine] : null;
                var lyric = lyrics[i];

                if (lastLyric == null)
                    continue;

                // Adjust start time and end time
                double lyricEndTime = lyric.EndTime;
                lyric.StartTime = lastLyric.EndTime + 1000;

                // Should re-assign duration here
                lyric.Duration = lyricEndTime - lyric.StartTime;
            }
        }
    }
}
