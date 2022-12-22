// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps
{
    public abstract class BaseBeatmapDetectorTest<TDetector, TObject, TConfig>
        : BaseGeneratorTest<TConfig>
        where TDetector : class, IBeatmapPropertyDetector<TObject> where TConfig : IHasConfig<TConfig>, new()
    {
        protected static TDetector GenerateDetector(TConfig config)
        {
            if (Activator.CreateInstance(typeof(TDetector), config) is not TDetector detector)
                throw new ArgumentNullException(nameof(detector));

            return detector;
        }

        protected static void CheckCanDetect(KaraokeBeatmap beatmap, bool canDetect, TConfig config)
        {
            var detector = GenerateDetector(config);

            CheckCanDetect(beatmap, canDetect, detector);
        }

        protected static void CheckCanDetect(KaraokeBeatmap beatmap, bool canDetect, TDetector detector)
        {
            bool actual = detector.CanDetect(beatmap);
            Assert.AreEqual(canDetect, actual);
        }

        protected void CheckDetectResult(KaraokeBeatmap beatmap, TObject expected, TConfig config)
        {
            var detector = GenerateDetector(config);

            CheckDetectResult(beatmap, expected, detector);
        }

        protected void CheckDetectResult(KaraokeBeatmap beatmap, TObject expected, TDetector detector)
        {
            // create time tag and actually time tag.
            var actual = detector.Detect(beatmap);
            AssertEqual(expected, actual);
        }

        protected abstract void AssertEqual(TObject expected, TObject actual);
    }
}
