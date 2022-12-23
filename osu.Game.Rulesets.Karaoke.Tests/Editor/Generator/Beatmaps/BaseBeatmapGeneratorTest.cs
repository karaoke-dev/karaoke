// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Beatmaps
{
    public abstract class BaseBeatmapGeneratorTest<TGenerator, TObject, TConfig>
        : BaseGeneratorTest<TConfig>
        where TGenerator : BeatmapPropertyGenerator<TObject, TConfig> where TConfig : IHasConfig<TConfig>, new()
    {
        protected static TGenerator GenerateGenerator(TConfig config)
        {
            if (Activator.CreateInstance(typeof(TGenerator), config) is not TGenerator generator)
                throw new ArgumentNullException(nameof(generator));

            return generator;
        }

        protected static void CheckCanGenerate(KaraokeBeatmap beatmap, bool canGenerate, TConfig config)
        {
            var generator = GenerateGenerator(config);

            CheckCanGenerate(beatmap, canGenerate, generator);
        }

        protected static void CheckCanGenerate(KaraokeBeatmap beatmap, bool canGenerate, TGenerator generator)
        {
            bool actual = generator.CanGenerate(beatmap);
            Assert.AreEqual(canGenerate, actual);
        }

        protected void CheckGenerateResult(KaraokeBeatmap beatmap, TObject expected, TConfig config)
        {
            var generator = GenerateGenerator(config);

            CheckGenerateResult(beatmap, expected, generator);
        }

        protected void CheckGenerateResult(KaraokeBeatmap beatmap, TObject expected, TGenerator generator)
        {
            // create time tag and actually time tag.
            var actual = generator.Generate(beatmap);
            AssertEqual(expected, actual);
        }

        protected abstract void AssertEqual(TObject expected, TObject actual);
    }
}
